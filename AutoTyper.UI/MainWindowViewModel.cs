using System.Collections.ObjectModel;
using System.Xml.Linq;

using AutoTyper.UI.Models;
using AutoTyper.UI.Services;
using AutoTyper.UI.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MaterialDesignThemes.Wpf;

namespace AutoTyper.UI;

public enum DialogResult
{
    None,
    Confirmed,
}

public partial class MainWindowViewModel : ObservableObject
{
    private readonly SnippetStorageService _storageService;
    private readonly TypingService _typingService;

    [ObservableProperty]
    private bool _isTopMost;

    public ObservableCollection<Snippet> Snippets { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSnippetCommand))]
    private bool _isExecuting;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    public MainWindowViewModel(
        SnippetStorageService storageService,
        TypingService typingService)
    {
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        _typingService = typingService ?? throw new ArgumentNullException(nameof(typingService));

        // Load snippets on startup
        _ = LoadSnippetsAsync();
    }

    private async Task LoadSnippetsAsync()
    {
        try
        {
            Snippets.Clear();
            List<Snippet> snippets = await _storageService.LoadSnippetsAsync();
            foreach (var snippet in snippets)
            {
                Snippets.Add(snippet);
            }
            StatusMessage = $"Loaded {snippets.Count} snippet(s)";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading snippets: {ex.Message}";
        }
    }

    private async Task SaveSnippetsAsync()
    {
        try
        {
            await _storageService.SaveSnippetsAsync(Snippets);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving snippets: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task AddSnippetAsync()
    {
        var dialogViewModel = new AddSnippetViewModel();

        // Show dialog
        if (await DialogHost.Show(dialogViewModel, "RootDialog") is DialogResult.Confirmed)
        {
            Snippet newSnippet = dialogViewModel.GetSnippet();

            Snippets.Add(newSnippet);
            await SaveSnippetsAsync();
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteSnippet))]
    private async Task ExecuteSnippetAsync(Snippet? snippet)
    {
        if (snippet is null)
        {
            return;
        }

        try
        {
            IsExecuting = true;
            StatusMessage = $"Waiting {snippet.Delay} second(s) before typing...";

            // Start countdown
            if (snippet.Delay > 0)
            {
                StatusMessage = $"Typing in {snippet.Delay} second(s)... Focus target window now!";
                for (int i = (int)Math.Floor(snippet.Delay); i > 0; i--)
                {
                    StatusMessage = $"Typing in {i} second(s)... Focus target window now!";
                    await Task.Delay(1000);
                }
            }

            StatusMessage = "Typing...";
            await _typingService.TypeSnippetAsync(snippet);

            string targetWindow = _typingService.GetActiveWindowTitle();
            StatusMessage = $"Typed snippet '{snippet.Name}' to {targetWindow}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsExecuting = false;
        }
    }

    private bool CanExecuteSnippet(Snippet? snippet) => !IsExecuting && snippet is not null;

    [RelayCommand]
    private async Task DeleteSnippetAsync(Snippet? snippet)
    {
        if (snippet is null)
        {
            return;
        }

        Snippets.Remove(snippet);
        await SaveSnippetsAsync();
        StatusMessage = $"Deleted snippet: {snippet.Name}";
    }

    [RelayCommand]
    private async Task EditSnippetAsync(Snippet? snippet)
    {
        if (snippet is null)
        {
            return;
        }

        // Set up dialog for editing
        var dialogViewModel = new AddSnippetViewModel
        {
            Name = snippet.Name,
            Content = snippet.Content,
            FastTyping = snippet.FastTyping,
            Delay = snippet.Delay,
            AppendNewLine = snippet.AppendNewLine
        };

        // Show dialog
        if (await DialogHost.Show(dialogViewModel, "RootDialog") is DialogResult.Confirmed)
        {
            Snippet newSnippet = dialogViewModel.GetSnippet();
            var index = Snippets.IndexOf(snippet);
            if (index >= 0)
            {
                Snippets[index] = newSnippet;
            }
            else
            {
                Snippets.Add(newSnippet);
            }
            await _storageService.SaveSnippetsAsync(Snippets);
        }
    }
}
