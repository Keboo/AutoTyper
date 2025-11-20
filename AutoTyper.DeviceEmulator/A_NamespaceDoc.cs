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
/// <item>Introduce RunMode, when set to 4, suppresses SendInput.</item>
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
[CompilerGenerated]
internal abstract class A_NamespaceDoc
{
}