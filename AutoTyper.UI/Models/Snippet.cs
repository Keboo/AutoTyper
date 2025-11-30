using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTyper.UI.Models;

public enum SnippetType
{
    Text,
    Image
}

public enum ImageCorner
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

public partial class Snippet : ObservableObject
{
    [ObservableProperty]
    private SnippetType _snippetType = SnippetType.Text;

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

    [ObservableProperty]
    private bool _useClipboard;

    // Image-specific properties
    [ObservableProperty]
    private string _imagePath = string.Empty;

    [ObservableProperty]
    private double _displayDuration = 5.0;

    [ObservableProperty]
    private ImageCorner _corner = ImageCorner.TopRight;

    [ObservableProperty]
    private int _offsetX;

    [ObservableProperty]
    private int _offsetY;

    [ObservableProperty]
    private int _targetWidth;

    [ObservableProperty]
    private int _targetHeight;

    [ObservableProperty]
    private bool _maintainAspectRatio = true;
}
