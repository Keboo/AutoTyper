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
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-29  AS00447  v0.00.04.013  Initial Version
/// 2015-06-08  AS00468  v0.00.04.030  Renamed Hooker class to ObserverAbstract
/// 2015-07-23  AS00495  v0.00.04.041  Renamed class to AppObserver
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-09  AS00559  v1.00.01.001  Renamed Subscribe method to Observe Method and modify CodingStyle
/// 2016-02-06  AS00619  v1.00.03.001  Modified the comment for IsGlobal method
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-09-22  AS00727  v1.00.03.027  Use moved NativeMethods
/// 2016-09-24  AS00729  v1.00.03.028  Renamed NativeMethods class to SafeNativeMethods class
/// 2016-10-01  AS00735  v1.00.03.030  Moved GetCurrentThreadId class to SaveNativeMethods class
/// 2016-10-02  AS00737  v1.00.03.031  Replace int with IntPtr for SafeNativeMethod calls
/// 2016-10-11  AS00746  v1.00.03.033  Added visibility xml tags, modified revision history
/// 2016-10-19  AS00754  v1.00.03.040  Modified summary and comments
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-03-27  AS01162  v1.01.03.010  Resolve CA1812 by adding a private constructor
/// 2019-05-02  AS01185  v1.01.04.012  Change the class accessor to be public to allow AppObserver to be set
/// </revisionhistory>
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