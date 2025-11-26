#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

using AutoTyper.DeviceEmulator.Native;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// Provides a mechanism to emulate sending commands to physical mouse device by calling methods.
/// </summary>
/// <remarks>
/// MouseController class offers the ability to control mouse devices.
/// In order to use MouseController, create an instance by using a constructor.
/// Then in order to emulate controlling the mouse, call the methods.
/// </remarks>
/// <example>
/// Mouse Controller Usage
/// <code>
/// // Use Mouse Controller to Move to certain coordinate on desktop.
/// public void MoveMouse()
/// {
///     // Initialize MouseController.
///     MouseController mouse = new MouseController();
///
///     // Emulate movement of the mouse.
///     mouse.Move(new System.Drawing.Point(100, 100));
/// }
/// </code>
/// </example>
public sealed class MouseController : BaseController
{
    /// <summary>
    /// Provides DesktopWidth to calculate dimensions of movable space.
    /// </summary>
    private int DesktopWidth { get; set; }

    /// <summary>
    /// Provides DesktopHight to calculate dimensions of movable space.
    /// </summary>
    private int DesktopHeight { get; set; }

    /// <summary>
    /// Provides if the coordinates to move mouse is based on physical location if set true,
    /// and based on logical location if set false.
    /// </summary>
    [Obsolete("Use UseLogicalCoordinate property. Made obsolete in v1.1.8")]
    public bool PhysicalToLogicalMoveMode { get; set; }

    /// <summary>
    /// If set true, use Logical coordinates.
    /// </summary>
    /// <remarks>
    /// Physical Coordinates: Actual dimensions of the display - regardless of scaling.
    /// Logical Coordinates: Display representation, of the actual pixels.
    /// </remarks>
    public bool UseLogicalCoordinate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.MouseController" /> with no arguments.
    /// </summary>
    /// <example>
    /// <code>
    /// // Initialize MouseController.
    /// MouseController mouse = new MouseController();
    /// </code>
    /// </example>
    public MouseController()
    {
        DesktopWidth = SystemInfo.PrimaryMonitorSize.Width;
        DesktopHeight = SystemInfo.PrimaryMonitorSize.Height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.MouseController" /> with
    /// <see cref="T:System.Threading.CancellationToken" /> as a parameter.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public MouseController(CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        DesktopWidth = SystemInfo.PrimaryMonitorSize.Width;
        DesktopHeight = SystemInfo.PrimaryMonitorSize.Height;
    }

    /// <summary>
    /// Provides all Screen display information.
    /// </summary>
    public static void ShowDisplayInfo()
    {
        ScreenInfo[] allScreens = ScreenInfo.AllScreens;
        foreach (ScreenInfo screen in allScreens)
        {
            Trace.WriteLine("Device Name: " + screen.DeviceName);
            Trace.WriteLine("Bounds: " + screen.Bounds.ToString());
            Trace.WriteLine("Type: " + screen.GetType().ToString());
            Trace.WriteLine("Working Area: " + screen.WorkingArea.ToString());
            Trace.WriteLine("Primary Screen: " + screen.Primary.ToString(CultureInfo.CurrentCulture));
        }
    }

    /// <summary>
    /// Default Cursor.Position will return logical coordinates.
    /// This method will return coordinates based on UseLogicalCoordinate flag.
    /// </summary>
    /// <returns></returns>
    public System.Drawing.Point GetMouseCursorPosition()
    {
        System.Drawing.Point position = CursorHelper.Position;
        if (UseLogicalCoordinate)
        {
            return position;
        }
        return LogicalToPhysicalPoint(position);
    }

    /// <summary>
    /// Provides a way to generate an inputBuffer and sends to native SendInput method.
    /// </summary>
    /// <param name="mouseFlag"></param>
    /// <param name="scrollAmount"></param>
    private void SendMouseInput(uint mouseFlag, uint scrollAmount = 0u)
    {
        inputBuffer = new Input
        {
            Type = 0u
        };
        inputBuffer.Data.Mouse.Flags = mouseFlag;
        if (scrollAmount != 0)
        {
            inputBuffer.Data.Mouse.MouseData = scrollAmount;
        }
        inputList = [inputBuffer];
        if (RunMode == 0)
        {
            _ = SafeNativeMethods.SendInput(1u, [.. inputList], Marshal.SizeOf<Input>());
        }
        inputList.Clear();
    }

