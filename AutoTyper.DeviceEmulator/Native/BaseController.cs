namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides structures used by both KeyboardController and MouseController. 
/// </summary>
/// <remarks>
/// Contains two structure fields.
/// </remarks>
/// <visibility>public</visibility>
public abstract class BaseController
{
    /// <summary>
    /// Buffer of <see cref="T:AutoTyper.DeviceEmulator.Native.Input" />.
    /// </summary>
    /// <visibility>internal</visibility>
    internal Input inputBuffer;

    /// <summary>
    /// List of <see cref="T:AutoTyper.DeviceEmulator.Native.Input" />.
    /// </summary>
    /// <visibility>internal</visibility>
    internal List<Input> inputList;
}