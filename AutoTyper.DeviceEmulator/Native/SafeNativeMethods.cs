using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides access to Native methods to the assembly.
/// </summary>
/// <remarks>
/// SafeNativeMethod provides the assembly to use common windows libraries to execute native methods.
/// Refer to documentation regarding what the native methods implemented in HenoohDeviceEmulator, and update
/// documents as necessary.
/// </remarks>
/// <visibility>internal</visibility>
internal class SafeNativeMethods
{
    /// <summary>
    /// Private constructor.
    /// </summary>
    /// <remarks>
    /// <see cref="T:AutoTyper.DeviceEmulator.Native.SafeNativeMethods" /> is an internal class that is apparently never instantiated, and only
    /// contain static methods. Adding a private constructor will prevent the compiler from generating a
    /// default constructor.
    /// </remarks>
    private SafeNativeMethods()
    {
    }

    /// <summary>
    /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain. 
    /// A hook procedure can call this function either before or after processing the hook information. 
    /// </summary>
    /// <param name="idHook">Ignored.</param>
    /// <param name="nCode">[in] Specifies the hook code passed to the current hook procedure.</param>
    /// <param name="wParam">[in] Specifies the wParam value passed to the current hook procedure.</param>
    /// <param name="lParam">[in] Specifies the lParam value passed to the current hook procedure.</param>
    /// <returns>This value is returned by the next hook procedure in the chain.</returns>
    /// <remarks>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974%28v=vs.85%29.aspx
    /// </remarks>
    /// <visibility>internal</visibility>
    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    internal static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain. 
    /// You would install a hook procedure to monitor the system for certain types of events. These events 
    /// are associated either with a specific thread or with all threads in the same desktop as the calling thread. 
    /// </summary>
    /// <param name="idHook">
    /// [in] Specifies the type of hook procedure to be installed. This parameter can be one of the following values.
    /// </param>
    /// <param name="lpfn">
    /// [in] Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a 
    /// thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link 
    /// library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
    /// </param>
    /// <param name="hMod">
    /// [in] Handle to the DLL containing the hook procedure pointed to by the lpfn parameter. 
    /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by 
    /// the current process and if the hook procedure is within the code associated with the current process. 
    /// </param>
    /// <param name="dwThreadId">
    /// [in] Specifies the identifier of the thread with which the hook procedure is to be associated. 
    /// If this parameter is zero, the hook procedure is associated with all existing threads running in the 
    /// same desktop as the calling thread. 
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is the handle to the hook procedure.
    /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
    /// </remarks>
    /// <visibility>internal</visibility>
    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr SetWindowsHookEx(int idHook, ObserverAbstract.HookCallback lpfn, IntPtr hMod, int dwThreadId);

    /// <summary>
    /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function. 
    /// </summary>
    /// <param name="idHook">
    /// [in] Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx. 
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
    /// </remarks>
    /// <visibility>internal</visibility>
    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern int UnhookWindowsHookEx(IntPtr idHook);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <visibility>internal</visibility>
    [DllImport("user32")]
    internal static extern int GetDoubleClickTime();

    /// <summary>
    /// The ToAscii function translates the specified virtual-key code and keyboard 
    /// state to the corresponding character or characters. The function translates the code 
    /// using the input language and physical keyboard layout identified by the keyboard layout handle.
    /// </summary>
    /// <param name="uVirtKey">
    /// [in] Specifies the virtual-key code to be translated. 
    /// </param>
    /// <param name="uScanCode">
    /// [in] Specifies the hardware scan code of the key to be translated. 
    /// The high-order bit of this value is set if the key is up (not pressed). 
    /// </param>
    /// <param name="lpbKeyState">
    /// [in] Pointer to a 256-byte array that contains the current keyboard state. 
    /// Each element (byte) in the array contains the state of one key. 
    /// If the high-order bit of a byte is set, the key is down (pressed). 
    /// The low bit, if set, indicates that the key is toggled on. In this function, 
    /// only the toggle bit of the CAPS LOCK key is relevant. The toggle state 
    /// of the NUM LOCK and SCROLL LOCK keys is ignored.
    /// </param>
    /// <param name="lpwTransKey">
    /// [out] Pointer to the buffer that receives the translated character or characters. 
    /// </param>
    /// <param name="fuState">
    /// [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 
    /// </param>
    /// <returns>
    /// If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values. 
    /// Value Meaning 
    /// 0 The specified virtual key has no translation for the current state of the keyboard. 
    /// 1 One character was copied to the buffer. 
    /// 2 Two characters were copied to the buffer. This usually happens when a dead-key character 
    /// (accent or diacritic) stored in the keyboard layout cannot be composed with the specified 
    /// virtual key to form a single character. 
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
    /// </remarks>
    /// <visibility>internal</visibility>
    [DllImport("user32")]
    internal static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

