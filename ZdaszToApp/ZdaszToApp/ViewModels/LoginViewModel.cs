using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using Tmds.DBus.Protocol;
using System;

namespace ZdaszToApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string? username;

    [ObservableProperty] private string? password;

    [ObservableProperty] private string? message;

    [ObservableProperty] private string? error_login = "Wpisz login";       // OPCJONALNE
    
    [ObservableProperty] private string? error_password = "Wpisz hasło";    // OPCJONALNE
    
    [ObservableProperty] private bool isLoggedIn;

    //DO ZMIANY - DANE DO LOGOWANIA POWINNY BYĆ POBIERANE Z API 
    private static List<(string Username, string Password)> Users = new()
    {
        ("test", "1234"),
        ("admin", "admin")
    };

    [RelayCommand]
    private void Login()
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

        var user = Users.FirstOrDefault(u => u.Username == Username);

        if (user == default)
        {
            Error_login = "Wpisany login jest błędny";
            Message = "Błędny login";
            return;
        }

        if (user.Password != Password)
        {
            Error_password = "Wpisane hasło jest błędne";
            Message = "Błędne dane logowania";
            Password = string.Empty;
            return;
        }

        Message = "Logowanie udane!";
        IsLoggedIn = true;
    }
}
