namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Virtual Key Codes
/// </summary>
/// <remarks>
/// VirtualKeyCode.cs provides enumerations to define windows keyboard virtual key codes.
/// </remarks>
/// <visibility>public</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-20  AS00439  v0.00.04.005  Initial version
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the class
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-16  AS00565  v1.00.01.006  Removed public comments
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-20  AS00755  v1.00.03.041  Changed access modifier to public for the class
/// 2017-10-25  AS00944  v1.01.01.004  Modified visibility of code
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2018-08-13  AS01099  v1.01.02.007  Update revisionhistory formatiing
/// </revisionhistory>
public enum VirtualKeyCode
{
	/// <summary></summary>
	None = 0,
	/// <summary>Left mouse button</summary>
	LBUTTON = 1,
	/// <summary>Right mouse button</summary>
	RBUTTON = 2,
	/// <summary>Control-break processing</summary>
	CANCEL = 3,
	/// <summary>Middle mouse button (three-button mouse)</summary>
	MBUTTON = 4,
	/// <summary>Windows 2000/XP: X1 mouse button</summary>
	XBUTTON1 = 5,
	/// <summary>Windows 2000/XP: X2 mouse button</summary>
	XBUTTON2 = 6,
	/// <summary>BACKSPACE key</summary>
	BACK = 8,
	/// <summary>TAB key</summary>
	TAB = 9,
	/// <summary>CLEAR key</summary>
	CLEAR = 12,
	/// <summary>ENTER key</summary>
	RETURN = 13,
	/// <summary>SHIFT key</summary>
	SHIFT = 16,
	/// <summary>CTRL key</summary>
	CONTROL = 17,
	/// <summary>ALT key</summary>
	MENU = 18,
	/// <summary>PAUSE key</summary>
	PAUSE = 19,
	/// <summary>CAPS LOCK key</summary>
	CAPITAL = 20,
	/// <summary>Input Method Editor (IME) Kana mode</summary>
	HANGUL = 21,
	/// <summary>IME Kana mode</summary>
	KANA = 21,
	/// <summary>IME Junja mode</summary>
	JUNJA = 23,
	/// <summary>IME final mode</summary>
	FINAL = 24,
	/// <summary>IME Hanja mode</summary>
	HANJA = 25,
	/// <summary>IME Kanji mode</summary>
	KANJI = 25,
	/// <summary>ESC key</summary>
	ESCAPE = 27,
	/// <summary>IME convert</summary>
	CONVERT = 28,
	/// <summary>IME nonconvert</summary>
	NONCONVERT = 29,
	/// <summary>IME accept</summary>
	ACCEPT = 30,
	/// <summary>IME mode change request</summary>
	MODECHANGE = 31,
	/// <summary>SPACEBAR</summary>
	SPACE = 32,
	/// <summary>PAGE UP key</summary>
	PRIOR = 33,
	/// <summary>PAGE DOWN key</summary>
	NEXT = 34,
	/// <summary>END key</summary>
	END = 35,
	/// <summary>HOME key</summary>
	HOME = 36,
	/// <summary>LEFT ARROW key</summary>
	LEFT = 37,
	/// <summary>UP ARROW key</summary>
	UP = 38,
	/// <summary>RIGHT ARROW key</summary>
	RIGHT = 39,
	/// <summary>DOWN ARROW key</summary>
	DOWN = 40,
	/// <summary>SELECT key</summary>
	SELECT = 41,
	/// <summary>PRINT key</summary>
	PRINT = 42,
	/// <summary>EXECUTE key</summary>
	EXECUTE = 43,
	/// <summary>PRINT SCREEN key</summary>
	SNAPSHOT = 44,
	/// <summary>INS key</summary>
	INSERT = 45,
	/// <summary>DEL key</summary>
	DELETE = 46,
	/// <summary>HELP key</summary>
	HELP = 47,
	/// <summary>0 key</summary>
	VK_0 = 48,
	/// <summary>1 key</summary>
	VK_1 = 49,
	/// <summary>2 key</summary>
	VK_2 = 50,
	/// <summary>3 key</summary>
	VK_3 = 51,
	/// <summary>4 key</summary>
	VK_4 = 52,
	/// <summary>5 key</summary>
	VK_5 = 53,
	/// <summary>6 key</summary>
	VK_6 = 54,
	/// <summary>7 key</summary>
	VK_7 = 55,
	/// <summary>8 key</summary>
	VK_8 = 56,
	/// <summary>9 key</summary>
	VK_9 = 57,
	/// <summary>A key</summary>
	VK_A = 65,
	/// <summary>B key</summary>
	VK_B = 66,
	/// <summary>C key</summary>
	VK_C = 67,
	/// <summary>D key</summary>
	VK_D = 68,
	/// <summary>E key</summary>
	VK_E = 69,
	/// <summary>F key</summary>
	VK_F = 70,
	/// <summary>G key</summary>
	VK_G = 71,
	/// <summary>H key</summary>
	VK_H = 72,
	/// <summary>I key</summary>
	VK_I = 73,
	/// <summary>J key</summary>
	VK_J = 74,
	/// <summary>K key</summary>
	VK_K = 75,
	/// <summary>L key</summary>
	VK_L = 76,
	/// <summary>M key</summary>
	VK_M = 77,
	/// <summary>N key</summary>
	VK_N = 78,
	/// <summary>O key</summary>
	VK_O = 79,
	/// <summary>P key</summary>
	VK_P = 80,
	/// <summary>Q key</summary>
	VK_Q = 81,
	/// <summary>R key</summary>
	VK_R = 82,
	/// <summary>S key</summary>
	VK_S = 83,
	/// <summary>T key</summary>
	VK_T = 84,
	/// <summary>U key</summary>
	VK_U = 85,
	/// <summary>V key</summary>
	VK_V = 86,
	/// <summary>W key</summary>
	VK_W = 87,
	/// <summary>X key</summary>
	VK_X = 88,
	/// <summary>Y key</summary>
	VK_Y = 89,
	/// <summary>Z key</summary>
	VK_Z = 90,
	/// <summary>Left Windows key (Microsoft Natural keyboard) </summary>
	LWIN = 91,
	/// <summary>Right Windows key (Natural keyboard)</summary>
	RWIN = 92,
	/// <summary>Applications key (Natural keyboard)</summary>
	APPS = 93,
	/// <summary>Computer Sleep key</summary>
	SLEEP = 95,
	/// <summary>Numeric keypad 0 key</summary>
	NUMPAD0 = 96,
	/// <summary>Numeric keypad 1 key</summary>
	NUMPAD1 = 97,
	/// <summary>Numeric keypad 2 key</summary>
	NUMPAD2 = 98,
	/// <summary>Numeric keypad 3 key</summary>
	NUMPAD3 = 99,
	/// <summary>Numeric keypad 4 key</summary>
	NUMPAD4 = 100,
	/// <summary>Numeric keypad 5 key</summary>
	NUMPAD5 = 101,
	/// <summary>Numeric keypad 6 key</summary>
	NUMPAD6 = 102,
	/// <summary>Numeric keypad 7 key</summary>
	NUMPAD7 = 103,
	/// <summary>Numeric keypad 8 key</summary>
	NUMPAD8 = 104,
	/// <summary>Numeric keypad 9 key</summary>
	NUMPAD9 = 105,
	/// <summary>Multiply key</summary>
	MULTIPLY = 106,
	/// <summary>Add key</summary>
	ADD = 107,
	/// <summary>Separator key</summary>
	SEPARATOR = 108,
	/// <summary>Subtract key</summary>
	SUBTRACT = 109,
	/// <summary>Decimal key</summary>
	DECIMAL = 110,
	/// <summary>Divide key</summary>
	DIVIDE = 111,
	/// <summary>F1 key</summary>
	F1 = 112,
	/// <summary>F2 key</summary>
	F2 = 113,
	/// <summary>F3 key</summary>
	F3 = 114,
	/// <summary>F4 key</summary>
	F4 = 115,
	/// <summary>F5 key</summary>
	F5 = 116,
	/// <summary>F6 key</summary>
	F6 = 117,
	/// <summary>F7 key</summary>
	F7 = 118,
	/// <summary>F8 key</summary>
	F8 = 119,
	/// <summary>F9 key</summary>
	F9 = 120,
	/// <summary>F10 key</summary>
	F10 = 121,
	/// <summary>F11 key</summary>
	F11 = 122,
	/// <summary>F12 key</summary>
	F12 = 123,
	/// <summary>F13 key</summary>
	F13 = 124,
	/// <summary>F14 key</summary>
	F14 = 125,
	/// <summary>F15 key</summary>
	F15 = 126,
	/// <summary>F16 key</summary>
	F16 = 127,
	/// <summary>F17 key</summary>
	F17 = 128,
	/// <summary>F18 key</summary>
	F18 = 129,
	/// <summary>F19 key</summary>
	F19 = 130,
	/// <summary>F20 key</summary>
	F20 = 131,
	/// <summary>F21 key</summary>
	F21 = 132,
	/// <summary>F22 key, (PPC only) Key used to lock device.</summary>
	F22 = 133,
	/// <summary>F23 key</summary>
	F23 = 134,
	/// <summary>F24 key</summary>
	F24 = 135,
	/// <summary>NUM LOCK key</summary>
	NUMLOCK = 144,
	/// <summary>SCROLL LOCK key</summary>
	SCROLL = 145,
	/// <summary>Left SHIFT key</summary>
	LSHIFT = 160,
	/// <summary>Right SHIFT key</summary>
	RSHIFT = 161,
	/// <summary>Left CONTROL key</summary>
	LCONTROL = 162,
	/// <summary>Right CONTROL key</summary>
	RCONTROL = 163,
	/// <summary>Left MENU key</summary>
	LMENU = 164,
	/// <summary>Right MENU key</summary>
	RMENU = 165,
	/// <summary>Windows 2000/XP: Browser Back key</summary>
	BROWSER_BACK = 166,
	/// <summary>Windows 2000/XP: Browser Forward key</summary>
	BROWSER_FORWARD = 167,
	/// <summary>Windows 2000/XP: Browser Refresh key</summary>
	BROWSER_REFRESH = 168,
	/// <summary>Windows 2000/XP: Browser Stop key</summary>
	BROWSER_STOP = 169,
	/// <summary>Windows 2000/XP: Browser Search key </summary>
	BROWSER_SEARCH = 170,
	/// <summary>Windows 2000/XP: Browser Favorites key</summary>
	BROWSER_FAVORITES = 171,
	/// <summary>Windows 2000/XP: Browser Start and Home key</summary>
	BROWSER_HOME = 172,
	/// <summary>Windows 2000/XP: Volume Mute key</summary>
	VOLUME_MUTE = 173,
	/// <summary>Windows 2000/XP: Volume Down key</summary>
	VOLUME_DOWN = 174,
	/// <summary>Windows 2000/XP: Volume Up key</summary>
	VOLUME_UP = 175,
	/// <summary>Windows 2000/XP: Next Track key</summary>
	MEDIA_NEXT_TRACK = 176,
	/// <summary>Windows 2000/XP: Previous Track key</summary>
	MEDIA_PREV_TRACK = 177,
	/// <summary>Windows 2000/XP: Stop Media key</summary>
	MEDIA_STOP = 178,
	/// <summary>Windows 2000/XP: Play/Pause Media key</summary>
	MEDIA_PLAY_PAUSE = 179,
	/// <summary>Windows 2000/XP: Start Mail key</summary>
	LAUNCH_MAIL = 180,
	/// <summary>Windows 2000/XP: Select Media key</summary>
	LAUNCH_MEDIA_SELECT = 181,
	/// <summary>Windows 2000/XP: Start Application 1 key</summary>
	LAUNCH_APP1 = 182,
	/// <summary>Windows 2000/XP: Start Application 2 key</summary>
	LAUNCH_APP2 = 183,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard.</summary>
	OEM_1 = 186,
	/// <summary>Windows 2000/XP: For any country/region, the '+' key</summary>
	OEM_PLUS = 187,
	/// <summary>Windows 2000/XP: For any country/region, the ',' key</summary>
	OEM_COMMA = 188,
	/// <summary>Windows 2000/XP: For any country/region, the '-' key</summary>
	OEM_MINUS = 189,
	/// <summary>Windows 2000/XP: For any country/region, the '.' key</summary>
	OEM_PERIOD = 190,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard.</summary>
	OEM_2 = 191,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard. </summary>
	OEM_3 = 192,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard. </summary>
	OEM_4 = 219,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard. </summary>
	OEM_5 = 220,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard. </summary>
	OEM_6 = 221,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard. </summary>
	OEM_7 = 222,
	/// <summary>Used for miscellaneous characters; it can vary by keyboard.</summary>
	OEM_8 = 223,
	/// <summary>Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard</summary>
	OEM_102 = 226,
	/// <summary>Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key</summary>
	PROCESSKEY = 229,
	/// <summary>Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. 
	/// The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, 
	/// see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP</summary>
	PACKET = 231,
	/// <summary>Attn key</summary>
	ATTN = 246,
	/// <summary>CrSel key</summary>
	CRSEL = 247,
	/// <summary>ExSel key</summary>
	EXSEL = 248,
	/// <summary>Erase EOF key</summary>
	EREOF = 249,
	/// <summary>Play key</summary>
	PLAY = 250,
	/// <summary>Zoom key</summary>
	ZOOM = 251,
	/// <summary>Reserved </summary>
	NONAME = 252,
	/// <summary>PA1 key</summary>
	PA1 = 253,
	/// <summary>Clear key</summary>
	OEM_CLEAR = 254
}
