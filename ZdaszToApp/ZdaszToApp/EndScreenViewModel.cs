using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ZdaszToApp.Views;
using ZdaszToApp.ViewModels;
using ZdaszToApp.Services;

namespace ZdaszToApp;

public partial class EndScreenViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _correctAnswers;

    [ObservableProperty]
    private int _incorrectAnswers;

    [ObservableProperty]
    private int _totalQuestions;

    [ObservableProperty]
    private double _percentage;

    [ObservableProperty]
    private string _resultMessage = "";

    private string _lastTestType = "";

    public EndScreenViewModel()
    {
        RefreshData();
    }

    public EndScreenViewModel(int correct, int incorrect)
    {
        CorrectAnswers = correct;
        IncorrectAnswers = incorrect;
        TotalQuestions = correct + incorrect;
        CalculateResult();
    }

    public void RefreshData()
    {
        CorrectAnswers = QuizCounter.CorrectAnswers;
        IncorrectAnswers = QuizCounter.IncorrectAnswers;
        TotalQuestions = CorrectAnswers + IncorrectAnswers;
        CalculateResult();
    }

    public void SetLastTestType(string testType)
    {
        _lastTestType = testType;
    }

    private void CalculateResult()
    {
        if (TotalQuestions > 0)
        {
            Percentage = (double)CorrectAnswers / TotalQuestions * 100;
        }
        else
        {
            Percentage = 0;
        }

        ResultMessage = Percentage switch
        {
            >= 90 => "Fantastycznie! Jesteś ekspertem!",
            >= 70 => "Świetnie! Dużo zaliczyłeś!",
            >= 50 => "Nieźle! Jeszcze trochę i będzie perfekcyjnie!",
            >= 30 => "Słabo... Musisz się uczyć!",
            _ => "Beznadziejnie! Koniecznie powtórz materiał!"
        };
    }

    private Control? FindControlRecursive(Control parent, string targetName)
    {
        if (parent.Name == targetName)
            return parent;
            
        if (parent is Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is Control c)
                {
                    var found = FindControlRecursive(c, targetName);
                    if (found != null) return found;
                }
            }
        }
        else if (parent is ContentControl cc && cc.Content is Control content)
        {
            var found = FindControlRecursive(content, targetName);
            if (found != null) return found;
        }
        
        return null;
    }

    [RelayCommand]
    private void GoToMenu()
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = desktop.MainWindow;
            if (window != null)
            {
                var endScreen = window.FindControl<UserControl>("EndScreen");
                var mainDock = window.FindControl<DockPanel>("Main");
                if (endScreen != null && mainDock != null)
                {
                    QuizCounter.Reset();
                    endScreen.IsVisible = false;
                    mainDock.IsVisible = true;
                }
            }
        }
        else if (App.Current?.ApplicationLifetime is ISingleViewApplicationLifetime mobile)
        {
            var root = mobile.MainView as Control;
            if (root != null)
            {
                var endScreen = FindControlRecursive(root, "EndScreen");
                var mainDock = FindControlRecursive(root, "Main") as DockPanel;
                if (endScreen != null && mainDock != null)
                {
                    QuizCounter.Reset();
                    endScreen.IsVisible = false;
                    mainDock.IsVisible = true;
                }
            }
        }
    }

    [RelayCommand]
    private void Retry()
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = desktop.MainWindow;
            if (window != null)
            {
                var endScreen = window.FindControl<UserControl>("EndScreen");
                if (endScreen != null && endScreen.DataContext is EndScreenViewModel endVm)
                {
                    QuizCounter.Reset();
                    endScreen.IsVisible = false;

                    switch (_lastTestType)
                    {
                        case "Inf02":
                            var testView = window.FindControl<Inf02View>("Test");
                            if (testView != null)
                            {
                                testView.DataContext = new Inf02(1);
                                testView.IsVisible = true;
                            }
                            break;
                        case "Inf03":
                            var inf03View = window.FindControl<Inf03View>("Inf03");
                            if (inf03View != null)
                            {
                                inf03View.DataContext = new Inf03(2);
                                inf03View.IsVisible = true;
                            }
                            break;
                        case "Inf04":
                            var inf04View = window.FindControl<Inf04View>("Inf04");
                            if (inf04View != null)
                            {
                                inf04View.DataContext = new Inf04(3);
                                inf04View.IsVisible = true;
                            }
                            break;
                    }
                }
            }
        }
        else if (App.Current?.ApplicationLifetime is ISingleViewApplicationLifetime mobile)
        {
            var root = mobile.MainView as Control;
            if (root != null)
            {
                var endScreen = FindControlRecursive(root, "EndScreen");
                if (endScreen != null)
                {
                    QuizCounter.Reset();
                    endScreen.IsVisible = false;

                    switch (_lastTestType)
                    {
                        case "Inf02":
                            var testView = FindControlRecursive(root, "Test");
                            if (testView != null)
                            {
                                testView.DataContext = new Inf02(1);
                                testView.IsVisible = true;
                            }
                            break;
                        case "Inf03":
                            var inf03View = FindControlRecursive(root, "Inf03");
                            if (inf03View != null)
                            {
                                inf03View.DataContext = new Inf03(2);
                                inf03View.IsVisible = true;
                            }
                            break;
                        case "Inf04":
                            var inf04View = FindControlRecursive(root, "Inf04");
                            if (inf04View != null)
                            {
                                inf04View.DataContext = new Inf04(3);
                                inf04View.IsVisible = true;
                            }
                            break;
                    }
                }
            }
        }
    }
}
