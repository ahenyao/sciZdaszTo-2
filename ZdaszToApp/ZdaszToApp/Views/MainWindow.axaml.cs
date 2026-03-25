using Avalonia.Controls;
using ZdaszToApp.ViewModels;
using System;
namespace ZdaszToApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new MainWindowViewModel();
        
        if (DataContext is MainWindowViewModel vm)
        {
            Login.DataContext = vm.LoginViewModel;
            AddAccount.DataContext = vm.AddAccountViewModel;
            
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
                Main.IsVisible = true;
                Login.IsVisible = false;
                AddAccount.IsVisible = false;
            };
        }
    }
}
