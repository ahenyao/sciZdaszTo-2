using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using ZdaszToApp.ViewModels;

namespace ZdaszToApp.Views;

public partial class LoginView : UserControl
{
    private DispatcherTimer? _spinnerTimer;
    private double _rotationAngle;

    public LoginView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("[LoginView] OnLoaded called");
        if (DataContext is LoginViewModel vm)
        {
            Console.WriteLine("[LoginView] DataContext is LoginViewModel");
            vm.PropertyChanged += (s, args) =>
            {
                Console.WriteLine("[LoginView] PropertyChanged: " + args.PropertyName);
                if (args.PropertyName == nameof(LoginViewModel.IsLoading))
                {
                    Console.WriteLine("[LoginView] IsLoading changed to: " + vm.IsLoading);
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
        Console.WriteLine("[LoginView] StartSpinner called");
        _rotationAngle = 0;
        _spinnerTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(30)
        };
        _spinnerTimer.Tick += (s, e) =>
        {
            Console.WriteLine("[LoginView] Spinner tick, Spinner null:" + (Spinner == null) + ", RenderTransform null:" + (Spinner?.RenderTransform == null));
            _rotationAngle = (_rotationAngle + 10) % 360;
            if (Spinner?.RenderTransform is RotateTransform rt)
            {
                rt.Angle = _rotationAngle;
            }
        };
        _spinnerTimer.Start();
        Console.WriteLine("[LoginView] Spinner started");
    }

    private void StopSpinner()
    {
        Console.WriteLine("[LoginView] StopSpinner called");
        _spinnerTimer?.Stop();
        _spinnerTimer = null;
    }
}
