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

public partial class AddAccountView : UserControl, ILoadable
{
    private DispatcherTimer? _spinnerTimer;
    private double _rotationAngle;

    public AddAccountView()
    {
        InitializeComponent();
        
        ThemeService.Instance.PropertyChanged += OnThemeChanged;
        ApplyTheme();
        
        Loaded += OnLoaded;
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

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is AddAccountViewModel vm)
        {
            vm.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == nameof(AddAccountViewModel.IsLoading))
                {
                    if (vm.IsLoading)
                        StartSpinner();
                    else
                        StopSpinner();
                }
            };
        }
    }

    private void StartSpinner()
    {
        _rotationAngle = 0;
        _spinnerTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(30)
        };
        _spinnerTimer.Tick += (s, e) =>
        {
            _rotationAngle = (_rotationAngle + 10) % 360;
            if (Spinner?.RenderTransform is RotateTransform rt)
            {
                rt.Angle = _rotationAngle;
            }
        };
        _spinnerTimer.Start();
    }

    private void StopSpinner()
    {
        _spinnerTimer?.Stop();
        _spinnerTimer = null;
    }
}
