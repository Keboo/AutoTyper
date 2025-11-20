using System;
using System.Drawing;

using AutoTyper.DeviceEmulator.Native;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// Provides a mechanism that will observe certain mouse activities and return data to appropriate events.
/// </summary>
/// <remarks>
/// MouseObserver allows observing Mouse device with Mouse related events.
/// </remarks>
/// <example>
/// Mouse Observer Usage
/// <code>
/// // Use Mouse Observer to detect Global Mouse Inputs.
/// public void ObserveMouse()
/// {
///     AutoTyper.DeviceEmulator.MouseObserver mouseObserver = new AutoTyper.DeviceEmulator.MouseObserver();
///     mouseObserver.Enable = true;
///     // Bind a MouseMove event with a method
///     mouseObserver.MouseMove += MouseObserver_MouseMove;
/// }
///
/// // Print that the mouse has moved.
/// public void MouseObserver_MouseMove(object sender, EventArgs e)
/// {
///     System.Console.WriteLine("Mouse movement detected");
/// }
/// </code>
/// </example>
/// <visibility>public</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-30  AS00448  v0.00.04.014  Initial Version
/// 2015-05-07  AS00453  v0.00.04.017  Renamed the Class from MouseHookListener to MouseObserver
/// 2015-05-11  AS00455  v0.00.04.019  Use renamed base class
/// 2015-05-12  AS00456  v0.00.04.020  Use Point struct from System.Drawing.Point
/// 2015-06-04  AS00466  v0.00.04.028  Implement changes to GlobalHooker to GlobalObserver
/// 2015-06-08  AS00468  v0.00.04.030  Renamed Hooker class to ObserverAbstract
/// 2015-07-16  AS00490  v0.00.04.036  Modified comments under remarks and method
/// 2015-07-21  AS00493  v0.00.04.039  Moved InteropService for GetDoubleClickTime to local class
/// 2015-07-23  AS00495  v0.00.04.041  Use renamed class AppObserver
/// 2015-10-25  AS00544  v1.00.00.000  Now available on nuget.org
/// 2015-10-28  AS00547  v1.00.00.001  Simplified the summary
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-04  AS00554  v1.00.00.005  Added sealed modifier to the class and changed modifiers for some methods
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-07  AS00556  v1.00.00.007  Renamed a parameter and modified some comments
/// 2015-11-18  AS00568  v1.00.01.008  Added a a default constructor and modified comments
/// 2015-11-23  AS00573  v1.00.01.010  Use modified version of MouseEventExtArgs without Point public property
/// 2016-02-07  AS00620  v1.00.03.002  Modified comments on methods and remarks
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-07-11  AS00692  v1.00.03.017  Added example code for MouseObserver
/// 2016-07-13  AS00693  v1.00.03.018  Modified revision history to correct previous typo
/// 2016-08-19  AS00709  v1.00.03.021  Removed unused using statements
/// 2016-09-24  AS00729  v1.00.03.028  Moved P/Inovke method to SafeNativeMethods class
/// 2016-10-02  AS00737  v1.00.00.031  Replace int with IntPtr for SafeNativeMethod calls
/// 2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
/// 2016-10-11  AS00746  v1.00.03.033  Rewrite Dispose method
/// 2016-10-13  AS00748  v1.00.03.035  Modified example of class to show renamed class
/// 2016-10-15  AS00750  v1.00.03.037  Changed fields to properties, renamed properties, added xml comment headers
/// 2016-10-18  AS00753  v1.00.03.039  Added visibility xml tags
/// 2016-10-19  AS00754  v1.00.03.040  Corrected xml comment under constructor
/// 2016-11-15  AS00777  v1.00.04.009  Added comments to properties
/// 2016-12-03  AS00794  v1.00.05.003  Summary of default constructor changed with proper see tags
/// 2016-12-05  AS00796  v1.00.05.005  Follow Henooh Style Guidelines and fit in 120 columns
/// 2017-02-12  AS00833  v1.00.06.008  Follow Henooh Style Guidelines with parameter names with a prefix
/// 2017-08-20  AS00897  v1.01.00.001  Resolve messages IDE1005 where Delegate invocation can be simplified
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-01-25  AS01141  v1.01.03.004  Modify the summary of the XML comments for the constructors
/// 2019-04-18  AS01179  v1.01.04.006  Resolve IDE0054 by using compound assignment
/// </revisionhistory>
public sealed class MouseObserver : BaseObserver
{
    /// <summary>
    /// Used to determine if the mouse has moved or not.
    /// </summary>
    /// <visibility>private</visibility>
    private Point PreviousPosition { get; set; }

