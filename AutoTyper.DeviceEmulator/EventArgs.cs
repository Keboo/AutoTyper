using System;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// Specifies constants that define which mouse button was pressed.
/// Compatible with System.Windows.Forms.MouseButtons.
/// </summary>
[Flags]
public enum MouseButtons
{
    /// <summary>No mouse button was pressed.</summary>
    None = 0,
    /// <summary>The left mouse button was pressed.</summary>
    Left = 1048576,
    /// <summary>The right mouse button was pressed.</summary>
    Right = 2097152,
    /// <summary>The middle mouse button was pressed.</summary>
    Middle = 4194304,
    /// <summary>The first XButton (XBUTTON1) was pressed.</summary>
    XButton1 = 8388608,
    /// <summary>The second XButton (XBUTTON2) was pressed.</summary>
    XButton2 = 16777216
}

/// <summary>
/// Specifies key codes and modifiers.
/// Compatible with System.Windows.Forms.Keys.
/// </summary>
[Flags]
public enum Keys
{
    /// <summary>The bitmask to extract modifiers from a key value.</summary>
    Modifiers = -65536,
    /// <summary>No key pressed.</summary>
    None = 0,
    /// <summary>The BACKSPACE key.</summary>
    Back = 8,
    /// <summary>The TAB key.</summary>
    Tab = 9,
    /// <summary>The LINEFEED key.</summary>
    LineFeed = 10,
    /// <summary>The CLEAR key.</summary>
    Clear = 12,
    /// <summary>The RETURN key.</summary>
    Return = 13,
    /// <summary>The ENTER key.</summary>
    Enter = 13,
    /// <summary>The SHIFT key.</summary>
    ShiftKey = 16,
    /// <summary>The CTRL key.</summary>
    ControlKey = 17,
    /// <summary>The ALT key.</summary>
    Menu = 18,
    /// <summary>The PAUSE key.</summary>
    Pause = 19,
    /// <summary>The CAPS LOCK key.</summary>
    Capital = 20,
    /// <summary>The CAPS LOCK key.</summary>
    CapsLock = 20,
    /// <summary>The ESC key.</summary>
    Escape = 27,
    /// <summary>The SPACEBAR key.</summary>
    Space = 32,
    /// <summary>The PAGE UP key.</summary>
    PageUp = 33,
    /// <summary>The PAGE DOWN key.</summary>
    PageDown = 34,
    /// <summary>The END key.</summary>
    End = 35,
    /// <summary>The HOME key.</summary>
    Home = 36,
    /// <summary>The LEFT ARROW key.</summary>
    Left = 37,
    /// <summary>The UP ARROW key.</summary>
    Up = 38,
    /// <summary>The RIGHT ARROW key.</summary>
    Right = 39,
    /// <summary>The DOWN ARROW key.</summary>
    Down = 40,
    /// <summary>The PRINT SCREEN key.</summary>
    PrintScreen = 44,
    /// <summary>The INS key.</summary>
    Insert = 45,
    /// <summary>The DEL key.</summary>
    Delete = 46,
    /// <summary>The 0 key.</summary>
    D0 = 48,
    /// <summary>The 1 key.</summary>
    D1 = 49,
    /// <summary>The 2 key.</summary>
    D2 = 50,
    /// <summary>The 3 key.</summary>
    D3 = 51,
    /// <summary>The 4 key.</summary>
    D4 = 52,
    /// <summary>The 5 key.</summary>
    D5 = 53,
    /// <summary>The 6 key.</summary>
    D6 = 54,
    /// <summary>The 7 key.</summary>
    D7 = 55,
    /// <summary>The 8 key.</summary>
    D8 = 56,
    /// <summary>The 9 key.</summary>
    D9 = 57,
    /// <summary>The A key.</summary>
    A = 65,
    /// <summary>The B key.</summary>
    B = 66,
    /// <summary>The C key.</summary>
    C = 67,
    /// <summary>The D key.</summary>
    D = 68,
    /// <summary>The E key.</summary>
    E = 69,
    /// <summary>The F key.</summary>
    F = 70,
    /// <summary>The G key.</summary>
    G = 71,
    /// <summary>The H key.</summary>
    H = 72,
    /// <summary>The I key.</summary>
    I = 73,
    /// <summary>The J key.</summary>
    J = 74,
    /// <summary>The K key.</summary>
    K = 75,
    /// <summary>The L key.</summary>
    L = 76,
    /// <summary>The M key.</summary>
    M = 77,
    /// <summary>The N key.</summary>
    N = 78,
    /// <summary>The O key.</summary>
    O = 79,
    /// <summary>The P key.</summary>
    P = 80,
    /// <summary>The Q key.</summary>
    Q = 81,
    /// <summary>The R key.</summary>
    R = 82,
    /// <summary>The S key.</summary>
    S = 83,
    /// <summary>The T key.</summary>
    T = 84,
    /// <summary>The U key.</summary>
    U = 85,
    /// <summary>The V key.</summary>
    V = 86,
    /// <summary>The W key.</summary>
    W = 87,
    /// <summary>The X key.</summary>
    X = 88,
    /// <summary>The Y key.</summary>
    Y = 89,
    /// <summary>The Z key.</summary>
    Z = 90,
    /// <summary>The 0 key on the numeric keypad.</summary>
    NumPad0 = 96,
    /// <summary>The 1 key on the numeric keypad.</summary>
    NumPad1 = 97,
    /// <summary>The 2 key on the numeric keypad.</summary>
    NumPad2 = 98,
    /// <summary>The 3 key on the numeric keypad.</summary>
    NumPad3 = 99,
    /// <summary>The 4 key on the numeric keypad.</summary>
    NumPad4 = 100,
    /// <summary>The 5 key on the numeric keypad.</summary>
    NumPad5 = 101,
    /// <summary>The 6 key on the numeric keypad.</summary>
    NumPad6 = 102,
    /// <summary>The 7 key on the numeric keypad.</summary>
    NumPad7 = 103,
    /// <summary>The 8 key on the numeric keypad.</summary>
    NumPad8 = 104,
    /// <summary>The 9 key on the numeric keypad.</summary>
    NumPad9 = 105,
    /// <summary>The multiply key.</summary>
    Multiply = 106,
    /// <summary>The add key.</summary>
    Add = 107,
    /// <summary>The subtract key.</summary>
    Subtract = 109,
    /// <summary>The decimal key.</summary>
    Decimal = 110,
    /// <summary>The divide key.</summary>
    Divide = 111,
    /// <summary>The F1 key.</summary>
    F1 = 112,
    /// <summary>The F2 key.</summary>
    F2 = 113,
    /// <summary>The F3 key.</summary>
    F3 = 114,
    /// <summary>The F4 key.</summary>
    F4 = 115,
    /// <summary>The F5 key.</summary>
    F5 = 116,
    /// <summary>The F6 key.</summary>
    F6 = 117,
    /// <summary>The F7 key.</summary>
    F7 = 118,
    /// <summary>The F8 key.</summary>
    F8 = 119,
    /// <summary>The F9 key.</summary>
    F9 = 120,
    /// <summary>The F10 key.</summary>
    F10 = 121,
    /// <summary>The F11 key.</summary>
    F11 = 122,
    /// <summary>The F12 key.</summary>
    F12 = 123,
    /// <summary>The NUM LOCK key.</summary>
    NumLock = 144,
    /// <summary>The SCROLL LOCK key.</summary>
    Scroll = 145,
    /// <summary>The left SHIFT key.</summary>
    LShiftKey = 160,
    /// <summary>The right SHIFT key.</summary>
    RShiftKey = 161,
    /// <summary>The left CTRL key.</summary>
    LControlKey = 162,
    /// <summary>The right CTRL key.</summary>
    RControlKey = 163,
    /// <summary>The left ALT key.</summary>
    LMenu = 164,
    /// <summary>The right ALT key.</summary>
    RMenu = 165,
    /// <summary>The SHIFT modifier key.</summary>
    Shift = 65536,
    /// <summary>The CTRL modifier key.</summary>
    Control = 131072,
    /// <summary>The ALT modifier key.</summary>
    Alt = 262144
}

