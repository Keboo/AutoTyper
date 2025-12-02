using System.IO;
using MaterialDesignThemes.Wpf;

namespace AutoTyper.UI.Services;

public class ThemeService
{
    private readonly PaletteHelper _paletteHelper = new();
    private static string SettingsPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AutoTyper",
        "settings.json");

    public virtual bool IsDarkMode { get; private set; }
    public virtual string ViewMode { get; private set; } = "Full";

    public ThemeService()
    {
        // Load saved theme preference or default to light mode
        var settings = LoadSettings();
        IsDarkMode = settings.IsDarkMode;
        ViewMode = settings.ViewMode ?? "Full";
        
        // Only apply theme if we're in a WPF application context
        if (System.Windows.Application.Current != null)
        {
            ApplyTheme(IsDarkMode);
        }
    }

    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        ApplyTheme(IsDarkMode);
        SaveSettings();
    }

    public void SetTheme(bool isDarkMode)
    {
        if (IsDarkMode != isDarkMode)
        {
            IsDarkMode = isDarkMode;
            ApplyTheme(IsDarkMode);
            SaveSettings();
        }
    }

    public void SetViewMode(string viewMode)
    {
        if (ViewMode != viewMode)
        {
            ViewMode = viewMode;
            SaveSettings();
        }
    }

    private void ApplyTheme(bool isDarkMode)
    {
        Theme theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(isDarkMode ? BaseTheme.Dark : BaseTheme.Light);
        _paletteHelper.SetTheme(theme);
    }

    private ThemeSettings LoadSettings()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                var settings = System.Text.Json.JsonSerializer.Deserialize<ThemeSettings>(json);
                return settings ?? new ThemeSettings();
            }
        }
        catch
        {
            // If there's any error reading the file, return defaults
        }
        return new ThemeSettings();
    }

    private void SaveSettings()
    {
        try
        {
            var settings = new ThemeSettings 
            { 
                IsDarkMode = IsDarkMode,
                ViewMode = ViewMode
            };
            string json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            
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
        public string? ViewMode { get; set; }
    }
}
