using Avalonia.Controls;
using Avalonia.VisualTree;
using ZdaszToApp.ViewModels;

namespace ZdaszToApp.Views;

public partial class TestView : UserControl
{
    public TestView()
    {
        InitializeComponent();
    }

    public TestView(int collectionId)
    {
        InitializeComponent();
        DataContext = new Test(collectionId);
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = this.GetVisualRoot() as Window;
        if (window != null)
        {
            var testView = window.FindControl<TestView>("Test");
            var mainDock = window.FindControl<DockPanel>("Main");
            if (testView != null && mainDock != null)
            {
                testView.IsVisible = false;
                mainDock.IsVisible = true;
            }
        }
    }
}
