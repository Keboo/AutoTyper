using System.IO;
using MaterialDesignThemes.Wpf;

namespace AutoTyper.UI.Services;

public class ThemeService
{
    private readonly PaletteHelper _paletteHelper = new();
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AutoTyper",
        "settings.json");

    public bool IsDarkMode { get; private set; }

    public ThemeService()
    {
        // Load saved theme preference or default to light mode
        IsDarkMode = LoadThemePreference();
        ApplyTheme(IsDarkMode);
    }

    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        ApplyTheme(IsDarkMode);
        SaveThemePreference(IsDarkMode);
    }

    public void SetTheme(bool isDarkMode)
    {
        if (IsDarkMode != isDarkMode)
        {
            IsDarkMode = isDarkMode;
            ApplyTheme(IsDarkMode);
            SaveThemePreference(IsDarkMode);
        }
    }

    private void ApplyTheme(bool isDarkMode)
    {
        Theme theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(isDarkMode ? BaseTheme.Dark : BaseTheme.Light);
        _paletteHelper.SetTheme(theme);
    }

    private bool LoadThemePreference()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                var settings = System.Text.Json.JsonSerializer.Deserialize<ThemeSettings>(json);
                return settings?.IsDarkMode ?? false;
            }
        }
        catch
        {
            // If there's any error reading the file, default to light mode
        }
        return false;
    }

    private void SaveThemePreference(bool isDarkMode)
    {
        try
        {
            var settings = new ThemeSettings { IsDarkMode = isDarkMode };
            string json = System.Text.Json.JsonSerializer.Serialize(settings);
            
            // Ensure directory exists
            string? directory = Path.GetDirectoryName(SettingsPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(SettingsPath, json);
        }
        catch
        {
            // Silently fail if we can't save the preference
        }
    }

    private class ThemeSettings
    {
        public bool IsDarkMode { get; set; }
    }
}
