using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Services;
using System.Diagnostics;
using System;

namespace ZdaszToApp.Views;

public partial class SettingsView : UserControl, ILoadable
{
    public event Action? OnLogoutRequested;
    
    private LinearGradientBrush? _lightBg;
    private LinearGradientBrush? _darkBg;
    private SolidColorBrush? _lightCardBg;
    private SolidColorBrush? _darkCardBg;
    private SolidColorBrush? _lightText;
    private SolidColorBrush? _darkText;
    private SolidColorBrush? _lightTextSecondary;
    private SolidColorBrush? _darkTextSecondary;

    public SettingsView()
    {
        InitializeComponent();
        
        _lightBg = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
            GradientStops = new GradientStops
            {
                new GradientStop(Color.Parse("#E8F5E9"), 0),
                new GradientStop(Color.Parse("#C8E6C9"), 0.5),
                new GradientStop(Color.Parse("#A5D6A7"), 1)
            }
        };
        
        _darkBg = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
            GradientStops = new GradientStops
            {
                new GradientStop(Color.Parse("#0D1B2A"), 0),
                new GradientStop(Color.Parse("#1B263B"), 0.5),
                new GradientStop(Color.Parse("#1B263B"), 1)
            }
        };
        
        _lightCardBg = new SolidColorBrush(Color.Parse("#FFFFFF"));
        _darkCardBg = new SolidColorBrush(Color.Parse("#1E1E3F"));
        _lightText = new SolidColorBrush(Color.Parse("#2E7D32"));
        _darkText = new SolidColorBrush(Color.Parse("#4CAF50"));
        _lightTextSecondary = new SolidColorBrush(Color.Parse("#558B2F"));
        _darkTextSecondary = new SolidColorBrush(Color.Parse("#81C784"));

        ThemeService.Instance.PropertyChanged += OnThemeChanged;
        
        var darkModeToggle = this.FindControl<ToggleSwitch>("DarkModeToggle");
        if (darkModeToggle != null)
            darkModeToggle.IsChecked = ThemeService.Instance.IsDarkMode;
    }

    public void ApplyThemeOnLoad()
    {
        ApplyTheme();
        
        var mainGrid = this.FindControl<Grid>("MainGrid");
        var cardBorder = this.FindControl<Border>("CardBorder");
        var darkModeToggle = this.FindControl<ToggleSwitch>("DarkModeToggle");
        var isDark = ThemeService.Instance.IsDarkMode;
        
        if (darkModeToggle != null)
            darkModeToggle.IsChecked = isDark;
        
        if (mainGrid != null)
        {
            if (isDark)
                mainGrid.Background = _darkBg;
            else
                mainGrid.Background = _lightBg;
        }
        
        if (cardBorder != null)
        {
            if (isDark)
                cardBorder.Background = new SolidColorBrush(Color.Parse("#40000000"));
            else
                cardBorder.Background = new SolidColorBrush(Color.Parse("#20FFFFFF"));
        }
    }

    private void OnThemeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ThemeService.IsDarkMode))
        {
            ApplyTheme();
        }
    }

    private void ApplyTheme()
    {
        var isDark = ThemeService.Instance.IsDarkMode;
        
        var mainGrid = this.FindControl<Grid>("MainGrid");
        var cardBorder = this.FindControl<Border>("CardBorder");
        
        var backText = this.FindControl<TextBlock>("BackText");
        var settingsTitle = this.FindControl<TextBlock>("SettingsTitle");
        
        var soundBorder = this.FindControl<Border>("SoundBorder");
        var soundTitle = this.FindControl<TextBlock>("SoundTitle");
        var volumeLabel = this.FindControl<TextBlock>("VolumeLabel");
        
        var appearanceBorder = this.FindControl<Border>("AppearanceBorder");
        var appearanceTitle = this.FindControl<TextBlock>("AppearanceTitle");
        var darkModeLabel = this.FindControl<TextBlock>("DarkModeLabel");
        
        var notificationsBorder = this.FindControl<Border>("NotificationsBorder");
        var notificationsTitle = this.FindControl<TextBlock>("NotificationsTitle");
        var pushLabel = this.FindControl<TextBlock>("PushLabel");
        
        var accountBorder = this.FindControl<Border>("AccountBorder");
        var accountTitle = this.FindControl<TextBlock>("AccountTitle");
        
        var versionBorder = this.FindControl<Border>("VersionBorder");
        var versionText = this.FindControl<TextBlock>("VersionText");
        
        if (isDark)
        {
            if (mainGrid != null)
                mainGrid.Background = _darkBg;
            
            if (cardBorder != null)
                cardBorder.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#40000000"));
            
            if (backText != null)
                backText.Foreground = _darkText;
            if (settingsTitle != null)
                settingsTitle.Foreground = _darkText;
            
            if (soundBorder != null)
                soundBorder.Background = _darkCardBg;
            if (soundTitle != null)
                soundTitle.Foreground = _darkText;
            if (volumeLabel != null)
                volumeLabel.Foreground = _darkTextSecondary;
            
            if (appearanceBorder != null)
                appearanceBorder.Background = _darkCardBg;
            if (appearanceTitle != null)
                appearanceTitle.Foreground = _darkText;
            if (darkModeLabel != null)
                darkModeLabel.Foreground = _darkTextSecondary;
            
            if (notificationsBorder != null)
                notificationsBorder.Background = _darkCardBg;
            if (notificationsTitle != null)
                notificationsTitle.Foreground = _darkText;
            if (pushLabel != null)
                pushLabel.Foreground = _darkTextSecondary;
            
            if (accountBorder != null)
                accountBorder.Background = _darkCardBg;
            if (accountTitle != null)
                accountTitle.Foreground = _darkText;
            
            if (versionBorder != null)
                versionBorder.Background = _darkCardBg;
            if (versionText != null)
                versionText.Foreground = _darkTextSecondary;
        }
        else
        {
            if (mainGrid != null)
                mainGrid.Background = _lightBg;
            
            if (cardBorder != null)
                cardBorder.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#20FFFFFF"));
            
            if (backText != null)
                backText.Foreground = _lightText;
            if (settingsTitle != null)
                settingsTitle.Foreground = _lightText;
            
            if (soundBorder != null)
                soundBorder.Background = _lightCardBg;
            if (soundTitle != null)
                soundTitle.Foreground = _lightText;
            if (volumeLabel != null)
                volumeLabel.Foreground = _lightTextSecondary;
            
            if (appearanceBorder != null)
                appearanceBorder.Background = _lightCardBg;
            if (appearanceTitle != null)
                appearanceTitle.Foreground = _lightText;
            if (darkModeLabel != null)
                darkModeLabel.Foreground = _lightTextSecondary;
            
            if (notificationsBorder != null)
                notificationsBorder.Background = _lightCardBg;
            if (notificationsTitle != null)
                notificationsTitle.Foreground = _lightText;
            if (pushLabel != null)
                pushLabel.Foreground = _lightTextSecondary;
            
            if (accountBorder != null)
                accountBorder.Background = _lightCardBg;
            if (accountTitle != null)
                accountTitle.Foreground = _lightText;
            
            if (versionBorder != null)
                versionBorder.Background = new SolidColorBrush(Color.Parse("#A5D6A7"));
            if (versionText != null)
                versionText.Foreground = _lightTextSecondary;
        }
    }

    private void OnDarkModeToggle(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var darkModeToggle = this.FindControl<ToggleSwitch>("DarkModeToggle");
        var isDark = darkModeToggle?.IsChecked == true;
        ThemeService.Instance.IsDarkMode = isDark;

        ApplyTheme();
        
        var root = this.GetVisualRoot() as Window;
        if (root != null)
        {
            NotifyAllViews(root, isDark);
        }
    }

    private void NotifyAllViews(Window root, bool isDark)
    {
    }

    private Control? FindInRoot(Control start, string name)
    {
        var root = this.GetVisualRoot() as Control;
        if (root == null) return null;

        if (root is Window window)
        {
            return window.FindControl<Control>(name);
        }

        if (name == null)
        {
            return root;
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
        else if (parent is ContentControl cc)
        {
            if (cc.Content is Control content)
            {
                var found = FindControlRecursive(content, name);
                if (found != null) return found;
            }
        }
        else if (parent is Grid grid)
        {
            foreach (var child in grid.Children)
            {
                if (child is Control c)
                {
                    var found = FindControlRecursive(c, name);
                    if (found != null) return found;
                }
            }
        }
        else if (parent is Decorator decorator)
        {
            if (decorator.Child is Control child)
            {
                var found = FindControlRecursive(child, name);
                if (found != null) return found;
            }
        }

        return null;
    }

    private UserControl? FindParentUserControl()
    {
        var current = this.Parent;
        while (current != null)
        {
            if (current is UserControl uc)
            {
                return uc;
            }
            current = current.Parent;
        }
        return null;
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainDock = FindInRoot(this, "Main") as DockPanel;
        var settingsView = FindInRoot(this, "Settings") as SettingsView;

        if (mainDock != null && settingsView != null)
        {
            mainDock.IsVisible = true;
            settingsView.IsVisible = false;
        }
    }

    private void OnLogoutClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("[SettingsView] OnLogoutClick - START");
        var root = this.GetVisualRoot();
        System.Diagnostics.Debug.WriteLine($"[SettingsView] GetVisualRoot: {root?.GetType().Name}");
        
        if (root is Window window)
        {
            System.Diagnostics.Debug.WriteLine("[SettingsView] Desktop - calling OnLogoutRequested");
            OnLogoutRequested?.Invoke();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("[SettingsView] Mobile path");
            var parentUc = FindParentUserControl();
            System.Diagnostics.Debug.WriteLine($"[SettingsView] parentUc: {parentUc?.GetType().Name}");
            
            if (parentUc is AppView appView)
            {
                System.Diagnostics.Debug.WriteLine("[SettingsView] Found AppView");
                var mainDock = appView.FindControl<DockPanel>("Main");
                var loginView = appView.FindControl<LoginView>("Login");
                var settingsView = appView.FindControl<SettingsView>("Settings");
                System.Diagnostics.Debug.WriteLine($"[SettingsView] mainDock: {mainDock?.GetType().Name}, loginView: {loginView?.GetType().Name}");
            
                if (mainDock != null && loginView != null)
                {
                    Services.AuthService.Instance.ClearCredentials();
                    mainDock.IsVisible = false;
                    if (settingsView != null)
                        settingsView.IsVisible = false;
                    loginView.IsVisible = true;
                    System.Diagnostics.Debug.WriteLine("[SettingsView] Wylogowano (mobile)");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[SettingsView] Blad: mainDock lub loginView jest null");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[SettingsView] parentUc nie jest AppView");
            }
        }
    }
}