    /// <summary>
    /// Used to determine what type of click it is to process.
    /// </summary>
    /// <visibility>private</visibility>
    private int PreviousClickedTime { get; set; }

    /// <summary>
    /// Used to determine if the mouse was previously clicked.
    /// </summary>
    /// <visibility>private</visibility>
    private MouseButtons PreviousClicked { get; set; }

    /// <summary>
    /// Used to determine the state of mouse button presses.
    /// </summary>
    /// <visibility>private</visibility>
    private MouseButtons DownButtonsWaitingForMouseUp { get; set; }

    /// <summary>
    /// Used to suppress button up flags.
    /// </summary>
    /// <visibility>private</visibility>
    private MouseButtons SuppressButtonUpFlags { get; set; }

    /// <summary>
    /// Used to determine the double click type.
    /// </summary>
    /// <visibility>private</visibility>
    private int SystemDoubleClickTime { get; set; }

    /// <summary>
    /// Occurs when the mouse pointer is moved.
    /// </summary>
    /// <visibility>public</visibility>
    public event MouseEventHandler MouseMove;

    /// <summary>
    /// Occurs when the mouse pointer is moved.
    /// </summary>
    /// <remarks>
    /// This event provides extended arguments of type <see cref="T:System.Windows.Forms.MouseEventArgs" /> enabling you to 
    /// supress further processing of mouse movement in other applications.
    /// </remarks>
    /// <visibility>internal</visibility>
    internal event EventHandler<MouseEventExtArgs> MouseMoveExt;

    /// <summary>
    /// Occurs when a click was performed by the mouse.
    /// </summary>
    /// <visibility>public</visibility>
    public event MouseEventHandler MouseClick;

    /// <summary>
    /// Occurs when a click was performed by the mouse.
    /// </summary>
    /// <remarks>
    /// This event provides extended arguments of type <see cref="T:System.Windows.Forms.MouseEventArgs" /> enabling you to 
    /// supress further processing of mouse click in other applications.
    /// </remarks>
    [Obsolete("To supress mouse clicks use MouseDownExt event instead.")]
    internal event EventHandler<MouseEventExtArgs> MouseClickExt;

    /// <summary>
    /// Occurs when the mouse a mouse button is pressed.
    /// </summary>
    /// <visibility>public</visibility>
    public event MouseEventHandler MouseDown;

    /// <summary>
    /// Occurs when the mouse a mouse button is pressed.
    /// </summary>
    /// <remarks>
    /// This event provides extended arguments of type <see cref="T:System.Windows.Forms.MouseEventArgs" /> enabling you to 
    /// supress further processing of mouse click in other applications.
    /// </remarks>
    /// <visibility>internal</visibility>
    internal event EventHandler<MouseEventExtArgs> MouseDownExt;

    /// <summary>
    /// Occurs when a mouse button is released.
    /// </summary>
    /// <visibility>public</visibility>
    public event MouseEventHandler MouseUp;

    /// <summary>
    /// Occurs when the mouse wheel moves.
    /// </summary>
    /// <visibility>public</visibility>
    public event MouseEventHandler MouseWheel;

