using System.IO;
using System.Text.Json;
using AutoTyper.UI.Models;

namespace AutoTyper.UI.Services;

public class SnippetStorageService
{
    private static readonly string AppDataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AutoTyper");

    private static readonly string SnippetsFilePath = Path.Combine(AppDataFolder, "snippets.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public SnippetStorageService()
    {
        // Ensure the app data folder exists
        if (!Directory.Exists(AppDataFolder))
        {
            Directory.CreateDirectory(AppDataFolder);
        }
    }

    public virtual async Task<List<Snippet>> LoadSnippetsAsync()
    {
        if (!File.Exists(SnippetsFilePath))
        {
            return [];
        }

        try
        {
            await using FileStream stream = File.OpenRead(SnippetsFilePath);
            List<Snippet>? snippets = await JsonSerializer.DeserializeAsync<List<Snippet>>(stream, JsonOptions);
            return snippets ?? [];
        }
        catch (Exception ex)
        {
            // Log error and return empty list
            Console.Error.WriteLine($"Error loading snippets: {ex.Message}");
            return [];
        }
    }

    public virtual async Task SaveSnippetsAsync(params IEnumerable<Snippet> snippets)
    {
        try
        {
            await using FileStream stream = File.Create(SnippetsFilePath);
            await JsonSerializer.SerializeAsync(stream, snippets, JsonOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error saving snippets: {ex.Message}");
            throw;
        }
    }
}
