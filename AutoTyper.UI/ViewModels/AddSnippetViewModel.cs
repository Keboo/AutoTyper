using AutoTyper.UI.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTyper.UI.ViewModels;

public partial class AddSnippetViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _fastTyping;

    [ObservableProperty]
    private double _delay = 3.0;

    [ObservableProperty]
    private bool _appendNewLine;

    [ObservableProperty]
    private bool _useClipboard;

    public Snippet GetSnippet()
    {
        return new()
        {
            Name = UseClipboard ? "<Clipboard>" : Name.Trim(),
            Content = Content,
            FastTyping = FastTyping,
            Delay = Delay,
            AppendNewLine = AppendNewLine,
            UseClipboard = UseClipboard
        };
    }
}
