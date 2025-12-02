using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AutoTyper.DeviceEmulator.Native;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// KeyboardController provides methods to type text and keys. 
/// </summary>
/// <remarks>
/// KeyboardController provides methods that help simulate typing method,  including method that works like 
/// a human types. 
/// </remarks>
public class KeyboardController : BaseController
{
    /// <summary>
    /// Returns a dictionary that contains the char key as the key and VirtualKeyCode as value.
    /// </summary>
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
        { '\t', VirtualKeyCode.TAB },
        // Punctuation - unshifted
        { ';', VirtualKeyCode.OEM_1 },
        { '=', VirtualKeyCode.OEM_PLUS },
        { ',', VirtualKeyCode.OEM_COMMA },
        { '-', VirtualKeyCode.OEM_MINUS },
        { '.', VirtualKeyCode.OEM_PERIOD },
        { '/', VirtualKeyCode.OEM_2 },
        { '`', VirtualKeyCode.OEM_3 },
        { '[', VirtualKeyCode.OEM_4 },
        { '\\', VirtualKeyCode.OEM_5 },
        { ']', VirtualKeyCode.OEM_6 },
        { '\'', VirtualKeyCode.OEM_7 },
        // Punctuation - shifted (require Shift key)
        { ':', VirtualKeyCode.OEM_1 },
        { '+', VirtualKeyCode.OEM_PLUS },
        { '<', VirtualKeyCode.OEM_COMMA },
        { '_', VirtualKeyCode.OEM_MINUS },
        { '>', VirtualKeyCode.OEM_PERIOD },
        { '?', VirtualKeyCode.OEM_2 },
        { '~', VirtualKeyCode.OEM_3 },
        { '{', VirtualKeyCode.OEM_4 },
        { '|', VirtualKeyCode.OEM_5 },
        { '}', VirtualKeyCode.OEM_6 },
        { '"', VirtualKeyCode.OEM_7 },
        // Shifted numbers
        { ')', VirtualKeyCode.VK_0 },
        { '!', VirtualKeyCode.VK_1 },
        { '@', VirtualKeyCode.VK_2 },
        { '#', VirtualKeyCode.VK_3 },
        { '$', VirtualKeyCode.VK_4 },
        { '%', VirtualKeyCode.VK_5 },
        { '^', VirtualKeyCode.VK_6 },
        { '&', VirtualKeyCode.VK_7 },
        { '*', VirtualKeyCode.VK_8 },
        { '(', VirtualKeyCode.VK_9 }
    };

    /// <summary>
    /// The minimum time needed between key presses.
    /// </summary>
    public TimeSpan MinimumKeyPressInterval { get; set; } = TimeSpan.FromMilliseconds(20);

    /// <summary>
    /// Method that sends input to press and release the VirtualKeyCode. 
    /// </summary>
    /// <param name="virtualKeyCode"></param>
    /// <param name="cancellationToken"></param>
    public async Task TypeAsync(VirtualKeyCode virtualKeyCode, CancellationToken cancellationToken)
    {
        PressKey(virtualKeyCode);
        await Task.Delay(MinimumKeyPressInterval, cancellationToken);
        ReleaseKey(virtualKeyCode);
    }

    /// <summary>
    /// Method that sends inputs to type an array of VirtualKeyCodes.
    /// </summary>
    /// <param name="cancelationToken"></param>
    /// <param name="virtualKeyCodeArray"></param>
    public async Task TypeAsync(CancellationToken cancelationToken, params VirtualKeyCode[] virtualKeyCodeArray)
    {
        foreach (VirtualKeyCode aVirtualKeyCode in virtualKeyCodeArray)
        {
            PressKey(aVirtualKeyCode);
            await Task.Delay(MinimumKeyPressInterval, cancelationToken);
            ReleaseKey(aVirtualKeyCode);
        }
    }

    /// <summary>
    /// Method that sends inputs to type a given string.
    /// </summary>
    /// <param name="string">The string to type</param>
    /// <param name="cancellationToken"></param>
    public Task TypeStringAsync(string @string, CancellationToken cancellationToken)
    {
        return TypeStringAsync(@string, TimeSpan.Zero, TimeSpan.Zero, cancellationToken);
    }

    /// <summary>
    /// Method that sends inputs to type a given string with given interval.
    /// </summary>
    /// <param name="string">The string to type</param>
    /// <param name="keyStrokeDelay">The time between keystrokes</param>
    /// <param name="cancellationToken"></param>
    public Task TypeStringAsync(string @string, TimeSpan keyStrokeDelay, CancellationToken cancellationToken)
    {
        return TypeStringAsync(@string, keyStrokeDelay, TimeSpan.Zero, cancellationToken);
    }

    /// <summary>
    /// Method that sends inputs to type a given string with natural typing interval.
    /// </summary>
    /// <param name="string"></param>
    /// <param name="cancellationToken"></param>
    public Task TypeStringNaturallyAsync(string @string, CancellationToken cancellationToken)
    {
        return TypeStringAsync(@string, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(20), cancellationToken);
    }


    /// <summary>
    /// Method that sends inputs to type a given string.
    /// </summary>
    /// <param name="string"></param>
    /// <param name="keystrokeDelay">The amount of time between keystrokes</param>
    /// <param name="keystrokeVariance">The amount of variance allowed between key stroke delay.</param>
    /// <param name="cancellationToken"></param>
    public async Task TypeStringAsync(string @string, TimeSpan keystrokeDelay, TimeSpan keystrokeVariance, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(@string))
        {
            return;
        }
        
        // Normalize line endings to prevent \r\n from being typed as two separate newlines
        @string = @string.Replace("\r\n", "\n");
        
        int characterIndex = 0;
        char[] array = @string.ToCharArray();
        foreach (char c in array)
        {
            characterIndex++;
            await TypeCharAsync(c, RequiresShift(c), cancellationToken);
            
            if (characterIndex < @string.Length)
            {
                TimeSpan delay = keystrokeDelay;
                if (keystrokeVariance != TimeSpan.Zero)
                {
                    int milliseconds = (int)Math.Round(keystrokeVariance.TotalMilliseconds);
                    delay += TimeSpan.FromMilliseconds(Random.Shared.Next(-milliseconds, milliseconds));
                }
                
                // Ensure minimum delay between characters to prevent dropped keystrokes
                if (delay < MinimumKeyPressInterval)
                {
                    delay = MinimumKeyPressInterval;
                }
                
                await Task.Delay(delay, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Determines if a character requires the Shift key to be pressed.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>True if Shift is required, false otherwise.</returns>
    private static bool RequiresShift(char c)
    {
        // Uppercase letters
        if (char.IsUpper(c))
            return true;

        // Shifted punctuation and symbols (US keyboard layout)
        return c switch
        {
            '!' or '@' or '#' or '$' or '%' or '^' or '&' or '*' or '(' or ')' => true, // Shifted numbers
            ':' or '+' or '<' or '_' or '>' or '?' or '~' or '{' or '|' or '}' or '"' => true, // Shifted punctuation
            _ => false
        };
    }

    /// <summary>
    /// Method that sends inputs to type a char.
    /// </summary>
    /// <param name="char"></param>
    /// <param name="shiftDown"></param>
    /// <param name="cancellationToken"></param>
    public async Task TypeCharAsync(char @char, bool shiftDown, CancellationToken cancellationToken)
    {
        char key = char.ToLower(@char);
        if (!VirtualKeyCodeDictionary.TryGetValue(key, out var value))
        {
            return;
        }
        if (shiftDown)
        {
            PressKey(VirtualKeyCode.SHIFT);
        }
        PressKey(value);
        await Task.Delay(MinimumKeyPressInterval, cancellationToken);
        ReleaseKey(value);
        if (shiftDown)
        {
            ReleaseKey(VirtualKeyCode.SHIFT);
        }
    }


    /// <summary>
    /// Method that simulates holding down of modifier key and pressing VirtualKeyCode.
    /// </summary>
    /// <param name="modifierVirtualKeyCode"></param>
    /// <param name="virtualKeyCode"></param>
    public static void ModifiedKeyStroke(VirtualKeyCode modifierVirtualKeyCode, VirtualKeyCode virtualKeyCode)
    {
        PressKey(modifierVirtualKeyCode);
        PressKey(virtualKeyCode);
        ReleaseKey(virtualKeyCode);
        ReleaseKey(modifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that simulates holding down of modifier key and pressing VirtualKeyCode.
    /// </summary>
    /// <param name="modifierVirtualKeyCode"></param>
    /// <param name="virtualKeyCodes"></param>
    public static void ModifiedKeyStroke(VirtualKeyCode modifierVirtualKeyCode, params VirtualKeyCode[] virtualKeyCodes)
    {
        PressKey(modifierVirtualKeyCode);
        foreach (VirtualKeyCode aVirtualKeyCode in virtualKeyCodes)
        {
            PressKey(aVirtualKeyCode);
            ReleaseKey(aVirtualKeyCode);
        }
        ReleaseKey(modifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that simulates holding down of two modifier keys and pressing VirtualKeyCode.
    /// </summary>
    /// <param name="firstModifierVirtualKeyCode"></param>
    /// <param name="secondModifierVirtualKeyCode"></param>
    /// <param name="virtualKeyCode"></param>
    public static void ModifiedKeyStroke(VirtualKeyCode firstModifierVirtualKeyCode, VirtualKeyCode secondModifierVirtualKeyCode, VirtualKeyCode virtualKeyCode)
    {
        PressKey(firstModifierVirtualKeyCode);
        PressKey(secondModifierVirtualKeyCode);
        PressKey(virtualKeyCode);
        ReleaseKey(virtualKeyCode);
        ReleaseKey(secondModifierVirtualKeyCode);
        ReleaseKey(firstModifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that simulates holding down of two modifier keys and pressing VirtualKeyCodes.
    /// </summary>
    /// <param name="firstModifierVirtualKeyCode"></param>
    /// <param name="secondModifierVirtualKeyCode"></param>
    /// <param name="virtualKeyCodes"></param>
    public static void ModifiedKeyStroke(VirtualKeyCode firstModifierVirtualKeyCode, VirtualKeyCode secondModifierVirtualKeyCode, params VirtualKeyCode[] virtualKeyCodes)
    {
        PressKey(firstModifierVirtualKeyCode);
        PressKey(secondModifierVirtualKeyCode);
        foreach (VirtualKeyCode aVirtualKeyCode in virtualKeyCodes)
        {
            PressKey(aVirtualKeyCode);
            ReleaseKey(aVirtualKeyCode);
        }
        ReleaseKey(secondModifierVirtualKeyCode);
        ReleaseKey(firstModifierVirtualKeyCode);
    }

    /// <summary>
    /// Method that sends input to simulate a single key press event.
    /// </summary>
    /// <param name="virtualKeyCode"></param>
    public static void PressKey(VirtualKeyCode virtualKeyCode)
    {
        Input[] array = new Input[1];
        array[0].Type = 1u;
        array[0].Data.Keyboard.KeyCode = (ushort)virtualKeyCode;
        array[0].Data.Keyboard.Scan = 0;
        array[0].Data.Keyboard.Flags = 0u;
        array[0].Data.Keyboard.Time = 0u;
        array[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
        SendKeyEvent(array);
    }

    /// <summary>
    /// Method that sends input to simulate a single key release event.
    /// </summary>
    /// <param name="virtualKeyCode"></param>
    public static void ReleaseKey(VirtualKeyCode virtualKeyCode)
    {
        Input[] array = new Input[1];
        array[0].Type = 1u;
        array[0].Data.Keyboard.KeyCode = (ushort)virtualKeyCode;
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
    private static void SendKeyEvent(Input[] input)
    {
        uint _ = SafeNativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf<Input>());
    }
}