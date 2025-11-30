using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using AutoTyper.UI.Models;
using AutoTyper.UI.Views;

using WpfPoint = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;
using WpfApplication = System.Windows.Application;

namespace AutoTyper.UI.Services;

public class ImageDisplayService
{
    public async Task DisplayImageAsync(Snippet snippet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snippet);

        if (snippet.SnippetType != SnippetType.Image)
        {
            throw new ArgumentException("Snippet must be of type Image", nameof(snippet));
        }

        if (string.IsNullOrWhiteSpace(snippet.ImagePath) || !System.IO.File.Exists(snippet.ImagePath))
        {
            throw new System.IO.FileNotFoundException($"Image file not found: {snippet.ImagePath}");
        }

        // Load the image
        BitmapImage image = new();
        image.BeginInit();
        image.UriSource = new Uri(snippet.ImagePath, UriKind.Absolute);
        image.CacheOption = BitmapCacheOption.OnLoad;

        // Apply target resolution if specified
        if (snippet.TargetWidth > 0 || snippet.TargetHeight > 0)
        {
            ApplyTargetResolution(image, snippet.TargetWidth, snippet.TargetHeight, snippet.MaintainAspectRatio);
        }

        image.EndInit();
        image.Freeze();

        // Get the target screen based on monitor selection
        Screen targetScreen = GetTargetScreen(snippet.MonitorSelection, snippet.MonitorIndex);

        // Get DPI scaling factor for the target screen
        double dpiScaleX = 1.0;
        double dpiScaleY = 1.0;
        
        await WpfApplication.Current.Dispatcher.InvokeAsync(() =>
        {
            // Get the DPI for the screen
            var source = PresentationSource.FromVisual(WpfApplication.Current.MainWindow);
            if (source?.CompositionTarget != null)
            {
                dpiScaleX = source.CompositionTarget.TransformToDevice.M11;
                dpiScaleY = source.CompositionTarget.TransformToDevice.M22;
            }
        }, DispatcherPriority.Normal);

        // Calculate the actual size of the image in device-independent pixels
        double imageWidthDip = image.PixelWidth / dpiScaleX;
        double imageHeightDip = image.PixelHeight / dpiScaleY;

        // Calculate position based on corner and offset (in device-independent pixels)
        WpfPoint windowPosition = CalculateWindowPosition(
            targetScreen,
            imageWidthDip,
            imageHeightDip,
            snippet.Corner,
            snippet.OffsetX,
            snippet.OffsetY,
            dpiScaleX,
            dpiScaleY);

        // Display the window on the UI thread
        ImageDisplayWindow? window = null;
        await WpfApplication.Current.Dispatcher.InvokeAsync(() =>
        {
            window = new ImageDisplayWindow
            {
                Left = windowPosition.X,
                Top = windowPosition.Y
            };
            window.SetImage(image);
            window.Show();
        }, DispatcherPriority.Normal, cancellationToken);

        // Wait for the specified duration
        await Task.Delay(TimeSpan.FromSeconds(snippet.DisplayDuration), cancellationToken);

        // Close the window
        if (window != null)
        {
            await WpfApplication.Current.Dispatcher.InvokeAsync(() =>
            {
                window.Close();
            }, DispatcherPriority.Normal, cancellationToken);
        }
    }

    private static Screen GetTargetScreen(MonitorSelection monitorSelection, int monitorIndex)
    {
        return monitorSelection switch
        {
            MonitorSelection.PrimaryMonitor => Screen.PrimaryScreen ?? Screen.AllScreens[0],
            MonitorSelection.MonitorByIndex => GetScreenByIndex(monitorIndex),
            MonitorSelection.CursorMonitor => GetScreenFromCursor(),
            _ => GetScreenFromCursor()
        };
    }

    private static Screen GetScreenByIndex(int index)
    {
        Screen[] screens = Screen.AllScreens;
        if (index >= 0 && index < screens.Length)
        {
            return screens[index];
        }
        // Fallback to primary screen if index is out of range
        return Screen.PrimaryScreen ?? screens[0];
    }

    private static Screen GetScreenFromCursor()
    {
        WpfPoint cursorPosition = GetCursorPosition();
        return Screen.FromPoint(new DrawingPoint((int)cursorPosition.X, (int)cursorPosition.Y));
    }

    private static void ApplyTargetResolution(BitmapImage image, int targetWidth, int targetHeight, bool maintainAspectRatio)
    {
        if (targetWidth > 0 && targetHeight > 0)
        {
            // Both dimensions specified
            if (maintainAspectRatio)
            {
                // Calculate which dimension to use based on aspect ratio
                image.DecodePixelWidth = targetWidth;
                image.DecodePixelHeight = targetHeight;
            }
            else
            {
                // Stretch to exact dimensions
                image.DecodePixelWidth = targetWidth;
                image.DecodePixelHeight = targetHeight;
            }
        }
        else if (targetWidth > 0)
        {
            // Only width specified
            image.DecodePixelWidth = targetWidth;
        }
        else if (targetHeight > 0)
        {
            // Only height specified
            image.DecodePixelHeight = targetHeight;
        }
    }

    private static WpfPoint GetCursorPosition()
    {
        if (NativeMethods.GetCursorPos(out NativeMethods.POINT point))
        {
            return new WpfPoint(point.X, point.Y);
        }
        return new WpfPoint(0, 0);
    }

    private static WpfPoint CalculateWindowPosition(
        Screen screen,
        double imageWidth,
        double imageHeight,
        ImageCorner corner,
        int offsetX,
        int offsetY,
        double dpiScaleX,
        double dpiScaleY)
    {
        // Convert screen bounds from physical pixels to device-independent pixels
        Rectangle bounds = screen.WorkingArea;
        double left = bounds.Left / dpiScaleX;
        double top = bounds.Top / dpiScaleY;
        double right = bounds.Right / dpiScaleX;
        double bottom = bounds.Bottom / dpiScaleY;
        
        // Convert offsets from pixels to device-independent pixels
        double offsetXDip = offsetX / dpiScaleX;
        double offsetYDip = offsetY / dpiScaleY;

        double x = 0;
        double y = 0;

        switch (corner)
        {
            case ImageCorner.TopLeft:
                x = left + offsetXDip;
                y = top + offsetYDip;
                break;

            case ImageCorner.TopRight:
                x = right - imageWidth - offsetXDip;
                y = top + offsetYDip;
                break;

            case ImageCorner.BottomLeft:
                x = left + offsetXDip;
                y = bottom - imageHeight - offsetYDip;
                break;

            case ImageCorner.BottomRight:
                x = right - imageWidth - offsetXDip;
                y = bottom - imageHeight - offsetYDip;
                break;
        }

        return new WpfPoint(x, y);
    }

    private static partial class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out POINT lpPoint);
    }
}
