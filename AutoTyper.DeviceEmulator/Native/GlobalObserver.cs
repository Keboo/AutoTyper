using System;
using System.Diagnostics;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides methods for subscription and unsubscription to global mouse and keyboard hooks.
/// </summary>
/// <remarks>
/// GlobalObserver class is a derived class from <see cref="T:AutoTyper.DeviceEmulator.Native.ObserverAbstract" /> which runs 
/// SetWindowsHookEx function.
/// </remarks>
/// <visibility>internal</visibility>
internal class GlobalObserver : ObserverAbstract
{
    /// <summary>
    /// Constant value that is used to install a hook procedure for a mouse in low level input events.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WH_MOUSE_LL = 14;

    /// <summary>
    /// Constant value that is used to install a hook procedure for a keyboard in low level input events.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WH_KEYBOARD_LL = 13;

    /// <summary>
    /// Getter for overridden IsGlobal property, set to true for a GlobalObserver.
    /// </summary>
    /// <visibility>internal</visibility>
    internal override bool IsGlobal => true;

    /// <summary>
    /// Observes a global hook by a hook identification and HookCallback.
    /// </summary>
    /// <param name="hookId"></param>
    /// <param name="hookCallback"></param>
    /// <returns></returns>
    /// <visibility>internal</visibility>
    internal override IntPtr Observe(int hookId, HookCallback hookCallback)
    {
        IntPtr intPtr = SafeNativeMethods.SetWindowsHookEx(hookId, hookCallback, Process.GetCurrentProcess().MainModule.BaseAddress, 0);
        if (intPtr == IntPtr.Zero)
        {
            ObserverAbstract.ThrowLastUnmanagedErrorAsException();
        }
        return intPtr;
    }
}