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
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2017-01-07  AS00810  v1.00.06.002  Initial Version
/// 2017-02-28  AS00843  v1.00.06.012  Modify summary, remarks, add XML comments to all enumeration values
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
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