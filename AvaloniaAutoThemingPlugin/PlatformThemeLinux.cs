using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Media;

namespace Avalonia.Themes;

public class PlatformThemeLinux : PlatformTheme
{
    protected DateTime lastModifiedTime = DateTime.MinValue;
    protected Color lastColor = DefaultBackgroundColor;
    protected override Color _GetBackgroundColor()
    {
        // Check for KDE
        const string constFilePath = ".config/kdeglobals";
        const string kdeCategoryPattern = "[Colors:Window]";
        const string kdeSearchPattern = "BackgroundNormal=";

        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), constFilePath);
        
        if (File.Exists(filePath))
        {
            FileStream handle;
            try
            {
                var lastWriteTime = File.GetLastWriteTime(filePath);
                if (lastWriteTime == lastModifiedTime)
                    return lastColor;

                lastModifiedTime = lastWriteTime;
                handle = File.Open(filePath, FileMode.Open);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot access kdeglobals file at " + filePath);
                return DefaultBackgroundColor;
            }
            
            using (var reader = new StreamReader(handle))
            {
                var inTargetCategory = false;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        continue;

                    if (line.StartsWith("["))
                    {
                        inTargetCategory = line.StartsWith(kdeCategoryPattern);
                        continue;
                    }
                    
                    if (inTargetCategory && line.StartsWith(kdeSearchPattern))
                    {
                        line = line.Substring(kdeSearchPattern.Length);
                        var colors = line.Split(',');
                        if (colors.Length != 3)
                        {
                            Debug.WriteLine("Error parsing kdeglobals file");
                            return DefaultBackgroundColor;
                        }

                        byte[] colorBytes = new byte[3];
                        for (int i = 0; i < 3; i++)
                        {
                            if (byte.TryParse(colors[i], out byte b))
                                colorBytes[i] = b;
                            else
                            {
                                Debug.WriteLine("Error parsing kdeglobals file - cannot convert value to byte");
                                return DefaultBackgroundColor;
                            }
                        }

                        lastColor = Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]);
                        return lastColor;
                    }
                }
            }
        }
        
        Debug.WriteLine("KDE configuration file doesn't exist, returning default value");
        return DefaultBackgroundColor;
    }

    protected override bool _IsDarkMode()
    {
        var color = this._GetBackgroundColor();
        var sum = color.R + color.B + color.G;
        return sum < (255 * 3) / 2;
    }
}