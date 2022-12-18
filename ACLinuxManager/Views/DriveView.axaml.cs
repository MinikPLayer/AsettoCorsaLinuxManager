using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ACLinuxManager.Dialogs;
using ACLinuxManager.Settings;
using AcUtils.ContentManagers;
using AcUtils.DataTypes;
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
    public readonly RaceIni RaceIni = new RaceIni();
    
    public ObservableCollection<Track> Tracks => DriveView.Tracks;
    public ObservableCollection<Car> Cars => DriveView.Cars;

    private string _selectedCarSkinLiveryPath = "";
    public string SelectedCarSkinLiveryPath
    {
        get => _selectedCarSkinLiveryPath;
        set => this.RaiseAndSetIfChanged(ref _selectedCarSkinLiveryPath, value);
    }
    
    private string _selectedCarSkinPreviewPath = "";
    public string SelectedCarSkinPreviewPath
    {
        get => _selectedCarSkinPreviewPath;
        set => this.RaiseAndSetIfChanged(ref _selectedCarSkinPreviewPath, value);
    }
    
    private Car.Skin? _selectedCarSkin;
    public Car.Skin? SelectedCarSkin
    {
        get => _selectedCarSkin;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedCarSkin, value);

            if (value == null)
            {
                RaceIni.Cars[0].Skin = "";
                return;
            }

            RaceIni.Cars[0].Skin = value.NameId;
            
            SelectedCarSkinLiveryPath = value.LiveryPath;
            SelectedCarSkinPreviewPath = value.PreviewPath;
        }
    }

    public ObservableCollection<Car.Skin> CarSkinsAvailable { get; set; } = new();

    private Car? _selectedCar;
    public Car? SelectedCar
    {
        get => _selectedCar;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedCar, value);
            if (value == null)
            {
                RaceIni.RaceCarModel = RaceIni.Cars[0].Model = "";
                return;
            }
            
            RaceIni.RaceCarModel = RaceIni.Cars[0].Model = value.NameId;

            CarSkinsAvailable.Clear();
            CarSkinsAvailable.AddRange(value.Skins);
            SelectedCarSkin = value.Skins[0];
        }
    }
    
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
            this.RaiseAndSetIfChanged(ref _selectedConfig, value);
            if (value is null)
            {
                RaceIni.RaceTrackConfig = "";
                return;
            }

            RaceIni.RaceTrackConfig = value.Value.NameId;

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
            this.RaiseAndSetIfChanged(ref _selectedTrack, value);
            if (value == null)
            {
                RaceIni.RaceTrackName = "";
                return;
            }
            RaceIni.RaceTrackName = value.NameId;

            SelectedTrackConfigs.Clear();
            SelectedTrackConfigs.AddRange(value.Configs);
            SelectedConfig = value.Configs[0];
        }
    }
}

public partial class DriveView : UserControl
{
    public static readonly ObservableCollection<Track> Tracks = new();
    public static readonly ObservableCollection<Car> Cars = new();
    
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
        
        await Task.Run(async () =>
        {
            Cars.Clear();
            var newCars = CarsManager.GetAllCars(GameInfoSettings.CarsContentPath);
            foreach (var car in newCars)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var str = car.ToString();
                    var i = 0;
                    for (; i < Cars.Count; i++)
                        if (string.CompareOrdinal(Cars[i].ToString(), str) > 0)
                            break;

                    Cars.Insert(i, car);
                    ViewModel.SelectedCar ??= car;
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
        var msgDialog = new MessageValueDialog(ViewModel.RaceIni.ToIni().ToString(), "Race .ini");
        msgDialog.ShowDialog(MainWindow.Singleton);

        // var dialog = new GameLaunchingDialog();
        // _ = dialog.ShowDialogHideOwner(MainWindow.Singleton);
    }
}