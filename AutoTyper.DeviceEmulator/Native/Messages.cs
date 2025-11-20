namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
///  Provides message that is provided with mouse and keboard actions.
///  </summary>
///  <remarks>
///  Values are from Winuser.h in Microsoft SDK
///  </remarks>
///  <visibility>internal</visibility>
internal class Messages
{
    /// <summary>
    /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. 
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_MOUSEMOVE = 512;

    /// <summary>
    /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_LBUTTONDOWN = 513;

    /// <summary>
    /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_RBUTTONDOWN = 516;

    /// <summary>
    /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_MBUTTONDOWN = 519;

    /// <summary>
    /// The WM_LBUTTONUP message is posted when the user releases the left mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_LBUTTONUP = 514;

    /// <summary>
    /// The WM_RBUTTONUP message is posted when the user releases the right mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_RBUTTONUP = 517;

    /// <summary>
    /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_MBUTTONUP = 520;

    /// <summary>
    /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_LBUTTONDBLCLK = 515;

    /// <summary>
    /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_RBUTTONDBLCLK = 518;

    /// <summary>
    /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_MBUTTONDBLCLK = 521;

    /// <summary>
    /// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel. 
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_MOUSEWHEEL = 522;

    /// <summary>
    /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X mouse button. 
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_XBUTTONDOWN = 523;

    /// <summary>
    /// The WM_XBUTTONUP message is posted when the user releases the first or second X  mouse button.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_XBUTTONUP = 524;

    /// <summary>
    /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X mouse button.
    /// </summary>
    /// <remarks>Only windows that have the CS_DBLCLKS style can receive WM_XBUTTONDBLCLK messages.</remarks>
    /// <visibility>internal</visibility>
    internal const int WM_XBUTTONDBLCLK = 525;

    /// <summary>
    /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
    /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_KEYDOWN = 256;

    /// <summary>
    /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
    /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
    /// or a keyboard key that is pressed when a window has the keyboard focus.
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_KEYUP = 257;

    /// <summary>
    /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
    /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
    /// presses another key. It also occurs when no window currently has the keyboard focus; 
    /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
    /// receives the message can distinguish between these two contexts by checking the context 
    /// code in the lParam parameter. 
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_SYSKEYDOWN = 260;

    /// <summary>
    /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
    /// releases a key that was pressed while the ALT key was held down. It also occurs when no 
    /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent 
    /// to the active window. The window that receives the message can distinguish between 
    /// these two contexts by checking the context code in the lParam parameter. 
    /// </summary>
    /// <visibility>internal</visibility>
    internal const int WM_SYSKEYUP = 261;
}