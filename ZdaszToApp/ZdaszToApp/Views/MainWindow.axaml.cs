using Avalonia.Controls;
using ZdaszToApp.Views;
using ZdaszToApp.ViewModels;
namespace ZdaszToApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (Login.DataContext is LoginViewModel loginVm)
        {
            loginVm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LoginViewModel.IsLoggedIn) && loginVm.IsLoggedIn)
                {
                    Main.IsVisible = true;
                    Login.IsVisible = false;
                    AddAccount.IsVisible = false;
                }
            };
        }
    }
}
