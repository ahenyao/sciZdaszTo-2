using Avalonia.Controls;
using ZdaszToApp;
using ZdaszToApp.Services;

namespace ZdaszToApp.Views;

public partial class EndScreenView : UserControl
{
    public EndScreenView()
    {
        InitializeComponent();
        DataContext = new EndScreenViewModel(QuizCounter.CorrectAnswers, QuizCounter.IncorrectAnswers);
    }
}
