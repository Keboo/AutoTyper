using System;
using System.Windows.Forms;
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
///  <revisionhistory>
///  YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
///  ==========  =======  ============  ============================================================================
///  2015-04-29  AS00447  v0.00.04.013  Initial Version
///  2015-05-07  AS00453  v0.00.04.017  Renamed the Class from KeyboardHookListener to KeyboardObserver
///  2015-05-11  AS00455  v0.00.04.019  Use renamed base class
///  2015-06-03  AS00465  v0.00.04.027  Implement using wpf events instead of WinForms events
///  2015-06-04  AS00466  v0.00.04.028  Implement changes to GlobalHooker to GlobalObserver
///  2015-06-08  AS00468  v0.00.04.030  Renamed Hooker class to ObserverAbstract
///  2015-06-09  AS00469  v0.00.04.031  Commented out code that could use System.Windows.Input to process inputs
///  2015-07-08  AS00486  v0.00.04.034  Modified the summary and remarks to contain correct information
///  2015-07-23  AS00495  v0.00.04.041  Use renamed class AppObserver
///  2015-10-15  AS00538  v0.00.04.053  Hide WpfKeyDown and WpfKeyUp as they are not implemented
///  2015-10-25  AS00544  v1.00.00.000  Now available on nuget.org
///  2015-10-28  AS00547  v1.00.00.001  Simplified the summary
///  2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
///  2015-11-04  AS00554  v1.00.00.005  Added sealed modifier to the class
///  2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
///  2015-11-07  AS00556  v1.00.00.007  Renamed methods, modified the summary
///  2015-11-09  AS00559  v1.00.01.001  Modify documentation
///  2015-11-10  AS00560  v1.00.01.002  Added comments and made wpf versions obsolete
///  2015-11-17  AS00566  v1.00.01.007  Added default constructor that uses GlobalObserver
///  2015-11-18  AS00568  v1.00.01.008  Comments on a default constructor and modified comments
///  2016-01-16  AS00601  v1.00.03.000  Fixed a bug that threw null exception
///  2016-02-27  AS00631  v1.00.03.005  Fixed XML comments on KeyPress event
///  2016-03-29  AS00658  v1.00.03.013  Added an example code to KeyboardObserver class
///  2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
///  2016-08-19  AS00709  v1.00.03.021  Removed unused using statements
///  2016-09-21  AS00726  v1.00.03.026  Added obsolete attribute to Dispose method
///  2016-09-24  AS00729  v1.00.03.028  Resolve XML comment warnings that point to older class name
///  2016-10-02  AS00737  v1.00.03.031  Replace int with IntPtr for SafeNativeMethod calls
///  2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
///  2016-10-11  AS00746  v1.00.03.033  Added visibility xml tags, rewrite Dispose method
///  2016-10-13  AS00748  v1.00.03.035  Modified example code in the header
///  2016-11-20  AS00781  v1.00.04.010  Follow Henooh Style Guidelines to change properties to Camel casing
///  2016-12-03  AS00794  v1.00.05.003  Summary of default constructor changed with proper see tags
///  2016-12-04  AS00795  v1.00.05.004  Corrected style under KeyboardObserver constructor, added header XML comment
///  2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
///  2019-01-25  AS01141  v1.01.03.004  Modify the summary of the XML comments for the constructors
///  2019-03-27  AS01162  v1.01.03.010  Removed method calling KeyEvent Wpf Extensions
///  2019-10-24  AS01242  v1.01.04.018  Add visibility tag on Dispose method
///  2019-10-30  AS01244  v1.01.04.019  Resolve IDE0052 by allowing an internal getter for KeyPressEventArgsExt
///  2019-11-27  AS01254  v1.01.06.002  Remove unused using statements
///  2020-02-22  AS01286  v1.01.06.003  Set the maximum character per line to be at 120 characters
///  </revisionhistory>
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
