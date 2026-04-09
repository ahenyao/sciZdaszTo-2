# System Motywów w ZdaszToApp

## Przegląd

Aplikacja używa stylu **Frutiger Aero** - charakterystycznego dla Windows Vista/7 z błyszczącymi elementami, gradientami i szkłem.

---

## Struktura Kolorów

### 1. Jasny Motyw (Light)

| Zmienna | Kolor | Zastosowanie |
|---------|-------|--------------|
| `FrutigerGreen` | `#8BC34A` | Główne akcenty |
| `FrutigerGreenLight` | `#C5E1A5` | Tła akcentowe |
| `FrutigerGreenDark` | `#689F38` | Ciemne akcenty |
| `FrutigerBlue` | `#03A9F4` | Elementy niebieskie |
| `FrutigerBlueLight` | `#B3E5FC` | Tła niebieskie |
| `FrutigerCyan` | `#00BCD4` | Elementy turkusowe |
| `FrutigerWhite` | `#F5F9F5` | Główne tło |
| `FrutigerLightGray` | `#E8F5E9` | Tła panele |

#### Gradienty

```xml
<!-- Tło aplikacji -->
<LinearGradientBrush StartPoint="0%,0%" EndPoint="0%,100%">
    <GradientStop Color="#E8F5E9" Offset="0"/>   <!-- jasny zielony -->
    <GradientStop Color="#C8E6C9" Offset="0.5"/>  <!-- średni zielony -->
    <GradientStop Color="#A5D6A7" Offset="1"/>    <!-- ciemny zielony -->
</LinearGradientBrush>

<!-- Przyciski (glossy effect) -->
<LinearGradientBrush StartPoint="0%,0%" EndPoint="0%,100%">
    <GradientStop Color="#AED581" Offset="0"/>    <!-- jasny -->
    <GradientStop Color="#8BC34A" Offset="0.4"/>   <!-- średni -->
    <GradientStop Color="#7CB342" Offset="1"/>     <!-- ciemny (błyszczący) -->
</LinearGradientBrush>

<!-- Karty -->
<LinearGradientBrush StartPoint="0%,0%" EndPoint="0%,100%">
    <GradientStop Color="#FFFFFF" Offset="0"/>
    <GradientStop Color="#E3F2FD" Offset="0.5"/>
    <GradientStop Color="#B3E5FC" Offset="1"/>
</LinearGradientBrush>
```

---

### 2. Ciemny Motyw (Dark)

| Zmienna | Kolor | Zastosowanie |
|---------|-------|--------------|
| `DarkBgPrimary` | `#0D1B2A` | Główne tło góra |
| `DarkBgSecondary` | `#1B263B` | Główne tło dół |
| `DarkBgTertiary` | `#0F3460` | Akcenty tła |
| `DarkSurface` | `#1E1E3F` | Karty |
| `DarkAccentGreen` | `#4CAF50` | Zielone akcenty |
| `DarkAccentBlue` | `#00BCD4` | Niebieskie akcenty |
| `DarkTextPrimary` | `#E8F5E9` | Główny tekst |
| `DarkTextSecondary` | `#81C784` | Drugorzędny tekst |

#### Gradienty Ciemne

```xml
<!-- Tło aplikacji -->
<LinearGradientBrush StartPoint="0%,0%" EndPoint="0%,100%">
    <GradientStop Color="#0D1B2A" Offset="0"/>
    <GradientStop Color="#1B263B" Offset="0.5"/>
    <GradientStop Color="#1B263B" Offset="1"/>
</LinearGradientBrush>

<!-- Karty ciemne -->
<LinearGradientBrush StartPoint="0%,0%" EndPoint="0%,100%">
    <GradientStop Color="#2D2D5A" Offset="0"/>
    <GradientStop Color="#1E1E3F" Offset="0.5"/>
    <GradientStop Color="#16213E" Offset="1"/>
</LinearGradientBrush>
```

---

## Jak Dodać Nowy Kolor

### Krok 1: Dodaj kolor do `FrutigerAero.axaml`

```xml
<Color x:Key="MojKolor">#HEX kod</Color>
<SolidColorBrush x:Key="MojKolorBrush" Color="#HEX kod"/>
```

### Krok 2: Użyj w widoku

```xml
<Border Background="{DynamicResource MojKolorBrush}">
```

---

## ThemeService

Klasa `ThemeService.cs` zarządza stanem motywu:

