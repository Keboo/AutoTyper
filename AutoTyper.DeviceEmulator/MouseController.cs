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
/// <visibility>public</visibility>
public sealed class MouseController : BaseController
{
    /// <summary>
    /// Provides DesktopWidth to calculate dimensions of movable space.
    /// </summary>
    /// <visibility>private</visibility>
    private int DesktopWidth { get; set; }

    /// <summary>
    /// Provides DesktopHight to calculate dimensions of movable space.
    /// </summary>
    /// <visibility>private</visibility>
    private int DesktopHeight { get; set; }

    /// <summary>
    /// Provides if the coordinates to move mouse is based on physical location if set true,
    /// and based on logical location if set false.
    /// </summary>
    /// <visibility>public</visibility>
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
    /// <visibility>public</visibility>
    public MouseController()
    {
        DesktopWidth = SystemInfo.PrimaryMonitorSize.Width;
        DesktopHeight = SystemInfo.PrimaryMonitorSize.Height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.MouseController" /> with
    /// <see cref="T:System.Threading.CancellationToken" /> as a parameter.
    /// </summary>
    /// <param name="aCancellationToken"></param>
    /// <visibility>public</visibility>
    public MouseController(CancellationToken aCancellationToken)
        : base(aCancellationToken)
    {
        DesktopWidth = SystemInfo.PrimaryMonitorSize.Width;
        DesktopHeight = SystemInfo.PrimaryMonitorSize.Height;
    }

    /// <summary>
    /// Provides all Screen display information.
    /// </summary>
    /// <visibility>public</visibility>
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
    /// <param name="aMouseFlag"></param>
    /// <param name="aScrollAmount"></param>
    /// <visibility>private</visibility>
    private void SendMouseInput(uint aMouseFlag, uint aScrollAmount = 0u)
    {
        inputBuffer = new Input
        {
            Type = 0u
        };
        inputBuffer.Data.Mouse.Flags = aMouseFlag;
        if (aScrollAmount != 0)
        {
            inputBuffer.Data.Mouse.MouseData = aScrollAmount;
        }
        inputList = new List<Input> { inputBuffer };
        if (base.RunMode == 0)
        {
            SafeNativeMethods.SendInput(1u, inputList.ToArray(), Marshal.SizeOf(typeof(Input)));
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
    /// <param name="aMouseButton"></param>
    /// <visibility>public</visibility>
    public void ButtonDown(MouseButton aMouseButton)
    {
        switch (aMouseButton)
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
    /// <param name="aMouseButton"></param>
    /// <visibility>public</visibility>
    public void ButtonUp(MouseButton aMouseButton)
    {
        switch (aMouseButton)
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
    /// <visibility>public</visibility>
    public void Click()
    {
        Click(MouseButton.Left);
    }

    /// <summary>
    /// Emulates clicking a mouse button.
    /// </summary>
    /// <param name="aMouseButton"></param>
    /// <visibility>public</visibility>
    public void Click(MouseButton aMouseButton)
    {
        Random random = new Random();
        int num = random.Next(0, 20);
        ButtonDown(aMouseButton);
        Sleep(num + 30);
        ButtonUp(aMouseButton);
    }

    /// <summary>
    /// Emulates pressing down on a left mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void LeftDown()
    {
        ButtonDown();
    }

    /// <summary>
    /// Emulates releasing a left mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void LeftUp()
    {
        ButtonUp();
    }

    /// <summary>
    /// Emulates pressing down on a right mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void RightDown()
    {
        ButtonDown(MouseButton.Right);
    }

    /// <summary>
    /// Emulates releasing a right mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void RightUp()
    {
        ButtonUp(MouseButton.Right);
    }

    /// <summary>
    /// Emulates pressing down on a middle mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void MiddleDown()
    {
        ButtonDown(MouseButton.Middle);
    }

    /// <summary>
    /// Emulates releasing a middle mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void MiddleUp()
    {
        ButtonUp(MouseButton.Middle);
    }

    /// <summary>
    /// Emulates scrolling down on a mouse wheel towards the user.
    /// </summary>
    /// <visibility>public</visibility>
    public void WheelDown()
    {
        int aScrollAmount = -120;
        SendMouseInput(2048u, (uint)aScrollAmount);
    }

    /// <summary>
    /// Emulates scrolling up on a mouse wheel away from the user.
    /// </summary>
    /// <visibility>public</visibility>
    public void WheelUp()
    {
        int aScrollAmount = 120;
        SendMouseInput(2048u, (uint)aScrollAmount);
    }

    /// <summary>
    /// Converts physical point coordinates to logical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="aPoint"></param>
    /// <returns></returns>
    /// <visibility>public</visibility>
    public static System.Drawing.Point PhysicalToLogicalPoint(System.Drawing.Point aPoint)
    {
        System.Drawing.Point aPoint2 = aPoint;
        SafeNativeMethods.PhysicalToLogicalPointForPerMonitorDPI(IntPtr.Zero, out aPoint2);
        return aPoint2;
    }

    /// <summary>
    /// Converts physical point coordinates to logical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="aPoint">Accepts <see cref="T:System.Windows.Point" /> as a parameter.</param>
    /// <returns></returns>
    public static System.Windows.Point PhysicalToLogicalPoint(System.Windows.Point aPoint)
    {
        System.Drawing.Point aPoint2 = new System.Drawing.Point((int)aPoint.X, (int)aPoint.Y);
        aPoint2 = PhysicalToLogicalPoint(aPoint2);
        return new System.Windows.Point(aPoint2.X, aPoint2.Y);
    }

    /// <summary>
    /// Converts logical point coordinates to physical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="aPoint">Accepts <see cref="T:System.Drawing.Point" /> as a parameter.</param>
    /// <returns></returns>
    /// <visibility>public</visibility>
    public static System.Drawing.Point LogicalToPhysicalPoint(System.Drawing.Point aPoint)
    {
        System.Drawing.Point aPoint2 = aPoint;
        SafeNativeMethods.LogicalToPhysicalPointForPerMonitorDPI(IntPtr.Zero, out aPoint2);
        return aPoint2;
    }

    /// <summary>
    /// Converts logical point coordinates to physical point coordinates.
    /// </summary>
    /// <remarks>
    /// This method is supported on operating systems greater than Windows 8.1.
    /// </remarks>
    /// <param name="aPoint">Accepts <see cref="T:System.Windows.Point" /> as a parameter.</param>
    /// <returns></returns>
    public static System.Windows.Point LogicalToPhysicalPoint(System.Windows.Point aPoint)
    {
        System.Drawing.Point aPoint2 = new System.Drawing.Point((int)aPoint.X, (int)aPoint.Y);
        aPoint2 = LogicalToPhysicalPoint(aPoint2);
        return new System.Windows.Point(aPoint2.X, aPoint2.Y);
    }

    /// <summary>
    /// Emulates instantly moving a cursor to an absolute position on the desktop using x and y coordinates.
    /// </summary>
    /// <param name="aX">integer value of x coordinate of cursor destination</param>
    /// <param name="aY">integer value of y coordinate of cursor destination</param>
    /// <visibility>public</visibility>
    public void AbsoluteSinglePointMove(int aX, int aY)
    {
        System.Drawing.Point aPoint = new System.Drawing.Point(aX, aY);
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
        inputList = new List<Input> { inputBuffer };
        if (base.RunMode == 0)
        {
            SafeNativeMethods.SendInput((uint)inputList.Count, inputList.ToArray(), Marshal.SizeOf(typeof(Input)));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="aX"></param>
    /// <param name="aY"></param>
    public void RelativeMoveSendInput(int aX, int aY)
    {
        System.Drawing.Point aPoint = new System.Drawing.Point(aX, aY);
        if (UseLogicalCoordinate)
        {
            SafeNativeMethods.PhysicalToLogicalPointForPerMonitorDPI(IntPtr.Zero, out aPoint);
        }
        inputBuffer = new Input
        {
            Type = 0u
        };
        inputBuffer.Data.Mouse.Flags = 1u;
        inputBuffer.Data.Mouse.X = aX;
        inputBuffer.Data.Mouse.Y = aY;
        inputList = new List<Input> { inputBuffer };
        if (base.RunMode == 0)
        {
            SafeNativeMethods.SendInput((uint)inputList.Count, inputList.ToArray(), Marshal.SizeOf(typeof(Input)));
        }
    }

    /// <summary>
    /// Emulates instantly moving a cursor to an absolute position on the desktop using System.Drawing.Point.
    /// </summary>
    /// <param name="aDestination">System.Drawing.Point method of a Destination</param>
    /// <visibility>public</visibility>
    public void AbsoluteSinglePointMove(System.Drawing.Point aDestination)
    {
        AbsoluteSinglePointMove(aDestination.X, aDestination.Y);
    }

    /// <summary>
    /// Emulates moving the mouse cursor naturally as a relative position to the current position.
    /// The backend of Mouse movement uses a Logical coordinate system.
    /// If you want to move to a physical location, it would have to convert to Logical destination.
    /// </summary>
    /// <param name="aDestinationX"></param>
    /// <param name="aDestinationY"></param>
    /// <param name="aOffsetAccuracy"></param>
    public void Move(int aDestinationX, int aDestinationY, int aOffsetAccuracy = 0)
    {
        System.Drawing.Point point = new System.Drawing.Point(int.MinValue, int.MinValue);
        if (aOffsetAccuracy < 0)
        {
            throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, "aOffsetAccuracy can not be negative."));
        }
        System.Drawing.Point point2 = new System.Drawing.Point(aDestinationX, aDestinationY);
        if (!UseLogicalCoordinate)
        {
            point2 = PhysicalToLogicalPoint(new System.Drawing.Point(point2.X, point2.Y));
        }
        while (point.X < point2.X - aOffsetAccuracy || point.X > point2.X + aOffsetAccuracy || point.Y < point2.Y - aOffsetAccuracy || point.Y > point2.Y + aOffsetAccuracy)
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
            Sleep(20);
        }
    }

    /// <summary>
    /// Emulates naturally moving a cursor to a position on the desktop.
    /// </summary>
    /// <param name="aDestinationX">integer value of x coordinate of cursor destination</param>
    /// <param name="aDestinationY">integer value of y coordinate of cursor destination</param>
    /// <param name="aMovementVelocityLogFactor">velocity of pixel displacement factor based on intervals</param>
    /// <visibility>public</visibility>
    public void AbsoluteMove(int aDestinationX, int aDestinationY, double aMovementVelocityLogFactor = 1.0)
    {
        int x = CursorHelper.Position.X;
        int y = CursorHelper.Position.Y;
        double a = Math.Sqrt((aDestinationX - x) * (aDestinationX - x) + (aDestinationY - y) * (aDestinationY - y));
        int num = (int)Math.Log(a, 1.001 + 1.0 * aMovementVelocityLogFactor) + 5;
        double num2 = x;
        double num3 = y;
        for (int i = 1; i < num; i++)
        {
            double num4 = Math.Sin((double)i / (double)num * Math.PI) * 1.57;
            num2 += num4 * (double)(aDestinationX - x) / (double)num;
            num3 += num4 * (double)(aDestinationY - y) / (double)num;
            if (i == num)
            {
                num2 = aDestinationX;
                num3 = aDestinationY;
            }
            AbsoluteSinglePointMove((int)num2, (int)num3);
            Sleep(30);
        }
        AbsoluteSinglePointMove(aDestinationX, aDestinationY);
        Sleep(5);
    }

    /// <summary>
    /// Emulates naturally moving a cursor to a position on the desktop.
    /// </summary>
    /// <param name="aDestination">System.Drawing.Point coordinate of cursor destination</param>
    /// <param name="aOffsetAccuracy"></param>
    /// <visibility>public</visibility>
    public void Move(System.Drawing.Point aDestination, int aOffsetAccuracy = 0)
    {
        Move(aDestination.X, aDestination.Y, aOffsetAccuracy);
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
    /// <visibility>public</visibility>
    public void MoveRelative(System.Drawing.Point aDisplacement)
    {
        MoveRelative(aDisplacement.X, aDisplacement.Y);
    }

    /// <summary>
    /// Emulates a natural mouse movement followed by a click.
    /// </summary>
    /// <param name="aDestinationX">integer value of x coordinate of cursor destination</param>
    /// <param name="aDestinationY">integer value of y coordinate of cursor destination</param>
    /// <visibility>public</visibility>
    public void MoveClick(int aDestinationX, int aDestinationY)
    {
        Move(aDestinationX, aDestinationY);
        Sleep(50);
        Click();
    }

    /// <summary>
    /// Emulates a natural mouse movement followed by a click.
    /// </summary>
    /// <param name="aPoint">point of coordinate of cursor destination</param>
    /// <visibility>public</visibility>
    public void MoveClick(System.Drawing.Point aPoint)
    {
        MoveClick(aPoint.X, aPoint.Y);
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button down, and a delay, followed up with a release.
    /// </summary>
    /// <param name="aDestinationX"></param>
    /// <param name="aDestinationY"></param>
    /// <param name="aWaitPeriod"></param>
    /// <visibility>public</visibility>
    public void MoveClickHold(int aDestinationX, int aDestinationY, TimeSpan aWaitPeriod)
    {
        Random random = new Random();
        int num = random.Next(5, 20);
        Move(aDestinationX, aDestinationY);
        LeftDown();
        Sleep(aWaitPeriod + TimeSpan.FromMilliseconds(num));
        LeftUp();
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button down, and a delay, followed up with a release.
    /// </summary>
    /// <param name="aPoint"></param>
    /// <param name="aWaitPeriod"></param>
    /// <visibility>public</visibility>
    public void MoveClickHold(System.Drawing.Point aPoint, TimeSpan aWaitPeriod)
    {
        MoveClickHold(aPoint.X, aPoint.Y, aWaitPeriod);
    }

    /// <summary>
    /// Emulate a natural mouse down of left and right down button, and a delay, 
    /// </summary>
    /// <param name="aHoldPeriod"></param>
    public void LeftRightClickHold(TimeSpan aHoldPeriod)
    {
        Random random = new Random();
        int num = random.Next(5, 20);
        ButtonDown(MouseButton.Left);
        ButtonDown(MouseButton.Right);
        Sleep(aHoldPeriod + TimeSpan.FromMilliseconds(num));
        ButtonUp(MouseButton.Left);
        ButtonUp(MouseButton.Right);
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button click and wait.
    /// </summary>
    /// <param name="aDestinationX"></param>
    /// <param name="aDestinationY"></param>
    /// <param name="aWaitPeriod"></param>
    /// <visibility>public</visibility>
    public void MoveClickDelay(int aDestinationX, int aDestinationY, TimeSpan aWaitPeriod)
    {
        MoveClick(aDestinationX, aDestinationY);
        Sleep(aWaitPeriod);
    }

    /// <summary>
    /// Emulates a natural mouse movement, follow by left button click and wait.
    /// </summary>
    /// <param name="aPoint"></param>
    /// <param name="aWaitPeriod"></param>
    /// <visibility>public</visibility>
    public void MoveClickDelay(System.Drawing.Point aPoint, TimeSpan aWaitPeriod)
    {
        MoveClickDelay(aPoint.X, aPoint.Y, aWaitPeriod);
    }

    /// <summary>
    /// Emulates a natural mouse movement which includes a delay between move and click action.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="aWaitPeriod"></param>
    /// <visibility>public</visibility>
    public void MoveDelayClick(int x, int y, TimeSpan aWaitPeriod)
    {
        Move(x, y);
        Sleep(aWaitPeriod);
        Click();
    }

    /// <summary>
    /// Emulates a natural mouse movement which includes a delay beetween move and click action.
    /// </summary>
    /// <param name="aPoint"></param>
    /// <param name="aWaitPeriod"></param>
    /// <visibility>public</visibility>
    public void MoveDelayClick(System.Drawing.Point aPoint, TimeSpan aWaitPeriod)
    {
        MoveDelayClick(aPoint.X, aPoint.Y, aWaitPeriod);
    }

    /// <summary>
    /// Emulates double clicking Left mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void DoubleClick()
    {
        Random random = new Random();
        int num = random.Next(0, 20);
        Click();
        Sleep(num + 35);
        Click();
    }

    /// <summary>
    /// Emulates clicking Middle mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void MiddleClick()
    {
        Random random = new Random();
        int num = random.Next(0, 20);
        ButtonDown(MouseButton.Middle);
        Sleep(num + 35);
        ButtonUp(MouseButton.Middle);
        Sleep(num + 35);
    }

    /// <summary>
    /// Emulates clicking Right mouse button.
    /// </summary>
    /// <visibility>public</visibility>
    public void RightClick()
    {
        Random random = new Random();
        int num = random.Next(0, 20);
        ButtonDown(MouseButton.Right);
        Sleep(num + 35);
        ButtonUp(MouseButton.Right);
        Sleep(num + 35);
    }

    /// <summary>
    /// Emulates a drag and drop motion.
    /// </summary>
    /// <param name="aOriginX">integer value of x coordinate of drag point</param>
    /// <param name="aOriginY">integer value of x coordinate of drag point</param>
    /// <param name="aDestinationX">integer value of x coordinate of drop destination</param>
    /// <param name="aDestinationY">integer value of x coordinate of drop destination</param>
    /// <visibility>public</visibility>
    public void DragDrop(int aOriginX, int aOriginY, int aDestinationX, int aDestinationY)
    {
        AbsoluteMove(aOriginX, aOriginY);
        LeftDown();
        AbsoluteMove(aDestinationX, aDestinationY);
        LeftUp();
    }

    /// <summary>
    /// Emulate a drag and drop motion.
    /// </summary>
    /// <param name="aFirstPoint"></param>
    /// <param name="aSecondPoint"></param>
    /// <visibility>public</visibility>
    public void DragDrop(System.Drawing.Point aFirstPoint, System.Drawing.Point aSecondPoint)
    {
        DragDrop(aFirstPoint.X, aFirstPoint.Y, aSecondPoint.X, aSecondPoint.Y);
    }
}