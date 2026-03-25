using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Services;
using System.Diagnostics;
using System;

namespace ZdaszToApp.Views;

    public partial class AppView : UserControl
{
    private MainWindowViewModel? _vm;

    public AppView()
    {
        InitializeComponent();
        Debug.WriteLine("[AppView] Zaladowano AppView (telefon)");
        DataContextChanged += OnDataContextChanged;
    }

    private void OnMainLoaded(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("[AppView] Main DockPanel loaded");
        if (Taskbar != null && _vm != null)
        {
            Debug.WriteLine("[AppView] Subscribing to Taskbar event");
            Taskbar.OnUserButtonClicked += OnUserLogout;
        }
    }

    private void OnUserLogout()
    {
        Debug.WriteLine("[AppView] OnUserLogout - START");
        if (_vm == null) return;
        
        AuthService.Instance.ClearCredentials();
        _vm.LoginViewModel.Reset();
        Main.IsVisible = false;
        Login.IsVisible = true;
        Debug.WriteLine("[AppView] Wylogowano");
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        Debug.WriteLine("[AppView] DataContext ustawiony");
        if (DataContext is MainWindowViewModel vm)
        {
            _vm = vm;
            Login.DataContext = vm.LoginViewModel;
            AddAccount.DataContext = vm.AddAccountViewModel;
            
            var authService = AuthService.Instance;
            if (authService.HasSavedCredentials())
            {
                vm.LoginViewModel.Username = authService.SavedUsername;
                vm.LoginViewModel.Password = authService.SavedPassword;
                
                DispatcherTimer.RunOnce(() =>
                {
                    vm.LoginViewModel.LoginCommand.Execute(null);
                }, TimeSpan.FromMilliseconds(100));
            }
            
            vm.LoginViewModel.OnCreateAccountClicked += () =>
            {
                Login.IsVisible = false;
                AddAccount.IsVisible = true;
                Debug.WriteLine("[AppView] Przelaczono na AddAccountView");
            };
            
            vm.LoginViewModel.OnLoginSuccess += () =>
            {
                Main.IsVisible = true;
                Login.IsVisible = false;
                AddAccount.IsVisible = false;
                Debug.WriteLine("[AppView] Zaladowano MenuView (Main)");
            };
            
            vm.AddAccountViewModel.OnLoginSuccess += () =>
            {
                Main.IsVisible = true;
                Login.IsVisible = false;
                AddAccount.IsVisible = false;
                Debug.WriteLine("[AppView] Zaladowano MenuView po rejestracji (Main)");
            };
            
            vm.AddAccountViewModel.OnGoBackToLogin += () =>
            {
                AddAccount.IsVisible = false;
                Login.IsVisible = true;
                Debug.WriteLine("[AppView] Przelaczono na LoginView");
            };
        }
    }
}
