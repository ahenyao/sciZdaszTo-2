using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using ZdaszToApp.Services;
using ZdaszToApp.ViewModels;
using System;
using System.Diagnostics;

namespace ZdaszToApp.Views;

public partial class Taskbar : UserControl, ILoadable
{
    public Taskbar()
    {
        InitializeComponent();
        
        ThemeService.Instance.PropertyChanged += OnThemeChanged;
        ApplyTheme();
    }

    private void OnThemeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ThemeService.IsDarkMode))
        {
            ApplyTheme();
        }
    }

    private void ApplyTheme()
    {
        var isDark = ThemeService.Instance.IsDarkMode;
        var mainGrid = this.FindControl<Grid>("TaskbarGrid");
        
        if (mainGrid != null)
        {
            mainGrid.Classes.Clear();
            mainGrid.Classes.Add(isDark ? "dark" : "light");
            
            var bg = Application.Current?.FindResource(isDark ? "DarkBgSecondaryBrush" : "FrutigerLightGrayBrush") as IBrush;
            if (bg != null)
                mainGrid.Background = bg;
        }
    }

    public void ApplyThemeOnLoad()
    {
        ApplyTheme();
    }

    private void OnSettingsClick(object? sender, RoutedEventArgs e)
    {
        OnSettingsClicked?.Invoke();
    }

    public event Action? OnSettingsClicked;
}
