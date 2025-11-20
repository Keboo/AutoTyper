#define TRACE
using System;
using System.Diagnostics;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides a set of common features between application observers and global observers.
/// It provides base methods to subscribe and unsubscribe to hooks.
/// Common processing, error handling and cleanup logic.
/// </summary>
/// <remarks>
/// BaseObserver has components that is shared by AppObserver and GlobalObserver.
/// </remarks>
/// <visibility>public</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-28  AS00446  v0.00.04.012  Initial Version
/// 2015-05-11  AS00455  v0.00.04.019  Renamed class to BaseObserver
/// 2015-06-08  AS00468  v0.00.04.030  Renamed Hooker class to ObserverAbstract
/// 2015-07-17  AS00491  v0.00.04.037  Corrected file name under remarks and made edits to summary
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-09  AS00559  v1.00.01.001  Renamed Subscribe method to Observe Method, m_Hooker to observer
/// 2015-11-11  AS00661  v1.00.01.003  Modified comments for GetHookId method
/// 2015-11-12  AS00562  v1.00.01.004  Use the new HookCallBack from ObserverAbstract namespace
/// 2015-11-16  AS00565  v1.00.01.006  Removed CallNextHook method and return directly from MonitorActivity method
/// 2015-11-17  AS00566  v1.00.01.007  Added default constructor that uses GlobalObserver
/// 2015-11-18  AS00568  v1.00.01.008  Added comments to new Default constructor, modified remarks
/// 2016-02-06  AS00619  v1.00.03.001  Added comment for IsGlobal method
/// 2016-02-27  AS00631  v1.00.03.005  Fixed the name of parameter for MonitorActivity method
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-09-17  AS00722  v1.00.03.023  Properly handle IDisposable
/// 2016-09-22  AS00727  v1.00.03.027  Use moved NativeMethods
/// 2016-09-24  AS00729  v1.00.03.028  Renamed NativeMethods class to SafeNativeMethods class
/// 2016-10-02  AS00737  v1.00.00.031  Replace int with IntPtr for SafeNativeMethod calls
/// 2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
/// 2016-10-11  AS00746  v1.00.03.033  Correctly implement IDisposable
/// 2016-10-12  AS00747  v1.00.03.034  Changed observer field to Observer property
/// 2016-10-13  AS00748  v1.00.03.035  Added comment to properties
/// 2016-10-18  AS00753  v1.00.03.039  Added visibilty xml tags, added xml comment to Dispose(bool)
/// 2016-10-19  AS00754  v1.00.03.040  Modified summary, comments throughout the code
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2017-02-21  AS00839  v1.00.06.010  Change the comment to be more clear on Dispose method
/// 2017-10-15  AS00936  v1.01.01.003  Resolved IDE0016 message, null check can be simplified
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-01-30  AS01143  v1.01.03.005  Modify the layout of the code, add XML header comments to all properties
/// 2019-03-25  AS01160  v1.01.03.008  Use the static StopObserve method from ObserverAbstract
/// 2019-03-27  AS01162  v1.01.03.010  Resolve CA1026 by adding constructor that provides all default arguments
/// 2019-04-24  AS01184  v1.01.04.011  Resolve CA1806 by assigning the result to a variable
/// 2019-05-02  AS01185  v1.01.04.012  Follow Henooh Coding Standards to have the code within 120 characters
/// 2019-08-08  AS01210  v1.01.04.014  Corrected spelling for the message that was thrown during an exception
/// 2019-08-12  AS01214  v1.01.04.016  Use the resource table to retrive string message for an exception
/// </revisionhistory>
public abstract class BaseObserver : IDisposable
{
    /// <summary>
    /// Provides instance of Observer to BaseObserver.
    /// </summary>
    /// <visibility>private</visibility>
    private ObserverAbstract Observer { get; set; }

    /// <summary>
    /// Stores the handle to the Keyboard or Mouse hook procedure.
    /// </summary>
    /// <visibility>protected</visibility>
    protected IntPtr HookHandle { get; set; }

    /// <summary>
    /// Keeps the reference to prevent garbage collection of delegate. See: CallbackOnCollectedDelegate http://msdn.microsoft.com/en-us/library/43yky316(v=VS.100).aspx
    /// </summary>
    /// <visibility>internal</visibility>
    internal ObserverAbstract.HookCallback HookCallbackReferenceKeeper { get; set; }

    /// <summary>
    /// Determine the value of IsGlobal based on value of observer.
    /// </summary>
    /// <visibility>internal</visibility>
    internal bool IsGlobal => Observer.IsGlobal;

