using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using ZdaszToApp.Services;
using Avalonia.Media.Imaging;

namespace ZdaszToApp.ViewModels;

public partial class Test : ViewModelBase
{
    private readonly ApiService _apiService = ApiService.Instance;

    [ObservableProperty]
    private int _lives = 3;

    [ObservableProperty]
    private int _timeRemaining = 300;

    [ObservableProperty]
    private string _userAvatar = "avares://ZdaszToApp/Assets/user.ico";

    [ObservableProperty]
    private string? _questionImage = null;

    [ObservableProperty]
    private Bitmap? _questionImageBitmap = null;

    [ObservableProperty]
    private bool _hasImage = false;

    [ObservableProperty]
    private string _questionText = "Ładowanie pytania...";

    [ObservableProperty]
    private string _questionTypeStr = "ABCD";

    [ObservableProperty]
    private int _questionType = 1;

    [ObservableProperty]
    private string _userAnswer = "";

    [ObservableProperty]
    private string? _selectedOption = null;

    [ObservableProperty]
    private bool? _trueFalseAnswer = null;

    [ObservableProperty]
    private string _selectedAnswerKey = "";

    [ObservableProperty]
    private bool _isCorrect = false;

    [ObservableProperty]
    private bool _answerSubmitted = false;

    private Question? _currentQuestion;
    private int _collectionId = 1;

    public ObservableCollection<AnswerOption> Options { get; } = new();

    public bool ShowTextInput => QuestionType == 0;
    public bool ShowABCD => QuestionType == 1;
    public bool ShowTrueFalse => QuestionType == 2;

    public Test()
    {
        _ = LoadQuestionAsync();
    }

    public Test(int collectionId)
    {
        _collectionId = collectionId;
        _ = LoadQuestionAsync();
    }

    private async Task LoadQuestionAsync()
    {
        var question = await _apiService.GetRandomQuestionAsync(_collectionId);
        Console.WriteLine($"Question loaded: {question?.Text}");
        Console.WriteLine($"Answers: {question?.Answers?.Count}");
        
        if (question == null)
        {
            QuestionText = "Nie udało się pobrać pytania";
            return;
        }

        _currentQuestion = question;
        QuestionText = question.Text;
        QuestionTypeStr = question.Type;
        QuestionImage = question.ImagePath;

        if (!string.IsNullOrEmpty(question.ImagePath))
        {
            await LoadQuestionImageAsync(question.Id);
        }
        else
        {
            QuestionImageBitmap = null;
            HasImage = false;
        }

        Options.Clear();
        
        if (question.Type == "ABCD")
        {
            QuestionType = 1;
            var keys = new[] { "A", "B", "C", "D" };
            foreach (var key in keys)
            {
                if (question.Answers.TryGetValue(key, out var answer))
                {
                    Console.WriteLine($"Option {key}: {answer}");
                    Options.Add(new AnswerOption { Key = key, Text = answer });
                }
            }
        }
        else if (question.Type == "TRUE_FALSE")
        {
            QuestionType = 2;
        }
        else
        {
            QuestionType = 0;
        }

        OnPropertyChanged(nameof(Options));
        OnPropertyChanged(nameof(ShowTextInput));
        OnPropertyChanged(nameof(ShowABCD));
        OnPropertyChanged(nameof(ShowTrueFalse));
    }

    private async Task LoadQuestionImageAsync(int questionId)
    {
        try
        {
            var imageData = await _apiService.GetQuestionImageAsync(questionId);
            if (imageData != null && imageData.Length > 0)
            {
                using var stream = new MemoryStream(imageData);
                QuestionImageBitmap = new Bitmap(stream);
                HasImage = true;
                Console.WriteLine($"Image loaded: {imageData.Length} bytes");
            }
            else
            {
                QuestionImageBitmap = null;
                HasImage = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading image: {ex.Message}");
            QuestionImageBitmap = null;
            HasImage = false;
        }
    }

    [RelayCommand]
    private void SelectOption(string optionKey)
    {
        if (AnswerSubmitted) return;
        SelectedAnswerKey = optionKey;
        SelectedOption = optionKey;
    }

    [RelayCommand]
    private void SelectTrueFalse(bool value)
    {
        if (AnswerSubmitted) return;
        TrueFalseAnswer = value;
        SelectedAnswerKey = value ? "TRUE" : "FALSE";
    }

    [RelayCommand]
    private async Task SubmitAnswer()
    {
        if (_currentQuestion == null || AnswerSubmitted) return;

        string userAnswer;
        if (QuestionType == 0)
        {
            userAnswer = UserAnswer;
        }
        else if (QuestionType == 1)
        {
            if (string.IsNullOrEmpty(SelectedAnswerKey)) return;
            userAnswer = SelectedAnswerKey;
        }
        else
        {
            if (TrueFalseAnswer == null) return;
            userAnswer = TrueFalseAnswer.Value ? "TRUE" : "FALSE";
        }

        IsCorrect = userAnswer.ToUpper() == _currentQuestion.CorrectAnswer.ToUpper();
        AnswerSubmitted = true;

        if (!IsCorrect)
        {
            Lives--;
        }

        await Task.Delay(1500);
        await LoadNextQuestionAsync();
    }

    private async Task LoadNextQuestionAsync()
    {
        AnswerSubmitted = false;
        SelectedAnswerKey = "";
        SelectedOption = null;
        TrueFalseAnswer = null;
        UserAnswer = "";
        IsCorrect = false;
        await LoadQuestionAsync();
    }

    [RelayCommand]
    private void SelectABCD(string key)
    {
        if (AnswerSubmitted) return;
        SelectedAnswerKey = key;
        SelectedOption = key;
    }
}

public class AnswerOption
{
    public string Key { get; set; } = "";
    public string Text { get; set; } = "";
}
