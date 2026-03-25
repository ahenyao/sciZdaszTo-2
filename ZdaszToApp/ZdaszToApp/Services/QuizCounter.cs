namespace ZdaszToApp.Services;

public static class QuizCounter
{
    public static int CorrectAnswers { get; set; } = 0;
    public static int IncorrectAnswers { get; set; } = 0;

    public static void Reset()
    {
        CorrectAnswers = 0;
        IncorrectAnswers = 0;
    }

    public static void AddCorrect()
    {
        CorrectAnswers++;
    }

    public static void AddIncorrect()
    {
        IncorrectAnswers++;
    }
}
