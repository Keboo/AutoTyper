using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

using AutoTyper.DeviceEmulator.Native;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// KeyboardController provides methods to type text and keys. 
/// </summary>
/// <remarks>
/// KeyboardController provides methods that help simulate typing method,  including method that works like 
/// a human types. 
/// </remarks>
/// <visibility>public</visibility>
public class KeyboardController : BaseController
{
    /// <summary>
    /// Returns a dictionary that contains the char key as the key and VirtualKeyCode as value.
    /// </summary>
    /// <visibility>private</visibility>
    private static Dictionary<char, VirtualKeyCode> VirtualKeyCodeDictionary => new Dictionary<char, VirtualKeyCode>
    {
        { '0', VirtualKeyCode.VK_0 },
        { '1', VirtualKeyCode.VK_1 },
        { '2', VirtualKeyCode.VK_2 },
        { '3', VirtualKeyCode.VK_3 },
        { '4', VirtualKeyCode.VK_4 },
        { '5', VirtualKeyCode.VK_5 },
        { '6', VirtualKeyCode.VK_6 },
        { '7', VirtualKeyCode.VK_7 },
        { '8', VirtualKeyCode.VK_8 },
        { '9', VirtualKeyCode.VK_9 },
        { 'a', VirtualKeyCode.VK_A },
        { 'b', VirtualKeyCode.VK_B },
        { 'c', VirtualKeyCode.VK_C },
        { 'd', VirtualKeyCode.VK_D },
        { 'e', VirtualKeyCode.VK_E },
        { 'f', VirtualKeyCode.VK_F },
        { 'g', VirtualKeyCode.VK_G },
        { 'h', VirtualKeyCode.VK_H },
        { 'i', VirtualKeyCode.VK_I },
        { 'j', VirtualKeyCode.VK_J },
        { 'k', VirtualKeyCode.VK_K },
        { 'l', VirtualKeyCode.VK_L },
        { 'm', VirtualKeyCode.VK_M },
        { 'n', VirtualKeyCode.VK_N },
        { 'o', VirtualKeyCode.VK_O },
        { 'p', VirtualKeyCode.VK_P },
        { 'q', VirtualKeyCode.VK_Q },
        { 'r', VirtualKeyCode.VK_R },
        { 's', VirtualKeyCode.VK_S },
        { 't', VirtualKeyCode.VK_T },
        { 'u', VirtualKeyCode.VK_U },
        { 'v', VirtualKeyCode.VK_V },
        { 'w', VirtualKeyCode.VK_W },
        { 'x', VirtualKeyCode.VK_X },
        { 'y', VirtualKeyCode.VK_Y },
        { 'z', VirtualKeyCode.VK_Z },
        { ' ', VirtualKeyCode.SPACE },
        { '\n', VirtualKeyCode.RETURN },
        { '\r', VirtualKeyCode.RETURN },
        { '\t', VirtualKeyCode.TAB }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.KeyboardController" /> with no arguments.
    /// </summary>
    /// <visibility>public</visibility>
    public KeyboardController()
    {
    }

    /// <summary>
    /// Default constructor with CancellationToken.
    /// </summary>
    /// <param name="aCancellationToken"></param>
    /// <visibility>public</visibility>
    public KeyboardController(CancellationToken aCancellationToken)
        : base(aCancellationToken)
    {
    }

    /// <summary>
    /// Method that sends input to press and release the VirtualKeyCode. 
    /// </summary>
    /// <param name="aVirtualKeyCode"></param>
    /// <visibility>public</visibility>
    public void Type(VirtualKeyCode aVirtualKeyCode)
    {
        if (RunMode == 4)
        {
            return;
        }
        PressKey(aVirtualKeyCode);
        ReleaseKey(aVirtualKeyCode);
    }

    /// <summary>
    /// Method that sends inputs to type a given string.
    /// </summary>
    /// <param name="aString"></param>
    /// <visibility>public</visibility>
    public void TypeString(string aString)
    {
        TypeString(aString, 0, naturalTyping: false, 0, 0);
    }

    /// <summary>
    /// Method that sends inputs to type a given string with given interval.
    /// </summary>
    /// <param name="aString"></param>
    /// <param name="aInterval"></param>
    /// <visibility>public</visibility>
    public void TypeString(string aString, int aInterval)
    {
        TypeString(aString, aInterval, naturalTyping: false, 0, 0);
    }

    /// <summary>
    /// Method that sends inputs to type a given string with natural typing interval.
    /// </summary>
    /// <param name="aString"></param>
    /// <param name="aWordPerMinute"></param>
    /// <visibility>public</visibility>
    public void TypeStringNaturally(string aString, int aWordPerMinute)
    {
        TypeStringNaturally(aString, aWordPerMinute, 0);
    }

    /// <summary>
    /// Method that sends inputs to type a given string with natural typing interval.
    /// Allows variance in typing speed.
    /// </summary>
    /// <param name="aString"></param>
    /// <param name="aWordPerMinute"></param>
    /// <param name="aPlusMinusVariance"></param>
    /// <visibility>public</visibility>
    public void TypeStringNaturally(string aString, int aWordPerMinute, int aPlusMinusVariance)
    {
        TypeString(aString, 0, naturalTyping: true, aWordPerMinute, aPlusMinusVariance);
    }

    /// <summary>
    /// Method that sends inputs to type a given string.
    /// </summary>
    /// <param name="aString"></param>
    /// <param name="aInterval"></param>
    /// <param name="naturalTyping"></param>
    /// <param name="aWordPerMinute"></param>
    /// <param name="aPlusMinusVariance"></param>
    /// <visibility>public</visibility>
    public void TypeString(string aString, int aInterval, bool naturalTyping, int aWordPerMinute, int aPlusMinusVariance)
    {
        if (string.IsNullOrWhiteSpace(aString))
        {
            return;
        }
        double num = 0.0;
        if (naturalTyping)
        {
            double num2 = 60000.0 / ((double)aWordPerMinute * 5.0);
            Random random = new Random();
            num = num2 + (double)random.Next(-1 * aPlusMinusVariance, aPlusMinusVariance);
        }
        int num3 = 0;
        char[] array = aString.ToCharArray();
        foreach (char c in array)
        {
            num3++;
            if (char.IsUpper(c))
            {
                TypeChar(c, aShiftDown: true);
            }
            else
            {
                TypeChar(c, aShiftDown: false);
            }
            if ((aInterval != 0 || naturalTyping) && num3 < aString.Length)
            {
                if (naturalTyping)
                {
                    Sleep((int)num);
                }
                else
                {
                    Sleep(aInterval);
                }
            }
        }
    }

    /// <summary>
    /// Method that sends inputs to type an array of VirtualKeyCodes.
    /// </summary>
    /// <param name="aVirtualKeyCodeArray"></param>
    /// <visibility>public</visibility>
    public void Type(params VirtualKeyCode[] aVirtualKeyCodeArray)
    {
        if (RunMode == 4)
        {
            return;
        }
        foreach (VirtualKeyCode aVirtualKeyCode in aVirtualKeyCodeArray)
        {
            PressKey(aVirtualKeyCode);
            ReleaseKey(aVirtualKeyCode);
        }
    }

    /// <summary>
    /// Method that sends inputs to type a char.
    /// </summary>
    /// <param name="aChar"></param>
    /// <param name="aShiftDown"></param>
    /// <visibility>public</visibility>
    public void TypeChar(char aChar, bool aShiftDown)
    {
        if (RunMode == 4)
        {
            return;
        }
        char key = char.ToLower(aChar);
        if (!VirtualKeyCodeDictionary.TryGetValue(key, out var value))
        {
            return;
        }
        if (aShiftDown)
        {
            PressKey(VirtualKeyCode.SHIFT);
        }
        PressKey(value);
        ReleaseKey(value);
        if (aShiftDown)
        {
            ReleaseKey(VirtualKeyCode.SHIFT);
        }
    }

    /// <summary>
    /// Method that simulates holding down of modifier key and pressing VirtualKeyCode.
    /// </summary>
    /// <param name="aModifierVirtualKeyCode"></param>
    /// <param name="aVirtualKeyCode"></param>
    /// <visibility>public</visibility>
    public void ModifiedKeyStroke(VirtualKeyCode aModifierVirtualKeyCode, VirtualKeyCode aVirtualKeyCode)
    {
        if (RunMode == 4)
        {
            return;
        }
        PressKey(aModifierVirtualKeyCode);
        PressKey(aVirtualKeyCode);
        ReleaseKey(aVirtualKeyCode);
        ReleaseKey(aModifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that simulates holding down of modifier key and pressing VirtualKeyCode.
    /// </summary>
    /// <param name="aModifierVirtualKeyCode"></param>
    /// <param name="aVirtualKeyCodeArray"></param>
    /// <visibility>public</visibility>
    public void ModifiedKeyStroke(VirtualKeyCode aModifierVirtualKeyCode, params VirtualKeyCode[] aVirtualKeyCodeArray)
    {
        if (RunMode == 4)
        {
            return;
        }
        PressKey(aModifierVirtualKeyCode);
        foreach (VirtualKeyCode aVirtualKeyCode in aVirtualKeyCodeArray)
        {
            PressKey(aVirtualKeyCode);
            ReleaseKey(aVirtualKeyCode);
        }
        ReleaseKey(aModifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that simulates holding down of two modifier keys and pressing VirtualKeyCode.
    /// </summary>
    /// <param name="aFirstModifierVirtualKeyCode"></param>
    /// <param name="aSecondModifierVirtualKeyCode"></param>
    /// <param name="aVirtualKeyCode"></param>
    /// <visibility>public</visibility>
    public void ModifiedKeyStroke(VirtualKeyCode aFirstModifierVirtualKeyCode, VirtualKeyCode aSecondModifierVirtualKeyCode, VirtualKeyCode aVirtualKeyCode)
    {
        if (RunMode == 4)
        {
            return;
        }
        PressKey(aFirstModifierVirtualKeyCode);
        PressKey(aSecondModifierVirtualKeyCode);
        PressKey(aVirtualKeyCode);
        ReleaseKey(aVirtualKeyCode);
        ReleaseKey(aSecondModifierVirtualKeyCode);
        ReleaseKey(aFirstModifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that simulates holding down of two modifier keys and pressing VirtualKeyCodes.
    /// </summary>
    /// <param name="aFirstModifierVirtualKeyCode"></param>
    /// <param name="aSecondModifierVirtualKeyCode"></param>
    /// <param name="aVirtualKeyCodeArray"></param>
    /// <visibility>public</visibility>
    public void ModifiedKeyStroke(VirtualKeyCode aFirstModifierVirtualKeyCode, VirtualKeyCode aSecondModifierVirtualKeyCode, params VirtualKeyCode[] aVirtualKeyCodeArray)
    {
        if (RunMode == 4)
        {
            return;
        }
        PressKey(aFirstModifierVirtualKeyCode);
        PressKey(aSecondModifierVirtualKeyCode);
        foreach (VirtualKeyCode aVirtualKeyCode in aVirtualKeyCodeArray)
        {
            PressKey(aVirtualKeyCode);
            ReleaseKey(aVirtualKeyCode);
        }
        ReleaseKey(aSecondModifierVirtualKeyCode);
        ReleaseKey(aFirstModifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that sends input to simulate a single key press event.
    /// </summary>
    /// <param name="aVirtualKeyCode"></param>
    /// <visibility>public</visibility>
    public void PressKey(VirtualKeyCode aVirtualKeyCode)
    {
        if (RunMode == 4)
        {
            return;
        }
        Input[] array = new Input[1];
        array[0].Type = 1u;
        array[0].Data.Keyboard.KeyCode = (ushort)aVirtualKeyCode;
        array[0].Data.Keyboard.Scan = 0;
        array[0].Data.Keyboard.Flags = 0u;
        array[0].Data.Keyboard.Time = 0u;
        array[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
        SendKeyEvent(array);
    }

    /// <summary>
    /// Method that sends input to simulate a single key release event.
    /// </summary>
    /// <param name="aVirtualKeyCode"></param>
    /// <visibility>public</visibility>
    public void ReleaseKey(VirtualKeyCode aVirtualKeyCode)
    {
        if (RunMode == 4)
        {
            return;
        }
        Input[] array = new Input[1];
        array[0].Type = 1u;
        array[0].Data.Keyboard.KeyCode = (ushort)aVirtualKeyCode;
        array[0].Data.Keyboard.Scan = 0;
        array[0].Data.Keyboard.Flags = 2u;
        array[0].Data.Keyboard.Time = 0u;
        array[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
        SendKeyEvent(array);
    }

    /// <summary>
    /// Method called to send the Input in using SendInput.
    /// </summary>
    /// <param name="input"></param>
    /// <visibility>private</visibility>
    private static void SendKeyEvent(Input[] input)
    {
        uint num = SafeNativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(typeof(Input)));
    }
}