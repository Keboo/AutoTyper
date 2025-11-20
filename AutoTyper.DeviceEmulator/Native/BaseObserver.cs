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
    /// Provides a method to be overridden, which will dictate the logic of firing events.
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