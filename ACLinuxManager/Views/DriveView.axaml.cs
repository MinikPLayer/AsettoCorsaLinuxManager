using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ACLinuxManager.Dialogs;
using ACLinuxManager.Settings;
using AcUtils.ContentManagers;
using AcUtils.DataTypes.IniFiles;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DynamicData;
using ReactiveUI;

namespace ACLinuxManager.Views;

public class DriveViewViewModel : ReactiveObject
{
    public RaceIni raceIni = new RaceIni();
    
    public ObservableCollection<Track>? Tracks => DriveView.Tracks;

    private string _selectedConfigPreviewPath = "";
    public string SelectedConfigPreviewPath
    {
        get => _selectedConfigPreviewPath;
        set => this.RaiseAndSetIfChanged(ref _selectedConfigPreviewPath, value);
    }
    
    private string _selectedConfigOutlinePath = "";
    public string SelectedConfigOutlinePath
    {
        get => _selectedConfigOutlinePath;
        set => this.RaiseAndSetIfChanged(ref _selectedConfigOutlinePath, value);
    }

    private Track.Config? _selectedConfig;
    public Track.Config? SelectedConfig
    {
        get => _selectedConfig;
        set
        {
            if (value is null)
                return;

            this.RaiseAndSetIfChanged(ref _selectedConfig, value);

            SelectedConfigPreviewPath = value.Value.PreviewPath;
            SelectedConfigOutlinePath = value.Value.OutlinePath;
        }
    }

    public ObservableCollection<Track.Config> SelectedTrackConfigs { get; set; } = new();

    private Track? _selectedTrack;
    public Track? SelectedTrack
    {
        get => _selectedTrack;
        set
        {
            if (value == null)
                return;
            
            this.RaiseAndSetIfChanged(ref _selectedTrack, value);
            raceIni.RaceTrackName = value.NameId;
            raceIni.RaceTrackConfig = value.Configs[0].NameId;

            SelectedTrackConfigs.Clear();
            SelectedTrackConfigs.AddRange(value.Configs);
            SelectedConfig = value.Configs[0];
        }
    }
}

public partial class DriveView : UserControl
{
    public static readonly ObservableCollection<Track> Tracks = new();
    
    public DriveViewViewModel ViewModel => (DriveViewViewModel)DataContext!;

    async void UpdateTracks()
    {
        await Task.Run(async () =>
        {
            Tracks.Clear();
            var newTracks = TracksManager.GetAllTracks(GameInfoSettings.TrackContentPath);
            foreach (var track in newTracks)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var str = track.ToString();
                    var i = 0;
                    for (; i < Tracks.Count; i++)
                        if (string.CompareOrdinal(Tracks[i].ToString(), str) > 0)
                            break;

                    Tracks.Insert(i, track);
                    ViewModel.SelectedTrack ??= track;
                });
            }
        });
    }

    public DriveView()
    {
        UpdateTracks();

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void DriveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var msgDialog = new MessageValueDialog(ViewModel.raceIni.ToIni().ToString(), "Race .ini");
        msgDialog.ShowDialog(MainWindow.Singleton);

        // var dialog = new GameLaunchingDialog();
        // _ = dialog.ShowDialogHideOwner(MainWindow.Singleton);
    }
}