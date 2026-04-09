using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using ZdaszToApp.Services;

namespace ZdaszToApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ApiService _apiService = ApiService.Instance;
    private readonly AuthService _authService = AuthService.Instance;

    [ObservableProperty] private string? username;

    [ObservableProperty] private string? password;

    [ObservableProperty] private string? message;

    [ObservableProperty] private string? error_login;
    
    [ObservableProperty] private string? error_password;
    
    [ObservableProperty] private bool isLoggedIn;

    [ObservableProperty] private bool isLoading;

    public LoginViewModel()
    {
        Reset();
    }

    public void Reset()
    {
        Username = null;
        Password = null;
        Message = null;
        Error_login = null;
        Error_password = null;
        IsLoggedIn = false;
        IsLoading = false;
        
        if (_authService.HasSavedCredentials())
        {
            Username = _authService.SavedUsername;
            Password = _authService.SavedPassword;
        }
    }

    [RelayCommand]
    private async Task Login()
    {
        Console.WriteLine("[LoginViewModel] Login command started");
        Message = null;
        Error_login = null;
        Error_password = null;
        IsLoading = true;
        Console.WriteLine("[LoginViewModel] IsLoading set to true");

        if (string.IsNullOrWhiteSpace(Username))
        {
            Error_login = "Wpisz login";
            IsLoading = false;
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            Error_password = "Wpisz hasło";
            IsLoading = false;
            return;
        }

        var result = await _apiService.LoginAsync(Username, Password);

        if (result == null || result.StartsWith("Error:"))
        {
            Message = "Brak połączenia z internetem";
            IsLoading = false;
            return;
        }

        if (result == "Invalid password")
        {
            Error_password = "Błędne hasło";
            Message = "Błędne dane logowania";
            Password = string.Empty;
            IsLoading = false;
            return;
        }

        if (result == "No user found")
        {
            Error_login = "Użytkownik nie istnieje";
            Message = "Błędne dane logowania";
            IsLoading = false;
            return;
        }

        _authService.SaveCredentials(Username!, Password!);
        Message = ""; //"Logowanie udane!";
        IsLoggedIn = true;
        IsLoading = false;
        Console.WriteLine("[LoginViewModel] Login success, calling OnLoginSuccess");
        OnLoginSuccess?.Invoke();
    }

    [RelayCommand]
    private void CreateAccount()
    {
        OnCreateAccountClicked?.Invoke();
    }

    public event Action? OnCreateAccountClicked;
    public event Action? OnLoginSuccess;
}
