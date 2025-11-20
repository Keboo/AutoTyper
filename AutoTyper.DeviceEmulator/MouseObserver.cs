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
    /// suppress further processing of mouse movement in other applications.
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
    /// suppress further processing of mouse click in other applications.
    /// </remarks>
    [Obsolete("To suppress mouse clicks use MouseDownExt event instead.")]
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
    /// suppress further processing of mouse click in other applications.
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
            SetSuppressButtonUpFlag(e.Button);
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
        if (!HasSuppressButtonUpFlag(e.Button))
        {
            InvokeMouseEventHandler(this.MouseUp, e);
            return;
        }
        RemoveSuppressButtonUpFlag(e.Button);
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
    /// Method that removes SuppressButtonUpFlags.
    /// </summary>
    /// <param name="aButton"></param>
    /// <visibility>private</visibility>
    private void RemoveSuppressButtonUpFlag(MouseButtons aButton)
    {
        SuppressButtonUpFlags ^= aButton;
    }

    /// <summary>
    /// Method that returns SuppressButtonUpFlags.
    /// </summary>
    /// <param name="aButton"></param>
    /// <returns></returns>
    /// <visibility>private</visibility>
    private bool HasSuppressButtonUpFlag(MouseButtons aButton)
    {
        return (SuppressButtonUpFlags & aButton) != 0;
    }

    /// <summary>
    /// Method that sets SuppressButtonUpFlags.
    /// </summary>
    /// <param name="aButton"></param>
    /// <visibility>private</visibility>
    private void SetSuppressButtonUpFlag(MouseButtons aButton)
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