    /// <summary>
    /// Emulates pressing down on a mouse button.
    /// </summary>
    public void ButtonDown()
    {
        ButtonDown(MouseButton.Left);
    }

    /// <summary>
    /// Emulates pressing down on a mouse button.
    /// </summary>
    /// <param name="mouseButton"></param>
    public void ButtonDown(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                SendMouseInput(2u);
                break;
            case MouseButton.Right:
                SendMouseInput(8u);
                break;
            case MouseButton.Middle:
                SendMouseInput(32u);
                break;
            case MouseButton.XButton:
                SendMouseInput(128u);
                break;
            case MouseButton.LeftAndRight:
                SendMouseInput(2u);
                SendMouseInput(8u);
                break;
        }
    }

    /// <summary>
    /// Emulates releasing up a mouse button.
    /// </summary>
    public void ButtonUp()
    {
        ButtonUp(MouseButton.Left);
    }

    /// <summary>
    /// Emulates releasing up a mouse button.
    /// </summary>
    /// <param name="mouseButton"></param>
    public void ButtonUp(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                SendMouseInput(4u);
                break;
            case MouseButton.Right:
                SendMouseInput(16u);
                break;
            case MouseButton.Middle:
                SendMouseInput(64u);
                break;
            case MouseButton.XButton:
                SendMouseInput(256u);
                break;
            case MouseButton.LeftAndRight:
                SendMouseInput(4u);
                SendMouseInput(16u);
                break;
        }
    }

    /// <summary>
    /// Emulates clicking a mouse button.
    /// </summary>
    public Task ClickAsync()
    {
        return ClickAsync(MouseButton.Left);
    }

    /// <summary>
    /// Emulates clicking a mouse button.
    /// </summary>
    /// <param name="aMouseButton"></param>
    public async Task ClickAsync(MouseButton aMouseButton)
    {
        int num = Random.Shared.Next(0, 20);
        ButtonDown(aMouseButton);
        await DelayAsync(TimeSpan.FromMilliseconds(num + 30));
        ButtonUp(aMouseButton);
    }

    /// <summary>
    /// Emulates pressing down on a left mouse button.
    /// </summary>
    public void LeftDown()
    {
        ButtonDown();
    }

    /// <summary>
    /// Emulates releasing a left mouse button.
    /// </summary>
    public void LeftUp()
    {
        ButtonUp();
    }

    /// <summary>
    /// Emulates pressing down on a right mouse button.
    /// </summary>
    public void RightDown()
    {
        ButtonDown(MouseButton.Right);
    }

    /// <summary>
    /// Emulates releasing a right mouse button.
    /// </summary>
    public void RightUp()
    {
        ButtonUp(MouseButton.Right);
    }

    /// <summary>
    /// Emulates pressing down on a middle mouse button.
    /// </summary>
    public void MiddleDown()
    {
        ButtonDown(MouseButton.Middle);
    }

    /// <summary>
    /// Emulates releasing a middle mouse button.
    /// </summary>
    public void MiddleUp()
    {
        ButtonUp(MouseButton.Middle);
    }

    /// <summary>
    /// Emulates scrolling down on a mouse wheel towards the user.
    /// </summary>
    public void WheelDown()
    {
        int scrollAmount = -120;
        SendMouseInput(2048u, (uint)scrollAmount);
    }

    /// <summary>
    /// Emulates scrolling up on a mouse wheel away from the user.
    /// </summary>
    public void WheelUp()
    {
        int scrollAmount = 120;
        SendMouseInput(2048u, (uint)scrollAmount);
    }

    /// <summary>
    /// Converts physical point coordinates to logical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="point"></param>
    /// <returns></returns>
    public static System.Drawing.Point PhysicalToLogicalPoint(System.Drawing.Point point)
    {
        System.Drawing.Point point2 = point;
        SafeNativeMethods.PhysicalToLogicalPointForPerMonitorDPI(IntPtr.Zero, out point2);
        return point2;
    }

    /// <summary>
    /// Converts physical point coordinates to logical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="point">Accepts <see cref="T:System.Windows.Point" /> as a parameter.</param>
    /// <returns></returns>
    public static System.Windows.Point PhysicalToLogicalPoint(System.Windows.Point point)
    {
        System.Drawing.Point point2 = new((int)point.X, (int)point.Y);
        point2 = PhysicalToLogicalPoint(point2);
        return new System.Windows.Point(point2.X, point2.Y);
    }

    /// <summary>
    /// Converts logical point coordinates to physical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="point">Accepts <see cref="T:System.Drawing.Point" /> as a parameter.</param>
    /// <returns></returns>
    /// <visibility>public</visibility>
    public static System.Drawing.Point LogicalToPhysicalPoint(System.Drawing.Point point)
    {
        System.Drawing.Point point2 = point;
        SafeNativeMethods.LogicalToPhysicalPointForPerMonitorDPI(IntPtr.Zero, out point2);
        return point2;
    }

    /// <summary>
    /// Converts logical point coordinates to physical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="point">Accepts <see cref="T:System.Windows.Point" /> as a parameter.</param>
    /// <returns></returns>
    public static System.Windows.Point LogicalToPhysicalPoint(System.Windows.Point point)
    {
        System.Drawing.Point point2 = new((int)point.X, (int)point.Y);
        point2 = LogicalToPhysicalPoint(point2);
        return new System.Windows.Point(point2.X, point2.Y);
    }

    /// <summary>
    /// Emulates instantly moving a cursor to an absolute position on the desktop using x and y coordinates.
    /// </summary>
    /// <param name="x">integer value of x coordinate of cursor destination</param>
    /// <param name="y">integer value of y coordinate of cursor destination</param>
    public void AbsoluteSinglePointMove(int x, int y)
    {
        System.Drawing.Point aPoint = new(x, y);
        if (UseLogicalCoordinate)
        {
            SafeNativeMethods.PhysicalToLogicalPointForPerMonitorDPI(IntPtr.Zero, out aPoint);
        }
        inputBuffer = new Input
        {
            Type = 0u
        };
        inputBuffer.Data.Mouse.Flags = 32769u;
        inputBuffer.Data.Mouse.X = (int)(65535f * (float)aPoint.X / (float)DesktopWidth);
        inputBuffer.Data.Mouse.Y = (int)(65535f * (float)aPoint.Y / (float)DesktopHeight);
        inputList = [inputBuffer];
        if (RunMode == 0)
        {
            _ = SafeNativeMethods.SendInput((uint)inputList.Count, [.. inputList], Marshal.SizeOf<Input>());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RelativeMoveSendInput(int x, int y)
    {
        System.Drawing.Point point = new(x, y);
        if (UseLogicalCoordinate)
        {
            SafeNativeMethods.PhysicalToLogicalPointForPerMonitorDPI(IntPtr.Zero, out point);
        }
        inputBuffer = new Input
        {
            Type = 0u
        };
        inputBuffer.Data.Mouse.Flags = 1u;
        inputBuffer.Data.Mouse.X = x;
        inputBuffer.Data.Mouse.Y = y;
        inputList = [inputBuffer];
        if (RunMode == 0)
        {
            _ = SafeNativeMethods.SendInput((uint)inputList.Count, [.. inputList], Marshal.SizeOf<Input>());
        }
    }

    /// <summary>
    /// Emulates instantly moving a cursor to an absolute position on the desktop using System.Drawing.Point.
    /// </summary>
    /// <param name="destination">System.Drawing.Point method of a Destination</param>
    /// <visibility>public</visibility>
    public void AbsoluteSinglePointMove(System.Drawing.Point destination)
    {
        AbsoluteSinglePointMove(destination.X, destination.Y);
    }

    /// <summary>
    /// Emulates moving the mouse cursor naturally as a relative position to the current position.
    /// The backend of Mouse movement uses a Logical coordinate system.
    /// If you want to move to a physical location, it would have to convert to Logical destination.
    /// </summary>
    /// <param name="destinationX"></param>
    /// <param name="destinationY"></param>
    /// <param name="offsetAccuracy"></param>
    public async Task MoveAsync(int destinationX, int destinationY, int offsetAccuracy = 0)
    {
        System.Drawing.Point point = new(int.MinValue, int.MinValue);
        if (offsetAccuracy < 0)
        {
            throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, "aOffsetAccuracy can not be negative."));
        }
        System.Drawing.Point point2 = new(destinationX, destinationY);
        if (!UseLogicalCoordinate)
        {
            point2 = PhysicalToLogicalPoint(new System.Drawing.Point(point2.X, point2.Y));
        }
        while (point.X < point2.X - offsetAccuracy || point.X > point2.X + offsetAccuracy || point.Y < point2.Y - offsetAccuracy || point.Y > point2.Y + offsetAccuracy)
        {
            point = CursorHelper.Position;
            int num = point2.X - point.X;
            if (num != 0)
            {
                num = num / 10 + ((num > 0) ? 1 : (-1));
            }
            int num2 = point2.Y - point.Y;
            if (num2 != 0)
            {
                num2 = num2 / 10 + ((num2 > 0) ? 1 : (-1));
            }
            RelativeMoveSendInput(num, num2);
            await DelayAsync(TimeSpan.FromMilliseconds(20));
        }
    }

    /// <summary>
    /// Emulates naturally moving a cursor to a position on the desktop.
    /// </summary>
    /// <param name="destinationX">integer value of x coordinate of cursor destination</param>
    /// <param name="destinationY">integer value of y coordinate of cursor destination</param>
    /// <param name="movementVelocityLogFactor">velocity of pixel displacement factor based on intervals</param>
    public async Task AbsoluteMoveAsync(int destinationX, int destinationY, double movementVelocityLogFactor = 1.0)
    {
        int x = CursorHelper.Position.X;
        int y = CursorHelper.Position.Y;
        double a = Math.Sqrt((destinationX - x) * (destinationX - x) + (destinationY - y) * (destinationY - y));
        int num = (int)Math.Log(a, 1.001 + 1.0 * movementVelocityLogFactor) + 5;
        double num2 = x;
        double num3 = y;
        for (int i = 1; i < num; i++)
        {
            double num4 = Math.Sin((double)i / (double)num * Math.PI) * 1.57;
            num2 += num4 * (double)(destinationX - x) / (double)num;
            num3 += num4 * (double)(destinationY - y) / (double)num;
            if (i == num)
            {
                num2 = destinationX;
                num3 = destinationY;
            }
            AbsoluteSinglePointMove((int)num2, (int)num3);
            await DelayAsync(TimeSpan.FromMilliseconds(30));
        }
        AbsoluteSinglePointMove(destinationX, destinationY);
        await DelayAsync(TimeSpan.FromMilliseconds(5));
    }

    /// <summary>
    /// Emulates naturally moving a cursor to a position on the desktop.
    /// </summary>
    /// <param name="destination">System.Drawing.Point coordinate of cursor destination</param>
    /// <param name="offsetAccuracy"></param>
    public Task MoveAsync(System.Drawing.Point destination, int offsetAccuracy = 0)
    {
        return MoveAsync(destination.X, destination.Y, offsetAccuracy);
    }

    /// <summary>
    /// Emulates movement by a relative position.
    /// </summary>
    /// <param name="aDisplacementX"></param>
    /// <param name="aDisplacementY"></param>
    /// <visibility>public</visibility>
    public void MoveRelative(int aDisplacementX, int aDisplacementY)
    {
        RelativeMoveSendInput(aDisplacementX, aDisplacementY);
    }

    /// <summary>
    /// Emulates movement by a relative position.
    /// </summary>
    /// <param name="aDisplacement"></param>
    public void MoveRelative(System.Drawing.Point aDisplacement)
    {
        MoveRelative(aDisplacement.X, aDisplacement.Y);
    }

    /// <summary>
    /// Emulates a natural mouse movement followed by a click.
    /// </summary>
    /// <param name="destinationX">integer value of x coordinate of cursor destination</param>
    /// <param name="destinationY">integer value of y coordinate of cursor destination</param>
    public async Task MoveClickAsync(int destinationX, int destinationY)
    {
        await MoveAsync(destinationX, destinationY);
        await DelayAsync(TimeSpan.FromMilliseconds(50));
        await ClickAsync();
    }

    /// <summary>
    /// Emulates a natural mouse movement followed by a click.
    /// </summary>
    /// <param name="point">point of coordinate of cursor destination</param>
    public Task MoveClickAsync(System.Drawing.Point point)
    {
        return MoveClickAsync(point.X, point.Y);
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button down, and a delay, followed up with a release.
    /// </summary>
    /// <param name="destinationX"></param>
    /// <param name="destinationY"></param>
    /// <param name="waitPeriod"></param>
    public async Task MoveClickHoldAsync(int destinationX, int destinationY, TimeSpan waitPeriod)
    {
        int num = Random.Shared.Next(5, 20);
        await MoveAsync(destinationX, destinationY);
        LeftDown();
        await DelayAsync(waitPeriod + TimeSpan.FromMilliseconds(num));
        LeftUp();
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button down, and a delay, followed up with a release.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="waitPeriod"></param>
    public Task MoveClickHoldAsync(System.Drawing.Point point, TimeSpan waitPeriod)
    {
        return MoveClickHoldAsync(point.X, point.Y, waitPeriod);
    }

    /// <summary>
    /// Emulate a natural mouse down of left and right down button, and a delay, 
    /// </summary>
    /// <param name="holdPeriod"></param>
    public async Task LeftRightClickHoldAsync(TimeSpan holdPeriod)
    {
        int num = Random.Shared.Next(5, 20);
        ButtonDown(MouseButton.Left);
        ButtonDown(MouseButton.Right);
        await DelayAsync(holdPeriod + TimeSpan.FromMilliseconds(num));
        ButtonUp(MouseButton.Left);
        ButtonUp(MouseButton.Right);
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button click and wait.
    /// </summary>
    /// <param name="destinationX"></param>
    /// <param name="destinationY"></param>
    /// <param name="waitPeriod"></param>
    public async Task MoveClickDelayAsync(int destinationX, int destinationY, TimeSpan waitPeriod)
    {
        await MoveClickAsync(destinationX, destinationY);
        await DelayAsync(waitPeriod);
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button click and wait.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="waitPeriod"></param>
    public Task MoveClickDelayAsync(System.Drawing.Point point, TimeSpan waitPeriod)
    {
        return MoveClickDelayAsync(point.X, point.Y, waitPeriod);
    }

    /// <summary>
    /// Emulates a natural mouse movement which includes a delay between move and click action.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="waitPeriod"></param>
    public async Task MoveDelayClickAsync(int x, int y, TimeSpan waitPeriod)
    {
        await MoveAsync(x, y);
        await DelayAsync(waitPeriod);
        await ClickAsync();
    }

    /// <summary>
    /// Emulates a natural mouse movement which includes a delay beetween move and click action.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="waitPeriod"></param>
    public Task MoveDelayClickAsync(System.Drawing.Point point, TimeSpan waitPeriod)
    {
        return MoveDelayClickAsync(point.X, point.Y, waitPeriod);
    }

    /// <summary>
    /// Emulates double clicking Left mouse button.
    /// </summary>
    public async Task DoubleClickAsync()
    {
        int num = Random.Shared.Next(0, 20);
        await ClickAsync();
        await DelayAsync(TimeSpan.FromMilliseconds(num + 35));
        await ClickAsync();
    }

    /// <summary>
    /// Emulates clicking Middle mouse button.
    /// </summary>
    public async Task MiddleClickAsync()
    {
        int num = Random.Shared.Next(0, 20);
        ButtonDown(MouseButton.Middle);
        await DelayAsync(TimeSpan.FromMilliseconds(num + 35));
        ButtonUp(MouseButton.Middle);
        await DelayAsync(TimeSpan.FromMilliseconds(num + 35));
    }

    /// <summary>
    /// Emulates clicking Right mouse button.
    /// </summary>
    public async Task RightClickAsync()
    {
        int num = Random.Shared.Next(0, 20);
        ButtonDown(MouseButton.Right);
        await DelayAsync(TimeSpan.FromMilliseconds(num + 35));
        ButtonUp(MouseButton.Right);
        await DelayAsync(TimeSpan.FromMilliseconds(num + 35));
    }

    /// <summary>
    /// Emulates a drag and drop motion.
    /// </summary>
    /// <param name="originX">integer value of x coordinate of drag point</param>
    /// <param name="originY">integer value of x coordinate of drag point</param>
    /// <param name="destinationX">integer value of x coordinate of drop destination</param>
    /// <param name="destinationY">integer value of x coordinate of drop destination</param>
    public async Task DragDropAsync(int originX, int originY, int destinationX, int destinationY)
    {
        await AbsoluteMoveAsync(originX, originY);
        LeftDown();
        await AbsoluteMoveAsync(destinationX, destinationY);
        LeftUp();
    }

    /// <summary>
    /// Emulate a drag and drop motion.
    /// </summary>
    /// <param name="firstPoint"></param>
    /// <param name="secondPoint"></param>
    public Task DragDropAsync(System.Drawing.Point firstPoint, System.Drawing.Point secondPoint)
    {
        return DragDropAsync(firstPoint.X, firstPoint.Y, secondPoint.X, secondPoint.Y);
    }
}