    /// <summary>
    /// Occurs when a mouse button is double-clicked.
    /// </summary>
    /// <visibility>public</visibility>
    public event MouseEventHandler MouseDoubleClick;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.MouseObserver" /> derived from <see cref="T:AutoTyper.DeviceEmulator.Native.BaseObserver" />.
    /// </summary>
    /// <visibility>public</visibility>
    public MouseObserver()
    {
        PreviousPosition = new Point(-1, -1);
        PreviousClickedTime = 0;
        DownButtonsWaitingForMouseUp = MouseButtons.None;
        SuppressButtonUpFlags = MouseButtons.None;
        PreviousClicked = MouseButtons.None;
        SystemDoubleClickTime = SafeNativeMethods.GetDoubleClickTime();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.MouseObserver" /> with <see cref="T:AutoTyper.DeviceEmulator.Native.ObserverAbstract" />
    /// as a parameter.
    /// </summary>
    /// <param name="aObserver">
    /// Depending on this parameter, the listener hooks either application or global mouse events.
    /// </param>
    /// <remarks>
    /// Hooks are not active after installation. You need to use either <see cref="P:AutoTyper.DeviceEmulator.Native.BaseObserver.Enabled" /> property
    /// or call <see cref="M:AutoTyper.DeviceEmulator.Native.BaseObserver.Start" /> method.
    /// </remarks>
    /// <visibility>public</visibility>
    public MouseObserver(ObserverAbstract aObserver)
        : base(aObserver)
    {
        PreviousPosition = new Point(-1, -1);
        PreviousClickedTime = 0;
        DownButtonsWaitingForMouseUp = MouseButtons.None;
        SuppressButtonUpFlags = MouseButtons.None;
        PreviousClicked = MouseButtons.None;
        SystemDoubleClickTime = SafeNativeMethods.GetDoubleClickTime();
    }

    /// <summary>
    /// This method processes the data from the hook and initiates event firing.
    /// </summary>
    /// <param name="wParam">The first Windows Messages parameter.</param>
    /// <param name="lParam">The second Windows Messages parameter.</param>
    /// <returns>
    /// True - The hook will be passed along to other applications.
    /// False - The hook will not be given to other applications, effectively blocking input.
    /// </returns>
    /// <visibility>protected</visibility>
    protected override bool ProcessCallback(IntPtr wParam, IntPtr lParam)
    {
        MouseEventExtArgs e = MouseEventExtArgs.FromRawData(wParam, lParam, base.IsGlobal);
        if (e.IsMouseKeyDown)
        {
            ProcessMouseDown(ref e);
        }
        if (e.Clicks == 1 && e.IsMouseKeyUp && !e.Handled)
        {
            ProcessMouseClick(ref e);
        }
        if (e.Clicks == 2 && !e.Handled)
        {
            InvokeMouseEventHandler(this.MouseDoubleClick, e);
        }
        if (e.IsMouseKeyUp)
        {
            ProcessMouseUp(ref e);
        }
        if (e.WheelScrolled)
        {
            InvokeMouseEventHandler(this.MouseWheel, e);
        }
        if (HasMoved(new Point(e.X, e.Y)))
        {
            ProcessMouseMove(ref e);
        }
        return !e.Handled;
    }

    /// <summary>
    /// Method called when MouseDown flag is enabled.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void ProcessMouseDown(ref MouseEventExtArgs e)
    {
        if (base.IsGlobal)
        {
            ProcessPossibleDoubleClick(ref e);
        }
        else
        {
            DownButtonsWaitingForMouseUp = MouseButtons.None;
            PreviousClicked = MouseButtons.None;
            PreviousClickedTime = 0;
        }
        InvokeMouseEventHandler(this.MouseDown, e);
        InvokeMouseEventHandlerExt(this.MouseDownExt, e);
        if (e.Handled)
        {
            SetSupressButtonUpFlag(e.Button);
            e.Handled = true;
        }
    }

    /// <summary>
    /// Method called when DoubleClick event is enabled.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void ProcessPossibleDoubleClick(ref MouseEventExtArgs e)
    {
        if (IsDoubleClick(e.Button, e.Timestamp))
        {
            e = e.ToDoubleClickEventArgs();
            DownButtonsWaitingForMouseUp = MouseButtons.None;
            PreviousClicked = MouseButtons.None;
            PreviousClickedTime = 0;
        }
        else
        {
            DownButtonsWaitingForMouseUp |= e.Button;
            PreviousClickedTime = e.Timestamp;
        }
    }

    /// <summary>
    /// Method called when MouseClick is enabled.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void ProcessMouseClick(ref MouseEventExtArgs e)
    {
        if ((DownButtonsWaitingForMouseUp & e.Button) != MouseButtons.None)
        {
            PreviousClicked = e.Button;
            DownButtonsWaitingForMouseUp = MouseButtons.None;
            InvokeMouseEventHandler(this.MouseClick, e);
            InvokeMouseEventHandlerExt(this.MouseClickExt, e);
        }
    }

    /// <summary>
    /// Method called when MouseUp event is called.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void ProcessMouseUp(ref MouseEventExtArgs e)
    {
        if (!HasSupressButtonUpFlag(e.Button))
        {
            InvokeMouseEventHandler(this.MouseUp, e);
            return;
        }
        RemoveSupressButtonUpFlag(e.Button);
        e.Handled = true;
    }

    /// <summary>
    /// Method called when Mouse is Moved.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void ProcessMouseMove(ref MouseEventExtArgs e)
    {
        PreviousPosition = new Point(e.X, e.Y);
        InvokeMouseEventHandler(this.MouseMove, e);
        InvokeMouseEventHandlerExt(this.MouseMoveExt, e);
    }

    /// <summary>
    /// Method that remove SupressButtonUpFlags.
    /// </summary>
    /// <param name="aButton"></param>
    /// <visibility>private</visibility>
    private void RemoveSupressButtonUpFlag(MouseButtons aButton)
    {
        SuppressButtonUpFlags ^= aButton;
    }

    /// <summary>
    /// Method that returns SupressButtonUpFlags.
    /// </summary>
    /// <param name="aButton"></param>
    /// <returns></returns>
    /// <visibility>private</visibility>
    private bool HasSupressButtonUpFlag(MouseButtons aButton)
    {
        return (SuppressButtonUpFlags & aButton) != 0;
    }

    /// <summary>
    /// Method that sets SupressButtonUpFlags.
    /// </summary>
    /// <param name="aButton"></param>
    /// <visibility>private</visibility>
    private void SetSupressButtonUpFlag(MouseButtons aButton)
    {
        SuppressButtonUpFlags |= aButton;
    }

    /// <summary>
    /// Returns the correct hook id to be used for <see cref="M:AutoTyper.DeviceEmulator.Native.SafeNativeMethods.SetWindowsHookEx(System.Int32,AutoTyper.DeviceEmulator.Native.ObserverAbstract.HookCallback,System.IntPtr,System.Int32)" /> call.
    /// </summary>
    /// <returns>WH_MOUSE (0x07) or WH_MOUSE_LL (0x14) constant.</returns>
    /// <visibility>protected</visibility>
    protected override int GetHookId()
    {
        return base.IsGlobal ? 14 : 7;
    }

    /// <summary>
    /// Method that returns when mouse has moved.
    /// </summary>
    /// <param name="aActualPoint"></param>
    /// <returns></returns>
    /// <visibility>private</visibility>
    private bool HasMoved(Point aActualPoint)
    {
        return PreviousPosition != aActualPoint;
    }

    /// <summary>
    /// Method that sets when mouse had double clicked.
    /// </summary>
    /// <param name="aButton"></param>
    /// <param name="aTimeStamp"></param>
    /// <returns></returns>
    /// <visibility>private</visibility>
    private bool IsDoubleClick(MouseButtons aButton, int aTimeStamp)
    {
        return aButton == PreviousClicked && aTimeStamp - PreviousClickedTime <= SystemDoubleClickTime;
    }

    /// <summary>
    /// Invoker for Mouse Event Handler.
    /// </summary>
    /// <param name="aEventHandler"></param>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void InvokeMouseEventHandler(MouseEventHandler aEventHandler, MouseEventArgs e)
    {
        aEventHandler?.Invoke(this, e);
    }

    /// <summary>
    /// Invoker for Mouse Event Handler Extention.
    /// </summary>
    /// <param name="aEventHandler"></param>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void InvokeMouseEventHandlerExt(EventHandler<MouseEventExtArgs> aEventHandler, MouseEventExtArgs e)
    {
        aEventHandler?.Invoke(this, e);
    }

    /// <summary>
    /// Release delegates, unsubscribes from hooks.
    /// </summary>
    /// <param name="aDisposing"></param>
    /// <visibility>protected</visibility>
    protected override void Dispose(bool aDisposing)
    {
        this.MouseClick = null;
        this.MouseClickExt = null;
        this.MouseDown = null;
        this.MouseDownExt = null;
        this.MouseMove = null;
        this.MouseMoveExt = null;
        this.MouseUp = null;
        this.MouseWheel = null;
        this.MouseDoubleClick = null;
        base.Dispose(aDisposing: true);
    }
}