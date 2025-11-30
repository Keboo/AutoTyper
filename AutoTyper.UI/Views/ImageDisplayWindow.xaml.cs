using System.Windows;
using System.Windows.Media.Imaging;

namespace AutoTyper.UI.Views;

public partial class ImageDisplayWindow : Window
{
    public ImageDisplayWindow()
    {
        InitializeComponent();
    }

    public void SetImage(BitmapImage image)
    {
        ArgumentNullException.ThrowIfNull(image);
        DisplayImage.Source = image;
    }
}
