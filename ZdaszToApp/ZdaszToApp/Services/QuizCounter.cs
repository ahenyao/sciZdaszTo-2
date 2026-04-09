using System.Collections.Generic;
using System.Linq;

namespace ZdaszToApp.Services;

public static class QuizCounter
{
    public static int CorrectAnswers { get; set; } = 0;
    public static int IncorrectAnswers { get; set; } = 0;
    public static List<(int QuestionId, bool IsCorrect)> Answers { get; } = new();

    public static void Reset()
    {
        CorrectAnswers = 0;
        IncorrectAnswers = 0;
        Answers.Clear();
    }

    public static void AddCorrect()
    {
        CorrectAnswers++;
    }

    public static void AddIncorrect()
    {
        IncorrectAnswers++;
    }

    public static void AddAnswer(int questionId, bool isCorrect)
    {
        Answers.Add((questionId, isCorrect));
    }

    public static string GetAnswersString()
    {
        return string.Join(";", Answers.Select(a => $"{a.QuestionId}:{(a.IsCorrect ? 1 : 0)}"));
    }
}
