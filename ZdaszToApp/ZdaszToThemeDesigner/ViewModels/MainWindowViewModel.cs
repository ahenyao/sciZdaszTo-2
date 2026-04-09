using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ZdaszToThemeDesigner.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _selectedThemeType = "Light";
    [ObservableProperty] private bool _isDarkMode;

    [ObservableProperty] private string _primaryGreen = "#8BC34A";
    [ObservableProperty] private string _primaryBlue = "#03A9F4";
    [ObservableProperty] private string _primaryCyan = "#00BCD4";
    [ObservableProperty] private string _bgTop = "#E8F5E9";
    [ObservableProperty] private string _bgMiddle = "#C8E6C9";
    [ObservableProperty] private string _bgBottom = "#A5D6A7";
    [ObservableProperty] private string _cardBackground = "#FFFFFF";
    [ObservableProperty] private string _textPrimary = "#212121";
    [ObservableProperty] private string _textSecondary = "#757575";

    public ObservableCollection<ColorItem> LightColors { get; } = new();
    public ObservableCollection<ColorItem> DarkColors { get; } = new();

    public MainWindowViewModel()
    {
        LoadDefaultColors();
    }

    private void LoadDefaultColors()
    {
        LightColors.Add(new ColorItem { Name = "Primary Green", Hex = "#8BC34A" });
        LightColors.Add(new ColorItem { Name = "Primary Blue", Hex = "#03A9F4" });
        LightColors.Add(new ColorItem { Name = "Primary Cyan", Hex = "#00BCD4" });
        LightColors.Add(new ColorItem { Name = "Background Top", Hex = "#E8F5E9" });
        LightColors.Add(new ColorItem { Name = "Background Middle", Hex = "#C8E6C9" });
        LightColors.Add(new ColorItem { Name = "Background Bottom", Hex = "#A5D6A7" });
        LightColors.Add(new ColorItem { Name = "Card Background", Hex = "#FFFFFF" });
        LightColors.Add(new ColorItem { Name = "Text Primary", Hex = "#212121" });
        LightColors.Add(new ColorItem { Name = "Text Secondary", Hex = "#757575" });

        DarkColors.Add(new ColorItem { Name = "Primary Green", Hex = "#4CAF50" });
        DarkColors.Add(new ColorItem { Name = "Primary Blue", Hex = "#00BCD4" });
        DarkColors.Add(new ColorItem { Name = "Primary Cyan", Hex = "#26C6DA" });
        DarkColors.Add(new ColorItem { Name = "Background Top", Hex = "#0D1B2A" });
        DarkColors.Add(new ColorItem { Name = "Background Middle", Hex = "#1B263B" });
        DarkColors.Add(new ColorItem { Name = "Background Bottom", Hex = "#1B263B" });
        DarkColors.Add(new ColorItem { Name = "Card Background", Hex = "#1E1E3F" });
        DarkColors.Add(new ColorItem { Name = "Text Primary", Hex = "#E8F5E9" });
        DarkColors.Add(new ColorItem { Name = "Text Secondary", Hex = "#81C784" });
    }

    [RelayCommand]
    private void SetLightTheme()
    {
        SelectedThemeType = "Light";
        IsDarkMode = false;
        LoadLightColors();
    }

    [RelayCommand]
    private void SetDarkTheme()
    {
        SelectedThemeType = "Dark";
        IsDarkMode = true;
        LoadDarkColors();
    }

    private void LoadLightColors()
    {
        PrimaryGreen = "#8BC34A";
        PrimaryBlue = "#03A9F4";
        PrimaryCyan = "#00BCD4";
        BgTop = "#E8F5E9";
        BgMiddle = "#C8E6C9";
        BgBottom = "#A5D6A7";
        CardBackground = "#FFFFFF";
        TextPrimary = "#212121";
        TextSecondary = "#757575";
    }

    private void LoadDarkColors()
    {
        PrimaryGreen = "#4CAF50";
        PrimaryBlue = "#00BCD4";
        PrimaryCyan = "#26C6DA";
        BgTop = "#0D1B2A";
        BgMiddle = "#1B263B";
        BgBottom = "#1B263B";
        CardBackground = "#1E1E3F";
        TextPrimary = "#E8F5E9";
        TextSecondary = "#81C784";
    }

    [RelayCommand]
    private void ExportAxaml()
    {
        var content = GenerateAxaml();
        System.IO.File.WriteAllText("generated_theme.axaml", content);
    }

    [RelayCommand]
    private void ExportCSharp()
    {
        var content = GenerateCSharp();
        System.IO.File.WriteAllText("ThemeColors.cs", content);
    }

    private string GenerateAxaml()
    {
        var colors = IsDarkMode ? DarkColors : LightColors;
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("<ResourceDictionary xmlns=\"https://github.com/avaloniaui\"");
        sb.AppendLine("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
        sb.AppendLine();
        
        foreach (var c in colors)
        {
            var key = c.Name.Replace(" ", "");
            sb.AppendLine($"    <Color x:Key=\"{key}\">{c.Hex}</Color>");
            sb.AppendLine($"    <SolidColorBrush x:Key=\"{key}Brush\" Color=\"{c.Hex}\"/>");
        }
        
        sb.AppendLine("</ResourceDictionary>");
        return sb.ToString();
    }

    private string GenerateCSharp()
    {
        var colors = IsDarkMode ? DarkColors : LightColors;
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("public static class ThemeColors");
        sb.AppendLine("{");
        
        foreach (var c in colors)
        {
            var key = c.Name.Replace(" ", "");
            sb.AppendLine($"    public static string {key} = \"{c.Hex}\";");
        }
        
        sb.AppendLine("}");
        return sb.ToString();
    }
}

public partial class ColorItem : ObservableObject
{
    [ObservableProperty] private string _name = "";
    [ObservableProperty] private string _hex = "";
}