using System.Runtime.CompilerServices;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// The AutoTyper.DeviceEmulator namespace provides a simple .NET (C#) interface to emulate Keyboard and Mouse input
/// and status. Input classes are called Controllers and Status classes are called Observers.
/// </summary>
/// <remarks>
/// Using Windows Forms or Windows WPF SendKey methods can emulate text entry, but not actual key strokes.
/// Other available input simulators or emulators lack ability to send key strokes that is behind DirectX layers,
/// and rarely provides output features as one package. HenoohDeviceEmulator provides easy way to gain hardware
/// access to Keyboard and Mouse devices, both input and output. Henooh Device Emulator is composed of four
/// separate classes each designed to handle their own functionality.
/// <dl class="dl-horizontal">
/// <dt>KeyboardController</dt>
/// <dd>Provides Keyboard Controlling features (Control SendInput to keyboard device)</dd>
/// <dt>MouseController</dt>
/// <dd>Provides Mouse Input features (Control SendInput to mouse device)</dd>
/// <dt>KeyboardObserver</dt>
/// <dd>Provides Keyboard Observing features (Receive events from a keyboard device)</dd>
/// <dt>MouseObserver</dt>
/// <dd>Provides Mouse Observing features (Receive events from a mouse device)</dd>
/// </dl>
///
/// What's new in v1.1.8 of Henooh.DeviceEmulator.
/// <list type="bullet">
/// <item>Removed globalization exception messages.</item>
/// <item>Added new property that made it simple to distinguish between Logical and Physical coordinates.</item>
/// </list>
///
/// What's new in v1.1.7 of Henooh.DeviceEmulator.
/// <list type="bullet">
/// <item>Move method uses the relative positioning to give realistic mouse movements.</item>
/// </list>
///
/// What's new in v1.1.6 of Henooh.DeviceEmulator.
/// <list type="bullet">
/// <item>Fixed a bug that the resource libraries are added as content.</item>
/// </list>
///
/// What's new in v1.1.5 of Henooh.DeviceEmulator.
/// <list type="bullet">
/// <item>Fixed an with null parameters in Keyboard.Controller.</item>
/// </list>
///
/// What's new in v1.1.4 of Henooh.DeviceEmulator.
/// <list type="bullet">
/// <item>Introduce RunMode, when set to 4, supresses SendInput.</item>
/// </list>
///
/// What's new in v1.01.03 of Henooh.DeviceEmulator.
/// <ul>
/// <li>Rename the namespace to Henooh.DeviceEmulator.</li>
/// </ul>
///
/// What's new in v1.01.02 of HenoohDeviceEmulator.
/// <ul>
/// <li>Press method on KeyboardController emulates repeated key presses of a physical keyboard device.</li>
/// </ul>
///
/// What's new in v1.01.01 of HenoohDeviceEmulator.
/// <ul>
/// <li>Added support for United Kingdom keyboard layout.</li>
/// </ul>
///
/// What's new in v1.01.00 of HenoohDeviceEmulator.
/// <ul>
/// <li>Fixed a bug that throws exception on observers when system uptime exceeds 24.5 days.</li>
/// </ul>
///
/// What's New in v1.00.06 of HenoohDeviceEmulator.
/// <ul>
/// <li>MouseController now handles two types of coordinate system based on scaling.</li>
/// <li>Added PhysicalToLogicalPoint method in MouseController.</li>
/// <li>Added LogicalToPhysicalPoint method in MouseController.</li>
/// <li>Added ShowAllScreenInfo method in MouseController.</li>
/// <li>Added LeftRightClickHold method in MouseController.</li>
/// </ul>
///
/// What's New in v1.00.05 of HenoohDeviceEmulator.
/// <ul>
/// <li>Supports CancellationToken for KeyboardController and MouseController.</li>
/// <li>Added Sleep() method, which responds to CancellationToken.</li>
/// </ul>
///
/// What's New in v1.00.04 of HenoohDeviceEmulator.
/// <ul>
/// <li>Added Press() method in KeyboardController - designate time delay between KeyDown and KeyUp</li>
/// <li>Added code samples to documentation - specifically to describe how to use Observers</li>
/// <li>Rewrote Dispose methods to properly dispose.</li>
/// <li>Optimized classes and native methods to be compatible with more hardware.</li>
/// <li>Improved comments and examples throughout many classes and methods.</li>
/// </ul>
/// </remarks>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-10-30  AS00549  v1.00.00.002  Initial Version
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-03  AS00553  v1.00.00.004  Modified the summary and remarks
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-07  AS00557  v1.00.01.000  New Release with project renamed and full documentation
/// 2015-11-11  AS00574  v1.00.01.011  Use description tags available in bootstrap to describe Namespace
/// 2016-01-10  AS00595  v1.00.02.000  Added default constructors, better documentations and general clean up
/// 2016-01-16  AS00601  v1.00.03.000  Fixed a bug that threw an exception with KeyboardObserver
/// 2016-02-23  AS00627  v1.00.03.004  Enable XML documentation generation for Debug
/// 2016-03-27  AS00656  v1.00.03.011  Removed Native.XButton.cs file
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-07-29  AS00697  v1.00.03.020  Modified remarks with up to date information in v1.00.04
/// 2016-10-23  AS00758  v1.00.04.000  New release to 1.00.04.000
/// 2016-10-24  AS00759  v1.00.04.001  Quick bug fix - in 1.00.04.000 - Released as 1.00.04.001
/// 2016-11-20  AS00781  v1.00.04.010  Removed unused using directives
/// 2016-11-22  AS00783  v1.00.04.011  Modified remarks in prepration of v1.00.05 release
/// 2016-11-23  AS00784  v1.00.05.000  New release to 1.00.05.000
/// 2016-12-10  AS00801  v1.00.05.008  Update remarks in prepration changes in 1.00.06 release
/// 2016-12-17  AS00803  v1.00.05.009  Update remarks in prepration changes in 1.00.06 release
/// 2016-12-29  AS00806  v1.00.06.000  New release to 1.00.06.000
/// 2017-02-18  AS00836  v1.00.06.009  Update remarks with critical changes
/// 2017-08-10  AS00892  v1.01.00.000  New release to 1.01.00.000
/// 2017-09-03  AS00907  v1.01.01.000  New release to 1.01.01.000
/// 2017-10-14  AS00935  v1.01.01.002  Added description to the AssemblyInfo
/// 2018-01-06  AS00975  v1.01.02.000  New release to 1.01.02.000
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2018-04-05  AS01042  v1.01.03.000  New release to 1.01.03.000
/// 2018-05-30  AS01070  v1.01.03.001  Change the AssemblyName to Henooh.DeviceEmulator
/// 2018-06-06  AS01074  v1.01.03.002  Modify the summary
/// 2019-03-28  AS01163  v1.01.04.000  New release to v1.1.4
/// 2019-04-11  AS01175  v1.01.04.005  Rename the class to A_NamespaceDoc
/// 2019-04-19  AS01181  v1.01.04.008  Use Microsoft.CodeAnalysis.FxCopAnalyzers v2.9.2 from Nuget
/// 2019-04-22  AS01182  v1.01.04.009  Add abstract prefix to class
/// 2019-08-09  AS01212  v1.01.04.015  Use Microsoft.CodeAnalysis.FxCopAnalyzers v2.9.4 from Nuget
/// 2019-08-12  AS01214  v1.01.04.016  Add a res/values/strings.resx resource file, set the neutural language
/// 2019-11-07  AS01246  v1.01.04.020  Update the summary
/// 2019-11-09  AS01247  v1.01.05.000  New release to v1.1.5
/// 2020-02-22  AS01286  v1.01.06.000  New release to v1.1.6 was not recorded in revision history
/// </revisionhistory>
[CompilerGenerated]
internal abstract class A_NamespaceDoc
{
}
