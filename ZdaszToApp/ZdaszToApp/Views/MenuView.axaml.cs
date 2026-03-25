using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Services;
using System.Diagnostics;
using System;

namespace ZdaszToApp.Views;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        InitializeComponent();
        QuizCounter.Reset();
        Debug.WriteLine("[MenuView] Zaladowano MenuView");
    }

    private Control? FindParentWithName(Control start, string name)
    {
        var current = start.Parent;
        while (current != null)
        {
            if (current is Control c && c.Name == name)
                return c;
            current = current.Parent;
        }
        return null;
    }
    
    private Control? FindInRoot(Control start, string name)
    {
        var root = this.GetVisualRoot() as Control;
        if (root == null) return null;
        
        if (root is Window window)
        {
            return window.FindControl<Control>(name);
        }
        
        return FindControlRecursive(root, name);
    }
    
    private Control? FindControlRecursive(Control parent, string name)
    {
        if (parent.Name == name)
            return parent;
            
        if (parent is Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is Control c)
                {
                    var found = FindControlRecursive(c, name);
                    if (found != null) return found;
                }
            }
        }
        else if (parent is ContentControl cc && cc.Content is Control content)
        {
            var found = FindControlRecursive(content, name);
            if (found != null) return found;
        }
        
        return null;
    }

    private void OnInf02Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Debug.WriteLine("[MenuView] OnInf02Click - START");
        var mainDock = FindParentWithName(this, "Main") as DockPanel;
        var testView = FindInRoot(this, "Test") as Inf02View;
        
        Debug.WriteLine($"[MenuView] mainDock: {mainDock}, testView: {testView}");
        
        if (testView != null && mainDock != null)
        {
            testView.DataContext = new Inf02(1);
            testView.IsVisible = true;
            mainDock.IsVisible = false;
            Debug.WriteLine("[MenuView] Przelaczono na Inf02View");
        }
    }

    private void OnInf03Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Debug.WriteLine("[MenuView] OnInf03Click - START");
        var mainDock = FindParentWithName(this, "Main") as DockPanel;
        var inf03View = FindInRoot(this, "Inf03") as Inf03View;
        
        Debug.WriteLine($"[MenuView] mainDock: {mainDock}, inf03View: {inf03View}");
        
        if (inf03View != null && mainDock != null)
        {
            inf03View.DataContext = new Inf03(2);
            inf03View.IsVisible = true;
            mainDock.IsVisible = false;
            Debug.WriteLine("[MenuView] Przelaczono na Inf03View");
        }
    }

    private void OnInf04Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Debug.WriteLine("[MenuView] OnInf04Click - START");
        var mainDock = FindParentWithName(this, "Main") as DockPanel;
        var inf04View = FindInRoot(this, "Inf04") as Inf04View;
        
        Debug.WriteLine($"[MenuView] mainDock: {mainDock}, inf04View: {inf04View}");
        
        if (inf04View != null && mainDock != null)
        {
            inf04View.DataContext = new Inf04(3);
            inf04View.IsVisible = true;
            mainDock.IsVisible = false;
            Debug.WriteLine("[MenuView] Przelaczono na Inf04View");
        }
    }

    private void OnLogoutClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Debug.WriteLine("[MenuView] OnLogoutClick");
        
        var mainDock = FindParentWithName(this, "Main");
        var loginView = FindInRoot(this, "Login");
        
        if (mainDock != null && loginView != null)
        {
            mainDock.IsVisible = false;
            loginView.IsVisible = true;
            
            AuthService.Instance.ClearCredentials();
            
            if (DataContext is MainWindowViewModel vm)
            {
                vm.LoginViewModel.Reset();
            }
            
            Debug.WriteLine("[MenuView] Wylogowano");
        }
    }
}