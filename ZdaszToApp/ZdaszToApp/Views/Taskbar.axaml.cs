using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace ZdaszToApp.Views;

public partial class Taskbar : UserControl
{
    public Taskbar()
    {
        InitializeComponent();
    }

    private void OnUserClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("[Taskbar] OnUserClick");
        UserPopup.IsOpen = !UserPopup.IsOpen;
    }

    private void OnLogoutClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("[Taskbar] OnLogoutClick - START");
        UserPopup.IsOpen = false;
        OnUserButtonClicked?.Invoke();
    }

    public event Action? OnUserButtonClicked;
}