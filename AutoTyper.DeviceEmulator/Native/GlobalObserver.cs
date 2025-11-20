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
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-29  AS00447  v0.00.04.013  Initial Version
/// 2015-06-04  AS00466  v0.00.04.028  Implement changes to GlobalHooker to GlobalObserver
/// 2015-06-08  AS00468  v0.00.04.030  Renamed Hooker class to ObserverAbstract
/// 2015-07-20  AS00492  v0.00.04.038  Made updates to remark and summary
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-09  AS00559  v1.00.01.001  Renamed Subscribe method to Observe Method
/// 2015-11-17  AS00566  v1.00.01.007  Modified the summary, remarks and method comments
/// 2015-11-23  AS00573  v1.00.01.010  Modified summary and remarks
/// 2016-02-06  AS00619  v1.00.03.001  Modified the comment for IsGlobal method
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-09-22  AS00727  v1.00.03.027  Use moved NativeMethods
/// 2016-09-24  AS00729  v1.00.03.028  Renamed NativeMethods class to SafeNativeMethods class
/// 2016-10-02  AS00737  v1.00.03.031  Replace int with IntPtr for SafeNativeMethod calls
/// 2016-10-12  AS00747  v1.00.03.034  Changed access modifier from public to internal
/// 2016-10-18  AS00753  v1.00.03.039  Added visibilty xml tags, fixed incorrect versions in revisionhistory
/// 2016-10-19  AS00754  v1.00.03.040  Modified comments throughout code, rearranged layout
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
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
