using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Services;

namespace ZdaszToApp.Views;

public partial class LoginView : UserControl, ILoadable
{
    private DispatcherTimer? _spinnerTimer;
    private double _rotationAngle;

    public LoginView()
    {
        InitializeComponent();
        
        ThemeService.Instance.PropertyChanged += OnThemeChanged;
        ApplyTheme();
        
        Loaded += (s, e) =>
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(LoginViewModel.IsLoading))
                    {
                        if (vm.IsLoading)
                            StartSpinner();
                        else
                            StopSpinner();
                    }
                };
            }
        };
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
        var mainGrid = this.FindControl<Grid>("MainGrid");
        var formBorder = this.FindControl<Border>("FormBorder");
        var inputBorder = this.FindControl<Border>("InputBorder");
        
        if (mainGrid != null)
        {
            mainGrid.Classes.Clear();
            mainGrid.Classes.Add(isDark ? "dark" : "light");
            
            var bg = Application.Current?.FindResource(isDark ? "DarkPageBackground" : "PageBackground") as IBrush;
            if (bg != null)
                mainGrid.Background = bg;
        }

        if (formBorder != null)
        {
            var overlay = Application.Current?.FindResource(isDark ? "OverlayDark" : "GlassOverlay") as IBrush;
            if (overlay != null)
                formBorder.Background = overlay;
        }

        if (inputBorder != null)
        {
            var card = Application.Current?.FindResource(isDark ? "DarkGlossyCard" : "GlossyCard") as IBrush;
            if (card != null)
                inputBorder.Background = card;
            else
                inputBorder.Background = isDark 
                    ? new SolidColorBrush(Avalonia.Media.Color.Parse("#1E1E3F"))
                    : new SolidColorBrush(Colors.White);
        }
    }

    public void ApplyThemeOnLoad()
    {
        ApplyTheme();
    }

    public void StartSpinner()
    {
        _spinnerTimer?.Stop();
        _rotationAngle = 0;
        _spinnerTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _spinnerTimer.Tick += (s, e) =>
        {
            if (Spinner == null || _spinnerTimer == null) return;
            
            _rotationAngle = (_rotationAngle + 10) % 360;
            Spinner.RenderTransform = new RotateTransform(_rotationAngle);
        };
        _spinnerTimer.Start();
    }

    public void StopSpinner()
    {
        if (_spinnerTimer != null)
        {
            _spinnerTimer.Stop();
            _spinnerTimer = null;
        }
        
        if (DataContext is LoginViewModel vm)
        {
            vm.IsLoading = false;
        }
    }
}
