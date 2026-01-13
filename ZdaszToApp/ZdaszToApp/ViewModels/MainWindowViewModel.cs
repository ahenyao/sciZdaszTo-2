using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZdaszToApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private bool isLoggedIn;
    [ObservableProperty] private bool isLoginVisible;
    [ObservableProperty] private bool isAddAccountVisible;
    [ObservableProperty] private bool isMainVisible;
    public LoginViewModel LoginViewModel { get; }
    public AddAccountViewModel AddAccountViewModel { get; }

    public MainWindowViewModel()
    {
        LoginViewModel = new LoginViewModel();
        AddAccountViewModel = new AddAccountViewModel();
        
        //LoginViewModel.AddAccountClicked += OnAddAccountClicked;
        //LoginViewModel.LoginSucceed += OnLoginSucceed;
        LoginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
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