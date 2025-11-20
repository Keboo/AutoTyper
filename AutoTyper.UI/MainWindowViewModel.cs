using System.Collections.ObjectModel;
using System.Xml.Linq;

using AutoTyper.UI.Models;
using AutoTyper.UI.Services;
using AutoTyper.UI.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GongSolutions.Wpf.DragDrop;

using MaterialDesignThemes.Wpf;

namespace AutoTyper.UI;

public enum DialogResult
{
    None,
    Confirmed,
}

public partial class MainWindowViewModel : ObservableObject, IDropTarget
{
    private readonly SnippetStorageService _storageService;
    private readonly TypingService _typingService;
    private readonly ThemeService _themeService;

    [ObservableProperty]
    private bool _isTopMost;

    [ObservableProperty]
    private bool _isDarkMode;

    public ObservableCollection<Snippet> Snippets { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSnippetCommand))]
    private bool _isExecuting;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    public MainWindowViewModel(
        SnippetStorageService storageService,
        TypingService typingService,
        ThemeService themeService)
    {
        ArgumentNullException.ThrowIfNull(storageService);
        ArgumentNullException.ThrowIfNull(typingService);
        ArgumentNullException.ThrowIfNull(themeService);
        
        _storageService = storageService;
        _typingService = typingService;
        _themeService = themeService;

        // Initialize theme state
        IsDarkMode = _themeService.IsDarkMode;
        
        // Load snippets on startup
        _ = LoadSnippetsAsync();
    }

    private async Task LoadSnippetsAsync()
    {
        try
        {
            Snippets.Clear();
            List<Snippet> snippets = await _storageService.LoadSnippetsAsync();
            
            // Sort by Order property
            snippets = snippets.OrderBy(s => s.Order).ToList();
            
            // Ensure Order values are sequential
            for (int i = 0; i < snippets.Count; i++)
            {
                snippets[i].Order = i;
            }
            
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
            newSnippet.Order = Snippets.Count;

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
                newSnippet.Order = snippet.Order;
                Snippets[index] = newSnippet;
            }
            else
            {
                newSnippet.Order = Snippets.Count;
                Snippets.Add(newSnippet);
            }
            await _storageService.SaveSnippetsAsync(Snippets);
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        _themeService.ToggleTheme();
        IsDarkMode = _themeService.IsDarkMode;
        StatusMessage = $"Switched to {(IsDarkMode ? "dark" : "light")} mode";
    }

    // IDropTarget implementation for drag-drop reordering
    public void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is Snippet && dropInfo.TargetCollection != null)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
    }

    public async void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is Snippet sourceSnippet && dropInfo.TargetCollection != null)
        {
            int oldIndex = Snippets.IndexOf(sourceSnippet);
            int newIndex = dropInfo.InsertIndex;

            if (oldIndex >= 0 && newIndex >= 0 && oldIndex != newIndex)
            {
                // Adjust new index if moving down
                if (newIndex > oldIndex)
                {
                    newIndex--;
                }

                // Move the snippet
                Snippets.Move(oldIndex, newIndex);

                // Update Order property for all snippets
                for (int i = 0; i < Snippets.Count; i++)
                {
                    Snippets[i].Order = i;
                }

                // Persist the new order
                await SaveSnippetsAsync();
                StatusMessage = $"Reordered snippet: {sourceSnippet.Name}";
            }
        }
    }
}
