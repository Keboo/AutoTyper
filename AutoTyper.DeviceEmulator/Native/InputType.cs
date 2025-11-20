namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Specifies the type of the input event. This member can be one of the following values. 
/// </summary>
/// <remarks>
/// Contains enumeration of Hardware devices, Mouse as 0, Keyboard as 1.
/// </remarks>
/// <visibility>internal</visibility>
internal enum InputType : uint
{
    /// <summary>
    /// INPUT_MOUSE = 0x00 (The event is a mouse event. Use the mi structure of the union.)
    /// </summary>
    Mouse,
    /// <summary>
    /// INPUT_KEYBOARD = 0x01 (The event is a keyboard event. Use the ki structure of the union.)
    /// </summary>
    Keyboard,
    /// <summary>
    /// INPUT_HARDWARE = 0x02 (Windows 95/98/Me: The event is from input hardware other than a keyboard or mouse. Use the hi structure of the union.)
    /// </summary>
    Hardware
}