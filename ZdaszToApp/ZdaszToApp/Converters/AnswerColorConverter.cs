using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ZdaszToApp.Converters;

public class AnswerColorConverter : IMultiValueConverter
{
    public static readonly AnswerColorConverter Instance = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 3) return new SolidColorBrush(Color.Parse("#363636"));

        bool isSelected = values[0] is bool b0 && b0;
        bool isCorrectAnswer = values[1] is bool b1 && b1;
        bool answerSubmitted = values[2] is bool b2 && b2;

        if (parameter?.ToString() == "invert")
        {
            isCorrectAnswer = !isCorrectAnswer;
        }

        if (answerSubmitted)
        {
            if (isCorrectAnswer)
                return new SolidColorBrush(Color.Parse("#4CAF50")); // green
            if (isSelected && !isCorrectAnswer)
                return new SolidColorBrush(Color.Parse("#F44336")); // red
        }
        else if (isSelected)
        {
            return new SolidColorBrush(Color.Parse("#555555")); // selected gray
        }

        return new SolidColorBrush(Color.Parse("#363636")); // default
    }
}
