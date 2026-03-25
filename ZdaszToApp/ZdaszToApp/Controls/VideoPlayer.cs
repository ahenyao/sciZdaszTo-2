using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace ZdaszToApp.Controls;

public class VideoPlayer : Control
{
    public static readonly StyledProperty<string?> SourceProperty =
        AvaloniaProperty.Register<VideoPlayer, string?>(nameof(Source));

    public static readonly StyledProperty<bool> IsPlayingProperty =
        AvaloniaProperty.Register<VideoPlayer, bool>(nameof(IsPlaying));

    public string? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public bool IsPlaying
    {
        get => GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, value);
    }

    static VideoPlayer()
    {
        SourceProperty.Changed.AddClassHandler<VideoPlayer>((s, e) => s.OnSourceChanged(e));
    }

    private void OnSourceChanged(AvaloniaPropertyChangedEventArgs e)
    {
        // Source changed - could trigger native update on Android
    }
}
