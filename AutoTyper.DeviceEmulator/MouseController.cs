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
/// Provides a mechanism to emulate sending commands to pysical mouse device by calling methods.
/// </summary>
/// <remarks>
/// MouseController class offers ability to control and mouse devices.
/// In order to use MouseController, create an instance by using a constructor.
/// Then in order to emulate controlling the mouse, call the methods.
/// </remarks>
/// <example>
/// Mouse Controller Usage
/// <code>
/// // Use Mouse Controller to Move to certain coordinate on desktop.
/// public void MoveMouse()
/// {
///     // Initalize MouseController.
///     MouseController mouse = new MouseController();
///
///     // Emulate movement of the mouse.
///     mouse.Move(new System.Drawing.Point(100, 100));
/// }
/// </code>
/// </example>
/// <visibility>public</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2011-09-29  AS00000  v0.00.00.000  Initial Version
/// 2012-01-14  AS00000  v0.00.00.000  Moved wheeldown and wheelup to public functions
/// 2012-07-26  AS00000  v0.00.00.000  Ported C++ version to C# version
/// 2013-02-01  AS00000  v0.00.00.000  Added Point based calls for mouse movements
/// 2013-03-26  AS00002  v0.00.00.002  Revision History Update - renamed methods to follow standard format
/// 2013-04-07  AS00007  v0.00.00.007  Added middle mouse button features
/// 2013-06-14  AS00040  v0.00.00.040  More natural mouse movements, increased base iterations, Move() method
/// 2013-06-15  AS00041  v0.00.00.041  Move will ensure the mouse will move to the final destination
/// 2013-10-02  AS00115  v0.00.00.115  Added comments, renamed class and clean up code
/// 2013-10-15  AS00122  v0.00.00.122  GetCursorBitmap method implemented
/// 2013-10-21  AS00127  v0.00.00.127  GetCursorHash method returns the hash code of current cursor
/// 2014-02-28  AS00185  v0.00.00.185  Added MoveClickDelay methods
/// 2014-03-12  AS00200  v0.00.00.200  Updated ConsoleDebug variable as a public variable with getter and setter
/// 2014-03-18  AS00203  v0.00.00.203  Moved HenoohMouse to Henooh.Utility namespace
/// 2014-03-23  AS00210  v0.00.00.210  Properly dispose cursor Bitmap after calculating hash value
/// 2014-04-05  AS00222  v0.00.00.222  CaptureBitmap now handles rare exceptions thrown in System.Drawing.dll
/// 2014-04-08  AS00224  v0.00.00.224  Changed the class to be Public
/// 2014-04-14  AS00230  v0.00.00.230  Moved to HenoohUtility as a Class Library Project (dll)
/// 2014-04-17  AS00233  v0.00.00.233  MoveDelayClick method added and restore functionality of GetCursorHash
/// 2014-06-25  AS00261  v0.00.01.027  Redefined summary
/// 2014-08-31  AS00287  v0.00.01.053  Redefined mouse cursor to PrimaryMonitorSize
/// 2014-10-06  AS00311  v0.00.03.000  Removed dependency of HenoohVision, moved GetCursor methods to HenoohVision
/// 2014-10-22  AS00312  v0.00.03.001  Fixed a bug in MoveClickDelay method
/// 2015-03-19  AS00407  v0.00.03.011  Changed Input Parameters for DragDrop
/// 2015-04-02  AS00419  v0.00.03.012  Changed name from HenoohMouse to MiniMouse
/// 2015-04-02  AS00420  v0.00.04.000  Moved to HenoohInputSimulator Project
/// 2015-04-06  AS00424  v0.00.04.001  Removed all Debug features
/// 2015-04-07  AS00426  v0.00.04.002  Change default value for Move parameters
/// 2015-04-08  AS00427  v0.00.04.003  Added MoveRelative methods and standardized movementVelocityLogFactor
/// 2015-04-11  AS00431  v0.00.04.004  Changed paramter for time from integer to TimeSpan
/// 2015-04-21  AS00439  v0.00.04.008  Implemented new method called MoveDelayClick
/// 2015-04-22  AS00440  v0.00.04.009  Added comments with MoveDelayClick methods
/// 2015-04-24  AS00442  v0.00.04.010  SendInput extern is now a private static method, overloaded AbsoluteMove
/// 2015-04-27  AS00444  v0.00.04.011  Changed the aMovementVelocityLogFactor where 0 is the lowest speed
/// 2015-05-06  AS00452  v0.00.04.016  Renamed HenoohMouse to MouseController
/// 2015-07-14  AS00489  v0.00.04.035  Modified the file name under remarks
/// 2015-07-22  AS00494  v0.00.04.040  Remove dependency on NativeMethods
/// 2015-10-25  AS00544  v1.00.00.000  Now available on nuget.org
/// 2015-10-28  AS00547  v1.00.00.001  Simplified the summary, modified the remarks and added paragraphing
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-04  AS00554  v1.00.00.005  Added sealed modifier to the class
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-09  AS00559  v1.00.01.001  Added example and modified the remarks section
/// 2015-11-10  AS00560  v1.00.01.002  Modified summary of all methods for better documentation
/// 2015-11-11  AS00561  v1.00.01.003  Fixed a bug that missed a semi-colon
/// 2016-02-28  AS00632  v1.00.03.006  Fix XML Comment warnings
/// 2016-03-08  AS00641  v1.00.03.010  Fixed few methods to follow Henooh Style Guidelines
/// 2016-03-28  AS00657  v1.00.03.012  Modified name of parameters to follow Henooh Style Guidelines
/// 2016-03-29  AS00658  v1.00.03.013  Modified example to follow Henooh Style Guidelines
/// 2016-04-02  AS00661  v1.00.03.014  Created a private method called SendMouseInput, a common input
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-04-07  AS00666  v1.00.03.016  Corrected some grammar in code example and method comments
/// 2016-08-19  AS00709  v1.00.03.021  Removed unused using statements
/// 2016-08-29  AS00716  v1.00.03.022  Derive from Abstract BaseController class
/// 2016-09-30  AS00735  v1.00.03.029  Use SendInput from SafeNativeMethods class
/// 2016-10-11  AS00746  v1.00.03.033  Add visibility xml tags, renamed class INPUT to Input
/// 2016-10-13  AS00748  v1.00.03.035  Modified comments inside the methods
/// 2016-10-15  AS00750  v1.00.03.037  Moved inputBuffer and inputList to BaseController
/// 2016-10-19  AS00754  v1.00.03.040  Corrected grammar on all comments
/// 2016-10-24  AS00759  v1.00.04.001  Fixed a bug that only sends LeftDown for SendMouseInput commands
/// 2016-10-29  AS00764  v1.00.04.002  Implement CancellationToken for MouseController
/// 2016-10-30  AS00765  v1.00.04.003  Implement CancellationToken on BaseController
/// 2016-11-02  AS00768  v1.00.04.005  Use Sleep method from Native instead of Thread.Sleep
/// 2016-11-09  AS00772  v1.00.00.032  Resolved XML comment for publicly visible type or member warning
/// 2016-11-20  AS00781  v1.00.04.010  Changed fields to properties, added access modifiers
/// 2016-11-22  AS00783  v1.00.04.011  Added comments to CancellationToken constructor
/// 2016-12-04  AS00795  v1.00.05.004  Modified XML comments for constructors
/// 2016-12-05  AS00796  v1.00.05.005  Implement Dpi Awareness methods
/// 2016-12-06  AS00797  v1.00.05.006  Remove code that is irrelevent, added implementation of PhysicalToLogical
/// 2016-12-09  AS00800  v1.00.05.007  Moved native class to SafeNativeMethods class, add XML header comments
/// 2016-12-10  AS00801  v1.00.05.008  Implement ShowDisplayInfo method, and added XML comments to added methods
/// 2016-12-17  AS00803  v1.00.05.009  Add LeftRightClickHold method
/// 2017-01-05  AS00808  v1.00.06.001  Add XButton, and click method now has an optional parameter
/// 2017-01-07  AS00810  v1.00.06.002  Add ButtonDown and ButtonUp methods
/// 2017-01-08  AS00811  v1.00.06.003  Made Down and Up methods obsolete, use new method with parameters instead
/// 2017-01-18  AS00818  v1.00.06.004  Click method rewritten to use MouseDown and MouseUp methods
/// 2017-01-21  AS00820  v1.00.06.005  Replace using obsolete methods to regular methods
/// 2017-01-26  AS00825  v1.00.06.006  Add XML comments to MouseDown and MouseUp methods
/// 2017-01-31  AS00829  v1.00.06.007  Add examples and visibility tags to some methods
/// 2017-08-09  AS00891  v1.00.06.015  Removed some methods from becoming obsolete, used ButtonDown methods
/// 2017-08-20  AS00897  v1.01.00.001  Resolve messages IDE0017 where Object initialization can be simplified
/// 2018-02-27  AS01011  v1.01.02.001  Delete obsolete methods, Added remarks for operations supported for Win 8.1
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2018-12-17  AS01131  v1.01.03.003  Add visibility tags to methods without them
/// 2019-01-25  AS01141  v1.01.03.004  Modify the summary of the XML comments for the constructors
/// 2019-03-27  AS01162  v1.01.03.010  Implement RunMode and suppress SendInput being sent
/// 2019-03-29  AS01164  v1.01.04.001  Resolve CA01026 by overriding click method
/// 2019-04-01  AS01166  v1.01.04.002  Resolve CA01026 by overriding ButtonDown and ButtonUp methods
/// 2019-04-18  AS01179  v1.01.04.006  Use discard for not using status from a native call
/// 2019-04-19  AS01180  v1.01.04.007  Resolve IDE0054 by using compound assignments
/// 2019-04-22  AS01182  v1.01.04.009  Replace System.Console with Trace
/// 2019-04-23  AS01183  v1.01.04.010  Add static prefix to few methods
/// 2019-04-24  AS01184  v1.01.04.011  Resolve CA1806 by assigning the result to a variable
/// 2019-05-02  AS01185  v1.01.04.012  Resolve IDE0059 by removing redundant assignments and values
/// 2019-08-06  AS01209  v1.01.04.013  Added two static methods that will return System.Windows.Points
/// 2019-08-08  AS01210  v1.01.04.014  Resolve IDE0059 by using Discard operation on AbsoluteMove method
/// 2019-11-15  AS01249  v1.01.05.001  Resolve CA1829 by using the Count property instead of Enumerable.Count
/// 2019-11-21  AS01251  v1.01.06.001  Resolve CA1829 by using the Count property instead of Enumerable.Count
/// 2019-11-27  AS01254  v1.01.06.002  Remove unused using statements
/// </revisionhistory>
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