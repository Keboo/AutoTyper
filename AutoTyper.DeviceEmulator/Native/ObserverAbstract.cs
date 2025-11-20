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
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-28  AS00446  v0.00.04.012  Initial Version
/// 2015-05-14  AS00457  v0.00.04.021  During Unsubscribe, result of 0 is expected from UnhookWindowsHookEx
/// 2015-06-08  AS00468  v0.00.04.030  Renamed Hooker class to ObserverAbstract
/// 2015-08-18  AS00509  v0.00.04.050  Correct file name under remarks as ObserverAbstract
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-09  AS00559  v1.00.01.001  Renamed Subscribe method to Observe, Unsubscribe to StopObserve
/// 2015-11-12  AS00562  v1.00.01.004  Moved HookCallback delegate inside to ObserverAbstract from Native namespace
/// 2016-07-13  AS00693  v1.00.03.018  Modified summary and remarks
/// 2016-07-24  AS00695  v1.00.03.019  Added XML comment to StopObserve method
/// 2016-09-22  AS00727  v1.00.03.027  Moved P/Invoke methods to NativeMethods class
/// 2016-09-24  AS00729  v1.00.03.028  Renamed NativeMethods class to SafeNativeMethods class
/// 2016-10-02  AS00737  v1.00.00.031  Replace int with IntPtr for SafeNativeMethod calls
/// 2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
/// 2016-10-14  AS00749  v1.00.03.036  Modified access modifier for HookCallback delegate to internal
/// 2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2017-02-25  AS00842  v1.00.06.011  Observe method parameters to follow Henooh Style Guidelines
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-02-04  AS01147  v1.01.03.006  Add XML header comments to all methods
/// 2019-03-25  AS01160  v1.01.03.008  Resolved CA1822, added static prefix to StopObserve method
/// </revisionhistory>
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