/// <summary>
/// Delegate for keyboard events.
/// </summary>
public delegate void KeyEventHandler(object sender, KeyEventArgs e);

/// <summary>
/// Delegate for key press events.
/// </summary>
public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);

/// <summary>
/// Delegate for mouse events.
/// </summary>
public delegate void MouseEventHandler(object sender, MouseEventArgs e);

/// <summary>
/// Provides data for keyboard-related events.
/// Compatible with System.Windows.Forms.KeyEventArgs.
/// </summary>
public class KeyEventArgs : EventArgs
{
    /// <summary>Gets the keyboard code for a KeyDown or KeyUp event.</summary>
    public Keys KeyCode { get; }

    /// <summary>Gets the key data for a KeyDown or KeyUp event.</summary>
    public Keys KeyData { get; }

    /// <summary>Gets the modifier flags for a KeyDown or KeyUp event.</summary>
    public Keys Modifiers => KeyData & Keys.Modifiers;

    /// <summary>Gets a value indicating whether the ALT key was pressed.</summary>
    public virtual bool Alt => (KeyData & Keys.Alt) == Keys.Alt;

    /// <summary>Gets a value indicating whether the CTRL key was pressed.</summary>
    public bool Control => (KeyData & Keys.Control) == Keys.Control;

    /// <summary>Gets a value indicating whether the SHIFT key was pressed.</summary>
    public virtual bool Shift => (KeyData & Keys.Shift) == Keys.Shift;