```csharp
public class ThemeService : INotifyPropertyChanged
{
    public static ThemeService Instance { get; }
    
    public bool IsDarkMode { get; set; }
    public bool IsLightMode => !IsDarkMode;
}
```

### Użycie w kodzie

```csharp
// Sprawdź stan
if (ThemeService.Instance.IsDarkMode) { }

// Nasłuchuj zmian
ThemeService.Instance.PropertyChanged += (s, e) => { };
```

---

## Implementacja w Widoku

### W kodzie C# (SettingsView.cs)

```csharp
private void ApplyTheme()
{
    bool isDark = ThemeService.Instance.IsDarkMode;
    
    var grid = this.FindControl<Grid>("MainGrid");
    var text = this.FindControl<TextBlock>("TitleText");
    var border = this.FindControl<Border>("CardBorder");
    
    if (isDark)
    {
        grid.Background = _darkBg;
        text.Foreground = _darkText;
        border.Background = _darkCardBg;
    }
    else
    {
        grid.Background = _lightBg;
        text.Foreground = _lightText;
        border.Background = _lightCardBg;
    }
}
```

### W kodzie XAML (Style)

```xml
<Style Selector="Border.dark">
    <Setter Property="Background" Value="{DynamicResource DarkGlossyCard}"/>
</Style>

<Style Selector="TextBlock.dark">
    <Setter Property="Foreground" Value="{DynamicResource DarkTextPrimaryBrush}"/>
</Style>
```

---

## Lista Plików

- `Themes/FrutigerAero.axaml` - Kolory i gradienty
- `Themes/FrutigerAeroStyles.axaml` - Style kontrolek
- `Services/ThemeService.cs` - Zarządzanie stanem
- `Views/SettingsView.axaml(.cs)` - Przykład implementacji

---

## Pomysł: GUI do Tworzenia Motywów

### Funkcjonalności

1. **Podgląd na żywo**
   - Edytuj kolory i widź zmiany natychmiast
   - Zmiany aplikują się do podglądu

2. **Edytor kolorów**
   - Input hex (#RRGGBB)
   - Suwak RGB
   - Wybór z palety
   - Kopiuj/wklej kolory

3. **Eksport**
   - Generowanie pliku `.axaml` z kolorami
   - Generowanie kodu C# dla ThemeService
   - Podgląd wygenerowanego kodu

4. **Preset motywów**
   - Zapisywanie/ładowanie konfiguracji
   - Wbudowane presety (Light, Dark, Blue, Purple, etc.)

### Propozycja UI

```
┌─────────────────────────────────────────────┐
│  🌈 Theme Designer - ZdaszToApp            │
├─────────────────────────────────────────────┤
│  [Light] [Dark] [Custom]                   │
├─────────────────────────────────────────────┤
│  Typ: Light                                │
│                                             │
│  ── Primary Colors ──                       │
│  [█████] #8BC34A  Primary Green            │
│  [█████] #03A9F4  Primary Blue             │
│  [█████] #00BCD4  Primary Cyan             │
│                                             │
│  ── Backgrounds ──                          │
│  [█████] #E8F5E9  Top                       │
│  [█████] #C8E6C9  Middle                   │
│  [█████] #A5D6A7  Bottom                   │
│                                             │
│  ── Cards ──                               │
│  [█████] #FFFFFF  Card Background          │
│                                             │
├─────────────────────────────────────────────┤
│  ┌─────────────────────────────────────┐    │
│  │     PODGLĄD APLIKACJI              │    │
│  │  ┌───────────────────────────┐     │    │
│  │  │  Menu     /home     👤   │     │    │
│  │  ├───────────────────────────┤     │    │
│  │  │                           │     │    │
│  │  │    [ INF.02 ] [ INF.03 ] │     │    │
│  │  │          [ INF.04 ]      │     │    │
│  │  │                           │     │    │
│  │  │    [ Zakupy ] [ Ranking ]│     │    │
│  │  └───────────────────────────┘     │    │
│  └─────────────────────────────────────┘    │
├─────────────────────────────────────────────┤
│  [Export AXAML] [Export C#] [Save Preset]  │
└─────────────────────────────────────────────┘
```

### Technologie

- **Avalonia UI** (już używane w projekcie)
- **.NET 9** (już używane)
- **MVVM** (CommunityToolkit.Mvvm)

### Krok następny

Chcesz żebym stworzył tę aplikację GUI do edycji motywów?