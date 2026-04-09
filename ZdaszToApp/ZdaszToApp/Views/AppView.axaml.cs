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
        if (Taskbar != null && _vm != null)
        {
            Taskbar.OnSettingsClicked += OnSettingsOpen;
        }
        if (Ranking != null)
        {
            Ranking.OnBackToMenu += OnRankingBack;
        }
        if (Settings != null)
        {
            Settings.OnLogoutRequested += () =>
            {
                System.Diagnostics.Debug.WriteLine("[AppView] OnLogoutRequested - START");
                System.Diagnostics.Debug.WriteLine($"[AppView] Main: {Main?.GetType().Name}, Login: {Login?.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"[AppView] Main.IsVisible: {Main?.IsVisible}, Login.IsVisible: {Login?.IsVisible}");
                
                if (Main != null && Login != null)
                {
                    Services.AuthService.Instance.ClearCredentials();
                    Main.IsVisible = false;
                    Settings.IsVisible = false;
                    Login.IsVisible = true;
                    Login.StopSpinner();
                    System.Diagnostics.Debug.WriteLine("[AppView] Wylogowano");
                }
            };
        }
    }
    
    private void OnSettingsOpen()
    {
        if (Main != null && Settings != null)
        {
            if (Settings is ILoadable loadable)
            {
                loadable.ApplyThemeOnLoad();
            }
            Main.IsVisible = false;
            Settings.IsVisible = true;
        }
    }
    
    private void OnRankingBack()
    {
        if (Main != null && Ranking != null)
        {
            Main.IsVisible = true;
            Ranking.IsVisible = false;
        }
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
                
                vm.LoginViewModel.IsLoading = true;
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
