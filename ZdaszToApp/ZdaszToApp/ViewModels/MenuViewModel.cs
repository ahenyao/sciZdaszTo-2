using CommunityToolkit.Mvvm.ComponentModel;
using System;
using ZdaszToApp.Services;

namespace ZdaszToApp.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    [ObservableProperty] private string _primaryGreen = "#8BC34A";
    [ObservableProperty] private string _primaryBlue = "#03A9F4";
    [ObservableProperty] private string _primaryCyan = "#00BCD4";
    [ObservableProperty] private string _bgTop = "#E8F5E9";
    [ObservableProperty] private string _bgMiddle = "#C8E6C9";
    [ObservableProperty] private string _bgBottom = "#A5D6A7";
    [ObservableProperty] private string _cardBackground = "#FFFFFF";
    [ObservableProperty] private string _textPrimary = "#212121";
    [ObservableProperty] private string _textSecondary = "#757575";
    [ObservableProperty] private bool _isDarkMode;

    public event Action? OnInf02Click;
    public event Action? OnInf03Click;
    public event Action? OnInf04Click;
    public event Action? OnSettingsClick;

    public MenuViewModel()
    {
        ThemeService.Instance.PropertyChanged += OnThemeChanged;
        ApplyTheme(ThemeService.Instance.IsDarkMode);
    }

    private void OnThemeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ThemeService.IsDarkMode))
        {
            ApplyTheme(ThemeService.Instance.IsDarkMode);
        }
    }

    private void ApplyTheme(bool isDark)
    {
        IsDarkMode = isDark;

        if (isDark)
        {
            PrimaryGreen = "#4CAF50";
            PrimaryBlue = "#00BCD4";
            PrimaryCyan = "#26C6DA";
            BgTop = "#0D1B2A";
            BgMiddle = "#1B263B";
            BgBottom = "#1B263B";
            CardBackground = "#1E1E3F";
            TextPrimary = "#E8F5E9";
            TextSecondary = "#81C784";
        }
        else
        {
            PrimaryGreen = "#8BC34A";
            PrimaryBlue = "#03A9F4";
            PrimaryCyan = "#00BCD4";
            BgTop = "#E8F5E9";
            BgMiddle = "#C8E6C9";
            BgBottom = "#A5D6A7";
            CardBackground = "#FFFFFF";
            TextPrimary = "#212121";
            TextSecondary = "#757575";
        }
    }
}