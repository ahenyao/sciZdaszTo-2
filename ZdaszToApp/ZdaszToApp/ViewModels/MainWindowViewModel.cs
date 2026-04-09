using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ZdaszToApp.Services;

namespace ZdaszToApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private bool isLoggedIn;
    [ObservableProperty] private bool isLoginVisible;
    [ObservableProperty] private bool isAddAccountVisible;
    [ObservableProperty] private bool isMainVisible;
    public LoginViewModel LoginViewModel { get; }
    public AddAccountViewModel AddAccountViewModel { get; }
    public MenuViewModel MenuViewModel { get; }

    public MainWindowViewModel()
    {
        LoginViewModel = new LoginViewModel();
        AddAccountViewModel = new AddAccountViewModel();
        MenuViewModel = new MenuViewModel();
        
        LoginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        
        _ = AutoLogin();
    }

    public event Action? OnAutoLoginSuccess;
    public event Action? OnAutoLoginStart;
    public event Action? OnLogoutRequested;

    private async Task AutoLogin()
    {
        var authService = AuthService.Instance;
        if (authService.HasSavedCredentials())
        {
            LoginViewModel.IsLoading = true;
            IsLoginVisible = true;
            OnAutoLoginStart?.Invoke();
            var result = await ApiService.Instance.LoginAsync(
                authService.SavedUsername!,
                authService.SavedPassword!);

            if (result != null && !result.StartsWith("Error:") && result != "Invalid password" && result != "No user found")
            {
                IsLoggedIn = true;
                IsLoginVisible = false;
                IsMainVisible = true;
                OnAutoLoginSuccess?.Invoke();
                return;
            }
            else
            {
                LoginViewModel.IsLoading = false;
                authService.ClearCredentials();
            }
        }
        LoginViewModel.IsLoading = false;
        IsLoginVisible = true;
    }

    [RelayCommand]
    private void Logout()
    {
        AuthService.Instance.ClearCredentials();
        ApiService.Instance.ClearToken();
        LoginViewModel.Reset();
        
        IsLoggedIn = false;
        IsLoginVisible = true;
        IsMainVisible = false;
        IsAddAccountVisible = false;
        
        OnLogoutRequested?.Invoke();
    }
    
    private void OnAddAccountClicked()
    {
        IsLoginVisible = false;
        IsAddAccountVisible = true;
    }

    private void OnLoginSucceed()
    {
        IsAddAccountVisible = false;
        IsLoginVisible = false;
    }
    
    private void LoginViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.IsLoggedIn) && LoginViewModel.IsLoggedIn)
        {
            IsLoggedIn = true;
        }
    }
}
