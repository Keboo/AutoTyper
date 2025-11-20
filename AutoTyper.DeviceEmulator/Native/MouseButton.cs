namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Contains information about a different set of mouse buttons.
/// </summary>
/// <remarks>
/// This method is to simplify when a button on a mouse is interacted.
/// Main reason for this method was included when a need to simplify when left and right mouse buttons are
/// pressed for movements.
/// </remarks>
/// <visibility>public</visibility>
public enum MouseButton
{
    /// <summary>
    /// Represents the left mouse button.
    /// </summary>
    Left,
    /// <summary>
    /// Represents the right mouse button.
    /// </summary>
    Right,
    /// <summary>
    /// Represents the middle mouse button.
    /// </summary>
    Middle,
    /// <summary>
    /// Represents the X mouse button.
    /// </summary>
    XButton,
    /// <summary>
    /// Represents the combination of the left and the right mouse buttons.
    /// </summary>
    LeftAndRight
}