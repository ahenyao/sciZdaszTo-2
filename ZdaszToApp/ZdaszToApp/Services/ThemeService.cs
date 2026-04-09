using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ZdaszToApp.Services;

public class ThemeService : INotifyPropertyChanged
{
    private static ThemeService? _instance;
    public static ThemeService Instance => _instance ??= new ThemeService();

    private bool _isDarkMode;
    
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLightMode));
                SaveToFile();
            }
        }
    }

    public bool IsLightMode => !_isDarkMode;

    public event PropertyChangedEventHandler? PropertyChanged;

    private ThemeService()
    {
        LoadFromFile();
    }

    public ThemeService CreateInstance()
    {
        return Instance;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string GetFilePath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "ZdaszToApp");
        Directory.CreateDirectory(appFolder);
        return Path.Combine(appFolder, "theme.json");
    }

    private void SaveToFile()
    {
        try
        {
            var data = new ThemeData
            {
                IsDarkMode = _isDarkMode
            };
            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(GetFilePath(), json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ThemeService] SaveToFile error: {ex.Message}");
        }
    }

    private void LoadFromFile()
    {
        try
        {
            var path = GetFilePath();
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<ThemeData>(json);
                if (data != null)
                {
                    _isDarkMode = data.IsDarkMode;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ThemeService] LoadFromFile error: {ex.Message}");
        }
    }

    private class ThemeData
    {
        public bool IsDarkMode { get; set; }
    }
}
