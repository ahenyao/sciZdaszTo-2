using Avalonia.Controls;
using Avalonia.Threading;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Views;
using ZdaszToApp.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ZdaszToApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var vm = new MainWindowViewModel();
        DataContext = vm;
        
        Login.DataContext = vm.LoginViewModel;
        AddAccount.DataContext = vm.AddAccountViewModel;
        this.Menu.DataContext = vm.MenuViewModel;
        
        vm.LoginViewModel.OnCreateAccountClicked += () =>
        {
            Login.IsVisible = false;
            AddAccount.IsVisible = true;
        };
        
        vm.LoginViewModel.OnLoginSuccess += () =>
        {
            Main.IsVisible = true;
            Login.IsVisible = false;
            AddAccount.IsVisible = false;
        };
        
        vm.AddAccountViewModel.OnLoginSuccess += () =>
        {
            Main.IsVisible = true;
            Login.IsVisible = false;
            AddAccount.IsVisible = false;
        };
        
        vm.AddAccountViewModel.OnGoBackToLogin += () =>
        {
            AddAccount.IsVisible = false;
            Login.IsVisible = true;
        };
        
        vm.OnAutoLoginSuccess += () =>
        {
            var loginView = this.FindControl<LoginView>("Login");
            loginView?.StopSpinner();
            
            Main.IsVisible = true;
            Login.IsVisible = false;
            AddAccount.IsVisible = false;
        };

        vm.OnAutoLoginStart += () =>
        {
            DispatcherTimer.RunOnce(() =>
            {
                var loginView = this.FindControl<LoginView>("Login");
                if (loginView != null)
                {
                    loginView.DataContext = vm.LoginViewModel;
                    vm.LoginViewModel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(LoginViewModel.IsLoading))
                        {
                            if (vm.LoginViewModel.IsLoading)
                                loginView.StartSpinner();
                            else
                                loginView.StopSpinner();
                        }
                    };
                    loginView.StartSpinner();
                }
            }, TimeSpan.FromMilliseconds(800));
        };
        
        vm.LoginViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(LoginViewModel.IsLoading))
            {
                var loginView = this.FindControl<LoginView>("Login");
                if (loginView != null)
                {
                    if (vm.LoginViewModel.IsLoading)
                    {
                        loginView.StartSpinner();
                    }
                    else
                    {
                        loginView.StopSpinner();
                    }
                }
            }
        };
        
        if (AuthService.Instance.HasSavedCredentials())
        {
            vm.LoginViewModel.IsLoading = true;
            DispatcherTimer.RunOnce(() =>
            {
                var loginView = this.FindControl<LoginView>("Login");
                loginView?.StartSpinner();
            }, TimeSpan.FromMilliseconds(100));
        }
        
        ApplyThemeOnLoad();
        
        var mainDock = this.FindControl<DockPanel>("Main");
        var taskbar = this.FindControl<Taskbar>("Taskbar");
        
        if (taskbar != null)
        {
            taskbar.OnSettingsClicked += () =>
            {
                if (Settings is ILoadable loadable)
                {
                    loadable.ApplyThemeOnLoad();
                }
                Main.IsVisible = false;
                Settings.IsVisible = true;
            };
        }
            
        var ranking = this.FindControl<RankingView>("Ranking");
        if (ranking != null)
        {
            ranking.OnBackToMenu += () =>
            {
                Main.IsVisible = true;
                Ranking.IsVisible = false;
            };
        }
        
        var settings = this.FindControl<SettingsView>("Settings");
        if (settings != null)
        {
            settings.OnLogoutRequested += () =>
            {
                DoLogout();
            };
        }
        
        Services.ApiService.Instance.OnTokenInvalid += OnTokenInvalid;

        vm.OnLogoutRequested += () =>
        {
            DoLogout();
        };
    }
    
    private void OnTokenInvalid()
    {
        Debug.WriteLine("[MainWindow] Token invalid - logging out");
        
        if (DataContext is MainWindowViewModel vm)
        {
            vm.LogoutCommand.Execute(null);
        }
    }
    
    private void ApplyThemeOnLoad()
    {
        var isDark = ThemeService.Instance.IsDarkMode;
        Debug.WriteLine($"[MainWindow] ApplyThemeOnLoad - IsDarkMode: {isDark}");
        
        if (isDark)
        {
            ApplyThemeToAll();
        }
    }
    
    private void ApplyThemeToAll()
    {
        var isDark = ThemeService.Instance.IsDarkMode;
        
        var loginView = this.FindControl<LoginView>("Login");
        if (loginView != null && loginView is Views.ILoadable loadableLogin)
        {
            loadableLogin.ApplyThemeOnLoad();
        }
        
        var addAccountView = this.FindControl<AddAccountView>("AddAccount");
        if (addAccountView != null && addAccountView is Views.ILoadable loadableAddAccount)
        {
            loadableAddAccount.ApplyThemeOnLoad();
        }
        
        var mainDock = this.FindControl<DockPanel>("Main");
        if (mainDock != null)
        {
            var menuView = mainDock.FindControl<MenuView>("Menu");
            if (menuView != null && menuView is Views.ILoadable loadableMenu)
            {
                loadableMenu.ApplyThemeOnLoad();
            }
            
            var taskbar = mainDock.FindControl<Taskbar>("Taskbar");
            if (taskbar != null && taskbar is Views.ILoadable loadableTaskbar)
            {
                loadableTaskbar.ApplyThemeOnLoad();
            }
        }
        
        var settingsView = this.FindControl<SettingsView>("Settings");
        if (settingsView != null && settingsView is Views.ILoadable loadableSettings)
        {
            loadableSettings.ApplyThemeOnLoad();
        }
        
        var testView = this.FindControl<Inf02View>("Test");
        if (testView != null && testView is Views.ILoadable loadableTest)
        {
            loadableTest.ApplyThemeOnLoad();
        }
        
        var inf03View = this.FindControl<Inf03View>("Inf03");
        if (inf03View != null && inf03View is Views.ILoadable loadableInf03)
        {
            loadableInf03.ApplyThemeOnLoad();
        }
        
        var inf04View = this.FindControl<Inf04View>("Inf04");
        if (inf04View != null && inf04View is Views.ILoadable loadableInf04)
        {
            loadableInf04.ApplyThemeOnLoad();
        }
        
        var endScreenView = this.FindControl<EndScreenView>("EndScreen");
        if (endScreenView != null && endScreenView is Views.ILoadable loadableEndScreen)
        {
            loadableEndScreen.ApplyThemeOnLoad();
        }
        
        var rankingView = this.FindControl<RankingView>("Ranking");
        if (rankingView != null && rankingView is Views.ILoadable loadableRanking)
        {
            loadableRanking.ApplyThemeOnLoad();
        }
    }
    
    public void DoLogout()
    {
        var mainDock = this.FindControl<DockPanel>("Main");
        var loginView = this.FindControl<LoginView>("Login");
        var settingsView = this.FindControl<SettingsView>("Settings");
        var addAccountView = this.FindControl<AddAccountView>("AddAccount");

        if (mainDock != null && loginView != null)
        {
            Services.AuthService.Instance.ClearCredentials();
            
            mainDock.IsVisible = false;
            if (settingsView != null)
                settingsView.IsVisible = false;
            if (addAccountView != null)
                addAccountView.IsVisible = false;
            loginView.IsVisible = true;
            loginView.StopSpinner();
        }
    }
}
