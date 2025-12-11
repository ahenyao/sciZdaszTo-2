using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using Tmds.DBus.Protocol;

namespace ZdaszToApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string username;

    [ObservableProperty] private string password;

    [ObservableProperty] private string message;

    private static List<(string Username, string Password)> Users = new()
    {
        ("test", "1234"),
        ("admin", "admin")
    };

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            Message = "Nazwa użytkownika i hasło nie mogą być puste.";
            return;
        }

        var user = Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

        if (user == default)
        {
            Message = "Nieprawidłowa nazwa użytkownika lub hasło.";
        }
        else
        {
            Message = "Logowanie udane!";
        }
    }
}
