using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace ZdaszToApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool isLoggedIn;

    public LoginViewModel LoginViewModel { get; }

    public MainViewModel()
    {
        LoginViewModel = new LoginViewModel();
        LoginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
    }

    private void LoginViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.IsLoggedIn) && LoginViewModel.IsLoggedIn)
        {
            IsLoggedIn = true;
        }
    }
}