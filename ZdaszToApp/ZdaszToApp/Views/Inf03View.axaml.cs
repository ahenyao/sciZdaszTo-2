using Avalonia.Controls;
using Avalonia.VisualTree;
using ZdaszToApp.ViewModels;

namespace ZdaszToApp.Views;

public partial class Inf03View : UserControl
{
    public Inf03View()
    {
        InitializeComponent();
        DataContext = new Inf03();
    }

    public Inf03View(int collectionId)
    {
        InitializeComponent();
        DataContext = new Inf03(collectionId);
    }

    private Control? FindInRoot(Control start, string name)
    {
        var root = this.GetVisualRoot() as Control;
        if (root == null) return null;
        
        if (root is Window window)
            return window.FindControl<Control>(name);
        
        return FindControlRecursive(root, name);
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

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var testView = FindInRoot(this, "Inf03");
        var mainDock = FindInRoot(this, "Main") as DockPanel;
        
        if (testView != null && mainDock != null)
        {
            testView.IsVisible = false;
            mainDock.IsVisible = true;
        }
    }
}
