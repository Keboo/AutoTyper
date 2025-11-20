using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTyper.UI.Models;

public partial class Snippet : ObservableObject
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
    private int _order;
}