    /// <summary>
    /// Gets or Sets the enabled status of the Hook.
    /// </summary>
    /// <remarks>
    /// True - The Hook is presently installed, activated, and will fire events.
    /// False - The Hook is not part of the hook chain, and will not fire events.
    /// </remarks>
    /// <visibility>public</visibility>
    public bool Enabled
    {
        get
        {
            return HookHandle != IntPtr.Zero;
        }
        set
        {
            if (value)
            {
                if (!Enabled)
                {
                    Start();
                }
            }
            else if (Enabled)
            {
                Stop();
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.Native.BaseObserver" /> with no arguments.
    /// </summary>
    /// <remarks>
    /// Hooks are not active after instantiation. You need to use either <see cref="P:AutoTyper.DeviceEmulator.Native.BaseObserver.Enabled" /> 
    /// property or call <see cref="M:AutoTyper.DeviceEmulator.Native.BaseObserver.Start" /> method.
    /// </remarks>
    /// <visibility>protected</visibility>
    protected BaseObserver()
    {
        Observer = new GlobalObserver();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.Native.BaseObserver" /> with <see cref="T:AutoTyper.DeviceEmulator.Native.ObserverAbstract" /> as a parameter.
    /// </summary>
    /// <param name="aObserver">
    /// Depending on this parameter the listener hooks either application or global observer events.
    /// </param>
    /// <remarks>
    /// Hooks are not active after instantiation. You need to use either <see cref="P:AutoTyper.DeviceEmulator.Native.BaseObserver.Enabled" />
    /// property or call <see cref="M:AutoTyper.DeviceEmulator.Native.BaseObserver.Start" /> method.
    /// </remarks>
    /// <visibility>protected</visibility>
    protected BaseObserver(ObserverAbstract aObserver)
    {
        Observer = aObserver ?? new GlobalObserver();
    }

    /// <summary>
    /// Provides a  method to be overriddden, which will dicate the logic of firing events.
    /// </summary>
    /// <visibility>protected</visibility>
    protected abstract bool ProcessCallback(IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// A callback function which will be called every time a keyboard or mouse activity detected.
    /// </summary>
    /// <visibility>protected</visibility>
    protected IntPtr MonitorActivity(int aCode, IntPtr wParam, IntPtr lParam)
    {
        if (aCode == 0 && !ProcessCallback(wParam, lParam))
        {
            return IntPtr.Zero;
        }
        return SafeNativeMethods.CallNextHookEx(HookHandle, aCode, wParam, lParam);
    }

    /// <summary>
    /// Subscribes to the hook and starts firing events.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException"></exception>
    /// <visibility>public</visibility>
    public void Start()
    {
        if (Enabled)
        {
            Trace.WriteLine("Hook listener is already started. Call Stop() method first or use Enabled property.");
            InvalidOperationException ex = new InvalidOperationException("Hook listener is already started. Call Stop() method first or use Enabled property.");
            throw ex;
        }
        HookCallbackReferenceKeeper = MonitorActivity;
        try
        {
            HookHandle = Observer.Observe(GetHookId(), HookCallbackReferenceKeeper);
        }
        catch (Exception)
        {
            HookCallbackReferenceKeeper = null;
            HookHandle = IntPtr.Zero;
            throw;
        }
    }

    /// <summary>
    /// Unsubscribes from the hook and stops firing events.
    /// </summary>
    /// <exception cref="T:System.ComponentModel.Win32Exception"></exception>
    /// <visibility>public</visibility>
    public void Stop()
    {
        try
        {
            ObserverAbstract.StopObserve(HookHandle);
        }
        finally
        {
            HookCallbackReferenceKeeper = null;
            HookHandle = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Enables you to switch from application hooks to global hooks and vice versa on the fly
    /// without unsubscribing from events. Component remains enabled or disabled state after this call as it was before.
    /// </summary>
    /// <param name="aObserver">An AppObserver or GlobalObserver object.</param>
    /// <visibility>public</visibility>
    public void Replace(ObserverAbstract aObserver)
    {
        bool enabled = Enabled;
        Enabled = false;
        Observer = aObserver;
        Enabled = enabled;
    }

    /// <summary>
    /// Override to deliver correct id to be used for <see cref="M:AutoTyper.DeviceEmulator.Native.SafeNativeMethods.SetWindowsHookEx(System.Int32,AutoTyper.DeviceEmulator.Native.ObserverAbstract.HookCallback,System.IntPtr,System.Int32)" /> call.
    /// </summary>
    /// <returns></returns>
    /// <visibility>protected</visibility>
    protected abstract int GetHookId();

    /// <summary>
    /// Release delegates, unsubscribes from hooks.
    /// </summary>
    /// <visibility>public</visibility>
    public void Dispose()
    {
        Dispose(aDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Unsubscribes from global hooks skiping error handling.
    /// </summary>
    ~BaseObserver()
    {
        Dispose(aDisposing: false);
    }

    /// <summary>
    /// Releases delegates, unsubscribe from hooks.
    /// </summary>
    /// <param name="aDisposing"></param>
    /// <visibility>protected</visibility>
    protected virtual void Dispose(bool aDisposing)
    {
        if (aDisposing)
        {
            Stop();
        }
        if (HookHandle != IntPtr.Zero)
        {
            SafeNativeMethods.UnhookWindowsHookEx(HookHandle);
        }
    }
}