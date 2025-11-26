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

    /// <summary>
    /// Provides access to CancellationToken.
    /// </summary>
    /// <visibility>private</visibility>
    private CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Allows the library to run in three different modes.
    /// 0 - Normal: Will run normally.
    /// 4 - Suppress: Will prevent sending any SendInput.
    /// </summary>
    public int RunMode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.Native.BaseController" /> with no arguments.
    /// </summary>
    /// <visibility>protected</visibility>
    protected BaseController()
    {
    }

    /// <summary>
    /// Default constructor with CancellationToken.
    /// </summary>
    /// <param name="aCancellationToken"></param>
    /// <visibility>protected</visibility>
    protected BaseController(CancellationToken aCancellationToken)
    {
        CancellationToken = aCancellationToken;
    }

    /// <summary>
    /// Provides Sleep cycle that can be cancelled with CancellationToken.
    /// </summary>
    /// <visibility>public</visibility>
    protected async Task DelayAsync(TimeSpan timeout)
    {
        await Task.Delay(timeout, CancellationToken);
    }
}