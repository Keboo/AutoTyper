using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides abstract of methods used by Observer classes. Provides base implementation of methods for 
/// subscription and unsubscription to application and/or global mouse and keyboard hooks.
/// </summary>
/// <remarks>
/// ObserverAbstract uses combination of SetWindowHookEx to raise events.
/// </remarks>
/// <visibility>internal</visibility>
public abstract class ObserverAbstract
{
    /// <summary>
    /// The CallWndProc hook procedure is an application-defined or library-defined callback method used with
    /// the SetWindowsHookEx method. The HOOKPROC type defines a pointer to this callback method. CallWndProc
    /// is a placeholder for the application-defined or library defined method name.
    /// </summary>
    /// <remarks>
    /// HookCallback delegate is now part of ObserverAbstract.
    /// </remarks>
    /// <param name="nCode">
    /// Specifies whether the hook procedure must process the message.
    /// If nCode is HC_ACTION, the hook procedure must process the message.
    /// If nCode is less than zero, the hook procedure must pass the message to the
    /// CallNextHookEx method without further processing and must return the value returned
    /// by CallNextHook.</param>
    /// <param name="wParam">
    /// Specifies whether the message was sent by the current thread.
    /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero.
    /// </param>
    /// <param name="lParam">
    /// Pointer to a CWPSTRUCT structure that contains details about the message.
    /// </param>
    /// <returns>
    /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
    /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
    /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
    /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
    /// procedure does not call CallNextHookEx, the return value should be zero. 
    /// </returns>
    /// <visibility>internal</visibility>
    /// <doctype>delegate</doctype>
    internal delegate IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Property that indicates the access scope of the hook.
    /// </summary>
    /// <visibility>internal</visibility>
    internal abstract bool IsGlobal { get; }

    /// <summary>
    /// Attaches the process to a hook.
    /// </summary>
    /// <param name="aHookId"></param>
    /// <param name="aHookCallback"></param>
    /// <returns></returns>
    /// <visibility>internal</visibility>
    internal abstract IntPtr Observe(int aHookId, HookCallback aHookCallback);

    /// <summary>
    /// Unsubscribes from the hook and stops firing events.
    /// </summary>
    /// <param name="aHandle">Integer value of Hook id.</param>
    /// <visibility>internal</visibility>
    internal static void StopObserve(IntPtr aHandle)
    {
        if (SafeNativeMethods.UnhookWindowsHookEx(aHandle) != 0)
        {
        }
    }

    /// <summary>
    /// This exception is thrown with the accurate error code if hookHandle is not established.
    /// </summary>
    /// <visibility>internal</visibility>
    internal static void ThrowLastUnmanagedErrorAsException()
    {
        int lastWin32Error = Marshal.GetLastWin32Error();
        throw new Win32Exception(lastWin32Error);
    }
}