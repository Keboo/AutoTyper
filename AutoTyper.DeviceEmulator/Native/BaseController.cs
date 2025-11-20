using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides structures used by both KeyboardContoller and MouseController. 
/// </summary>
/// <remarks>
/// Contains two structure fields.
/// </remarks>
/// <visibility>public</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2016-08-29  AS00716  v1.00.03.022  Initial Version
/// 2016-10-15  AS00750  v1.00.03.037  Combined InputBuffer and InputList from Controller to BaseController
/// 2016-10-18  AS00753  v1.00.03.039  Added summary and remarks, added visibility xml tag to class
/// 2016-10-30  AS00765  v1.00.04.003  Implement CancellationToken on BaseController
/// 2016-11-01  AS00767  v1.00.04.004  Changed the logic of Sleep method to account for fractional intervals
/// 2016-11-22  AS00783  v1.00.04.011  Added comments to methods
/// 2016-11-24  AS00785  v1.00.05.001  Removed Console output
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2017-11-10  AS00952  v1.01.01.004  Sleep method is now asynchronous and uses Task.Delay instead of Thread.Sleep
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2018-12-17  AS01131  v1.01.03.003  Add visibility tags to XML comments
/// 2019-01-30  AS01143  v1.01.03.005  Modify the layout of the code, add XML header comments to all properties
/// </revisionhistory>
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
    public void Sleep(TimeSpan aTimeout)
    {
        int num = (int)aTimeout.TotalMilliseconds;
        while (num > 0)
        {
            if (num > 100)
            {
                Task.Delay(100).Wait();
                num -= 100;
            }
            else
            {
                Task.Delay(num).Wait();
                num = 0;
            }
            if (CancellationToken.IsCancellationRequested)
            {
                CancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    /// <summary>
    /// Provides Sleep cycle that can be cancelled with CancellationToken.
    /// </summary>
    /// <param name="aMillisecondTimeout"></param>
    /// <visibility>public</visibility>
    public void Sleep(int aMillisecondTimeout)
    {
        Sleep(new TimeSpan(0, 0, 0, 0, aMillisecondTimeout));
    }
}