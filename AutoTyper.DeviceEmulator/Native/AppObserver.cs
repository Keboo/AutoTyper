using System;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides methods for subscription and unsubscription to application mouse and keyboard hooks.
/// </summary>
/// <remarks>
/// AppObserver class is a derived class from <see cref="T:AutoTyper.DeviceEmulator.Native.ObserverAbstract" /> which runs 
/// SetWindowsHookEx function.
/// </remarks>
/// <visibility>internal</visibility>
public class AppObserver : ObserverAbstract
{
    /// <summary>
    /// Constant value that is used to install a hook procedure for a mouse in applications.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WH_MOUSE = 7;

    /// <summary>
    /// Constant value that is used to install a hook procedure for a keyboard in applications.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WH_KEYBOARD = 2;

    /// <summary>
    /// Returns overridden IsGlobal property, set to false for an AppObserver.
    /// </summary>
    /// <visibility>internal</visibility>
    internal override bool IsGlobal => false;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.Native.AppObserver" /> with no arguments.
    /// </summary>
    /// <visibility>public</visibility>
    public AppObserver()
    {
    }

    /// <summary>
    /// Observes an application hook by a hook identification and HookCallback.
    /// </summary>
    /// <param name="aHookId"></param>
    /// <param name="aHookCallback"></param>
    /// <returns>Integer value of hookHandle from SetWindowHookEx method</returns>
    /// <visibility>internal</visibility>
    internal override IntPtr Observe(int aHookId, HookCallback aHookCallback)
    {
        IntPtr intPtr = SafeNativeMethods.SetWindowsHookEx(aHookId, aHookCallback, IntPtr.Zero, SafeNativeMethods.GetCurrentThreadId());
        if (intPtr == IntPtr.Zero)
        {
            ObserverAbstract.ThrowLastUnmanagedErrorAsException();
        }
        return intPtr;
    }
}