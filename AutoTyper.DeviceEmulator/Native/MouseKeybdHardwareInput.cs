using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides combined/overlayed structure that includes Mouse, Keyboard and Hardware Input message data.
/// </summary>
/// <remarks>
/// MOUSEKEYBDHARDWAREINPUT is struct layout of Mouse, Keyboard and Hardware Input message data.
/// (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
/// </remarks>
/// <visibility>internal</visibility>
[StructLayout(LayoutKind.Explicit)]
internal struct MouseKeybdHardwareInput
{
    /// <summary>
    /// The <see cref="T:AutoTyper.DeviceEmulator.Native.MouseInput" /> definition.
    /// </summary>
    /// <visibility>internal</visibility>
    [FieldOffset(0)]
    internal MouseInput Mouse;

    /// <summary>
    /// The <see cref="T:AutoTyper.DeviceEmulator.Native.KeybdInput" /> definition.
    /// </summary>
    /// <visibility>internal</visibility>
    [FieldOffset(0)]
    internal KeybdInput Keyboard;

    /// <summary>
    /// The <see cref="T:AutoTyper.DeviceEmulator.Native.HardwareInput" /> definition.
    /// </summary>
    /// <visibility>internal</visibility>
    [FieldOffset(0)]
    internal HardwareInput Hardware;

    /// <summary>
    /// Provide string output of the <see cref="T:AutoTyper.DeviceEmulator.Native.MouseKeybdHardwareInput" /> content.
    /// </summary>
    /// <returns></returns>
    /// <visibility>public</visibility>
    public override string ToString()
    {
        return Mouse.ToString() + Keyboard.ToString() + Hardware;
    }
}