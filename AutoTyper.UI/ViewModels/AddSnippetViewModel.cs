using AutoTyper.UI.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

namespace AutoTyper.UI.ViewModels;

public partial class AddSnippetViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTextSnippet))]
    [NotifyPropertyChangedFor(nameof(IsImageSnippet))]
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
    private bool _useTargetWindow;

    [ObservableProperty]
    private string _targetWindowTitle = string.Empty;

    [ObservableProperty]
    private bool _appendNewLine;

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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMonitorIndexEnabled))]
    private MonitorSelection _monitorSelection = MonitorSelection.CursorMonitor;

    [ObservableProperty]
    private int _monitorIndex;

    public bool IsTextSnippet => SnippetType == SnippetType.Text;
    public bool IsImageSnippet => SnippetType == SnippetType.Image;
    public bool IsMonitorIndexEnabled => MonitorSelection == MonitorSelection.MonitorByIndex;

    [RelayCommand]
    private void BrowseImage()
    {
        Microsoft.Win32.OpenFileDialog dialog = new()
        {
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff|All Files|*.*",
            Title = "Select an Image"
        };

        if (dialog.ShowDialog() == true)
        {
            ImagePath = dialog.FileName;
        }
    }

    public Snippet GetSnippet()
    {
        return new()
        {
            SnippetType = SnippetType,
            Name = SnippetType == SnippetType.Image
                ? (string.IsNullOrWhiteSpace(Name) ? System.IO.Path.GetFileName(ImagePath) : Name.Trim())
                : (UseClipboard ? "<Clipboard>" : Name.Trim()),
            Content = Content,
            FastTyping = FastTyping,
            Delay = Delay,
            UseTargetWindow = UseTargetWindow,
            TargetWindowTitle = TargetWindowTitle,
            AppendNewLine = AppendNewLine,
            UseClipboard = UseClipboard,
            ImagePath = ImagePath,
            DisplayDuration = DisplayDuration,
            Corner = Corner,
            OffsetX = OffsetX,
            OffsetY = OffsetY,
            TargetWidth = TargetWidth,
            TargetHeight = TargetHeight,
            MaintainAspectRatio = MaintainAspectRatio,
            MonitorSelection = MonitorSelection,
            MonitorIndex = MonitorIndex
        };
    }
}