    /// <summary>
    /// The GetKeyboardState function copies the status of the 256 virtual keys to the 
    /// specified buffer. 
    /// </summary>
    /// <param name="pbKeyState">
    /// [in] Pointer to a 256-byte array that contains keyboard key states. 
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
    /// </remarks>
    /// <visibility>internal</visibility>
    [DllImport("user32")]
    internal static extern int GetKeyboardState(byte[] pbKeyState);

    /// <summary>
    /// The GetKeyState function retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled 
    /// (on, off—alternating each time the key is pressed). 
    /// </summary>
    /// <param name="vKey">
    /// [in] Specifies a virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code. 
    /// </param>
    /// <returns>
    /// The return value specifies the status of the specified virtual key, as follows: 
    ///             If the high-order bit is 1, the key is down; otherwise, it is up.
    ///             If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
    /// </returns>
    /// <remarks>http://msdn.microsoft.com/en-us/library/ms646301.aspx</remarks>
    /// <visibility>internal</visibility>
    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    internal static extern short GetKeyState(int vKey);

    /// <summary>
    /// SendInput.
    /// </summary>
    /// <param name="numberOfInputs"></param>
    /// <param name="inputs"></param>
    /// <param name="sizeOfInputStructure"></param>
    /// <returns></returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint SendInput(uint numberOfInputs, Input[] inputs, int sizeOfInputStructure);

    /// <summary>
    /// Retrieves the unmanaged thread identifier of the calling thread.
    /// </summary>
    /// <returns></returns>
    /// <visibility>internal</visibility>
    [DllImport("kernel32")]
    internal static extern int GetCurrentThreadId();

    /// <summary>
    /// Converts a point in a window from logical coordinates into physical coordinates, regardless of
    /// dots per inch (dpi) awareness of the caller.
    /// </summary>
    /// <param name="hMonitor">A handle to the window whose transform is used for the conversion.</param>
    /// <param name="aPoint">A pointer to a Point structure that specifies the logical coordinates to be converted.
    /// The new physical coordinates are copied into this structure if the function succeeds.</param>
    /// <returns>Returns true if successful, or false otherwise.</returns>
    /// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/dn384110%28v=vs.85%29.aspx</remarks>
    [DllImport("User32.dll")]
    internal static extern bool LogicalToPhysicalPointForPerMonitorDPI([In] IntPtr hMonitor, out Point aPoint);

    /// <summary>
    /// Converts a point in a window from physical coordinates into logical coordinates, regardless of
    /// dots per inch (dpi) awareness of the caller.
    /// </summary>
    /// <param name="hMonitor">A handle to the window whose transform is used for the conversion.</param>
    /// <param name="aPoint">A pointer to a Point structure the specifies the physical/screen coordinates to be
    /// converted. The new logical coordinates are copied into this structure if the function succeds.</param>
    /// <returns>Returns true if successful, or false otherwise.</returns>
    /// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/dn384112%28v=vs.85%29.aspx</remarks>
    [DllImport("User32.dll")]
    internal static extern bool PhysicalToLogicalPointForPerMonitorDPI([In] IntPtr hMonitor, out Point aPoint);

    /// <summary>
    ///
    /// </summary>
    /// <param name="uiAction"></param>
    /// <param name="uiParam"></param>
    /// <param name="pvParam"></param>
    /// <param name="fWinIni"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    internal static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);
}