    /// <summary>Gets or sets a value indicating whether the event was handled.</summary>
    public bool Handled { get; set; }

    /// <summary>Gets or sets a value indicating whether the key event should be passed on to the underlying control.</summary>
    public bool SuppressKeyPress { get; set; }

    /// <summary>Gets the keyboard value for a KeyDown or KeyUp event.</summary>
    public int KeyValue => (int)KeyData & 65535;

    /// <summary>Initializes a new instance of the KeyEventArgs class.</summary>
    public KeyEventArgs(Keys keyData)
    {
        KeyData = keyData;
        KeyCode = keyData & ~Keys.Modifiers;
    }
}

/// <summary>
/// Provides data for the KeyPress event.
/// Compatible with System.Windows.Forms.KeyPressEventArgs.
/// </summary>
public class KeyPressEventArgs : EventArgs
{
    /// <summary>Gets the character corresponding to the key pressed.</summary>
    public char KeyChar { get; set; }

    /// <summary>Gets or sets a value indicating whether the KeyPress event was handled.</summary>
    public bool Handled { get; set; }

    /// <summary>Initializes a new instance of the KeyPressEventArgs class.</summary>
    public KeyPressEventArgs(char keyChar)
    {
        KeyChar = keyChar;
    }
}

/// <summary>
/// Provides data for mouse events.
/// Compatible with System.Windows.Forms.MouseEventArgs.
/// </summary>
public class MouseEventArgs : EventArgs
{
    /// <summary>Gets which mouse button was pressed.</summary>
    public MouseButtons Button { get; }

    /// <summary>Gets the number of times the mouse button was pressed and released.</summary>
    public int Clicks { get; }

    /// <summary>Gets the x-coordinate of the mouse.</summary>
    public int X { get; }

    /// <summary>Gets the y-coordinate of the mouse.</summary>
    public int Y { get; }

    /// <summary>Gets a signed count of the number of detents the mouse wheel has rotated.</summary>
    public int Delta { get; }

    /// <summary>Gets the location of the mouse.</summary>
    public System.Drawing.Point Location => new System.Drawing.Point(X, Y);

    /// <summary>Initializes a new instance of the MouseEventArgs class.</summary>
    public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
    {
        Button = button;
        Clicks = clicks;
        X = x;
        Y = y;
        Delta = delta;
    }
}