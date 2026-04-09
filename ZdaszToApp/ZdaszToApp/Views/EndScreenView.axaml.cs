using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ZdaszToApp;
using ZdaszToApp.Services;

namespace ZdaszToApp.Views;

public partial class EndScreenView : UserControl, ILoadable
{
    public EndScreenView()
    {
        InitializeComponent();
        SetupTheme();
        DataContext = new EndScreenViewModel(QuizCounter.CorrectAnswers, QuizCounter.IncorrectAnswers);
    }

    private void SetupTheme()
    {
        ThemeService.Instance.PropertyChanged += OnThemeChanged;
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
        var mainGrid = this.FindControl<Grid>("MainGrid");
        var outerBorder = this.FindControl<Border>("OuterBorder");
        var innerBorder = this.FindControl<Border>("InnerBorder");
        
        if (mainGrid != null)
        {
            mainGrid.Classes.Clear();
            mainGrid.Classes.Add(isDark ? "dark" : "light");
            
            var bg = Application.Current?.FindResource(isDark ? "DarkPageBackground" : "PageBackground") as IBrush;
            if (bg != null)
                mainGrid.Background = bg;
        }

        if (outerBorder != null)
        {
            var overlay = Application.Current?.FindResource(isDark ? "OverlayDark" : "GlassOverlay") as IBrush;
            if (overlay != null)
                outerBorder.Background = overlay;
        }

        if (innerBorder != null)
        {
            var card = Application.Current?.FindResource(isDark ? "DarkGlossyCard" : "GlossyWhite") as IBrush;
            if (card != null)
                innerBorder.Background = card;
            else
                innerBorder.Background = isDark 
                    ? new SolidColorBrush(Avalonia.Media.Color.Parse("#1E1E3F"))
                    : new SolidColorBrush(Colors.White);
        }
    }

    public void ApplyThemeOnLoad()
    {
        ApplyTheme();
    }
}
