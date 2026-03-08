using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using ZdaszToApp.Services;

namespace ZdaszToApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ApiService _apiService = ApiService.Instance;

    [ObservableProperty] private string? username;

    [ObservableProperty] private string? password;

    [ObservableProperty] private string? message;

    [ObservableProperty] private string? error_login;
    
    [ObservableProperty] private string? error_password;
    
    [ObservableProperty] private bool isLoggedIn;

    [RelayCommand]
    private async Task Login()
    {
        Message = null;
        Error_login = null;
        Error_password = null;

        if (string.IsNullOrWhiteSpace(Username))
        {
            Error_login = "Wpisz login";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            Error_password = "Wpisz hasło";
            return;
        }

        var result = await _apiService.LoginAsync(Username, Password);

        if (result == null || result.StartsWith("Error:"))
        {
            Message = result ?? "Błąd połączenia";
            return;
        }

        if (result == "Invalid password")
        {
            Error_password = "Błędne hasło";
            Message = "Błędne dane logowania";
            Password = string.Empty;
            return;
        }

        if (result == "No user found")
        {
            Error_login = "Użytkownik nie istnieje";
            Message = "Błędne dane logowania";
            return;
        }

        Message = "Logowanie udane!";
        IsLoggedIn = true;
    }
}
