using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Media;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Services;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace ZdaszToApp.Views;

public partial class RankingView : UserControl, ILoadable
{
    private LinearGradientBrush? _lightBg;
    private LinearGradientBrush? _darkBg;
    private SolidColorBrush? _lightCardBg;
    private SolidColorBrush? _darkCardBg;
    private SolidColorBrush? _lightText;
    private SolidColorBrush? _darkText;
    private SolidColorBrush? _lightSecondaryText;
    private SolidColorBrush? _darkSecondaryText;
    private SolidColorBrush? _lightBorderBg;
    private SolidColorBrush? _darkBorderBg;

    public RankingView()
    {
        InitializeComponent();
        DataContext = new RankingViewModel();

        InitializeBrushes();
        ThemeService.Instance.PropertyChanged += OnThemeChanged;

        Debug.WriteLine("[RankingView] Zaladowano RankingView");
    }

    public async Task RefreshRankingAsync()
    {
        if (DataContext is RankingViewModel vm)
        {
            await vm.LoadRankingAsync();
        }
    }

    private void InitializeBrushes()
    {
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

        _lightSecondaryText = new SolidColorBrush(Color.Parse("#558B2F"));
        _darkSecondaryText = new SolidColorBrush(Color.Parse("#81C784"));

        _lightBorderBg = new SolidColorBrush(Color.Parse("#20FFFFFF"));
        _darkBorderBg = new SolidColorBrush(Color.Parse("#40000000"));
    }

    public void ApplyThemeOnLoad()
    {
        ApplyTheme();
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
        Debug.WriteLine($"[RankingView] ApplyTheme - isDark: {isDark}");

        var mainGrid = this.FindControl<Grid>("MainGrid");
        var cardBorder = this.FindControl<Border>("CardBorder");
        var backText = this.FindControl<TextBlock>("BackText");
        var titleText = this.FindControl<TextBlock>("TitleText");
        var titleBorder = this.FindControl<Border>("TitleBorder");

        if (isDark)
        {
            if (mainGrid != null)
                mainGrid.Background = _darkBg;

            if (cardBorder != null)
                cardBorder.Background = _darkBorderBg;

            if (backText != null)
                backText.Foreground = _darkText;

            if (titleText != null)
                titleText.Foreground = new SolidColorBrush(Colors.White);

            if (titleBorder != null)
                titleBorder.Background = new SolidColorBrush(Color.Parse("#4CAF50"));
        }
        else
        {
            if (mainGrid != null)
                mainGrid.Background = _lightBg;

            if (cardBorder != null)
                cardBorder.Background = _lightBorderBg;

            if (backText != null)
                backText.Foreground = _lightText;

            if (titleText != null)
                titleText.Foreground = new SolidColorBrush(Colors.White);

            if (titleBorder != null)
                titleBorder.Background = new SolidColorBrush(Color.Parse("#8BC34A"));
        }
    }

    public event Action? OnBackToMenu;
    
    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Debug.WriteLine("[RankingView] OnBackClick - START");
        OnBackToMenu?.Invoke();
    }
}