using System;

using AutoTyper.DeviceEmulator.Native;

namespace AutoTyper.DeviceEmulator;

/// <summary>
///  Provides a mechanism that will observe certain keyboard activities and return data to appropriate events.
///  </summary>
///  <remarks>
///  KeyboardObserver allows observing Keyboard device with KeyUp and KeyDown events.
///  </remarks>
///  <example>
///  Keyboard Observer Usage.
///  <code>
///  // Use Keyboard Observer to detect Global Keyboard Inputs.
///  public void ObserveKeyboard()
///  {
///      // Initialize KeyboardObserver.
///      HenoohDeviceEmulator.KeyboardObserver kbObserver = new HenoohDeviceEmulator.KeyboardObserver();
///      kbObserver.Enable = true;
///      // Bind a KeyDown event with a method
///      kbObserver.KeyDown += KeyboardObserver_KeyDown;
///  }
///
///  // Print the key that was pressed
///  public void KeyboardObserver_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
///  {
///      System.Console.WriteLine("KeyDown detected on " + e.KeyCode);
///  }
///  </code>
///  </example>
///  <visibility>public</visibility>
public sealed class KeyboardObserver : BaseObserver
{
    /// <summary>
    /// Provides accessor to KeyPressEventArgsExt.
    /// </summary>
    internal KeyPressEventArgsExt KeyPressEventArgsExt { get; private set; }

    /// <summary>
    /// Occurs when a key is pressed. 
    /// </summary>
    /// <visibility>public</visibility>
    public event KeyEventHandler KeyDown;

    /// <summary>
    /// Occurs when a key is pressed.
    /// </summary>
    /// <remarks>
    /// Key events occur in the following order: 
    /// <ol>
    /// <li>KeyDown</li>
    /// <li>KeyPress</li>
    /// <li>KeyUp</li>
    /// </ol>
    /// <para>
    /// The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown 
    /// and KeyUp events. Use the KeyChar property to sample keystrokes at run time and to consume or modify a
    /// subset of common keystrokes. To handle keyboard events only in your application and not enable other
    /// applications to receive keyboard events, set the <see cref="P:System.Windows.Forms.KeyPressEventArgs.Handled" /> property in your
    /// form's KeyPress event-handling method to <b>true</b>. 
    /// </para>
    /// </remarks>
    /// <visibility>public</visibility>
    public event KeyPressEventHandler KeyPress;

    /// <summary>
    /// Occurs when a key is released.
    /// </summary>
    /// <visibility>public</visibility>
    public event KeyEventHandler KeyUp;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.KeyboardObserver" /> derived from <see cref="T:AutoTyper.DeviceEmulator.Native.BaseObserver" />.
    /// </summary>
    /// <visibility>public</visibility>
    public KeyboardObserver()
    {
        KeyPressEventArgsExt = new KeyPressEventArgsExt(' ');
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.KeyboardObserver" /> with <see cref="T:AutoTyper.DeviceEmulator.Native.ObserverAbstract" />
    /// as a paramter.
    /// </summary>
    /// <param name="aObserver">
    /// Depending on this parameter the listener hooks either application or global keyboard events.
    /// </param>
    /// <remarks>
    /// Hooks are not active after instantiation. You need to use either <see cref="P:AutoTyper.DeviceEmulator.Native.BaseObserver.Enabled" /> 
    /// property or call <see cref="M:AutoTyper.DeviceEmulator.Native.BaseObserver.Start" /> method.
    /// </remarks>
    /// <visibility>public</visibility>
    public KeyboardObserver(ObserverAbstract aObserver)
        : base(aObserver)
    {
        KeyPressEventArgsExt = new KeyPressEventArgsExt(' ');
    }

    /// <summary>
    /// This method processes the data from the hook and initiates event firing.
    /// </summary>
    /// <param name="wParam">The first Windows Messages parameter.</param>
    /// <param name="lParam">The second Windows Messages parameter.</param>
    /// <returns>
    /// True - The hook will be passed along to other applications.
    /// <para>
    /// False - The hook will not be given to other applications, effectively blocking input.
    /// </para>
    /// </returns>
    /// <visibility>protected</visibility>
    protected override bool ProcessCallback(IntPtr wParam, IntPtr lParam)
    {
        KeyEventArgsExt keyEventArgsExt = KeyEventArgsExt.FromRawData(wParam, lParam, base.IsGlobal);
        InvokeKeyDown(keyEventArgsExt);
        InvokeKeyPress(wParam, lParam);
        InvokeKeyUp(keyEventArgsExt);
        return !keyEventArgsExt.Handled;
    }

    /// <summary>
    /// Returns the correct hook id to be used for <see cref="M:AutoTyper.DeviceEmulator.Native.SafeNativeMethods.SetWindowsHookEx(System.Int32,AutoTyper.DeviceEmulator.Native.ObserverAbstract.HookCallback,System.IntPtr,System.Int32)" /> call.
    /// </summary>
    /// <returns>WH_KEYBOARD (0x02) or WH_KEYBOARD_LL (0x13) constant.</returns>
    /// <visibility>protected</visibility>
    protected override int GetHookId()
    {
        return base.IsGlobal ? 13 : 2;
    }

    /// <summary>
    /// Windows Form version of InvokeKeyDown.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void InvokeKeyDown(KeyEventArgsExt e)
    {
        KeyEventHandler keyEventHandler = this.KeyDown;
        if (keyEventHandler != null && !e.Handled && e.IsKeyDown)
        {
            keyEventHandler(this, e);
        }
    }

    /// <summary>
    /// Windows Form version of InvokeKeyPress.
    /// </summary>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <visibility>private</visibility>
    private void InvokeKeyPress(IntPtr wParam, IntPtr lParam)
    {
        InvokeKeyPress(KeyPressEventArgsExt.FromRawData(wParam, lParam, base.IsGlobal));
    }

    /// <summary>
    /// Winforms version of InvokeKeyPress.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void InvokeKeyPress(KeyPressEventArgsExt e)
    {
        KeyPressEventHandler keyPressEventHandler = this.KeyPress;
        if (keyPressEventHandler != null && !e.Handled && !e.IsNonChar)
        {
            keyPressEventHandler(this, e);
        }
    }

    /// <summary>
    /// Winforms version of InvokeKeyUp.
    /// </summary>
    /// <param name="e"></param>
    /// <visibility>private</visibility>
    private void InvokeKeyUp(KeyEventArgsExt e)
    {
        KeyEventHandler keyEventHandler = this.KeyUp;
        if (keyEventHandler != null && !e.Handled && e.IsKeyUp)
        {
            keyEventHandler(this, e);
        }
    }

    /// <summary>
    /// Release delegates, Stop Observe, and dispose.
    /// </summary>
    /// <param name="aDisposing"></param>
    /// <visibility>protected</visibility>
    protected override void Dispose(bool aDisposing)
    {
        this.KeyPress = null;
        this.KeyDown = null;
        this.KeyUp = null;
        base.Dispose(aDisposing);
    }
}