# AutoTyper.DeviceEmulator

This project is a converted version of the Henooh.DeviceEmulator library (v1.1.8).  
It has been decompiled and brought into the AutoTyper solution to fix bugs and add custom functionality.

## What is this?

AutoTyper.DeviceEmulator provides keyboard and mouse control and observation capabilities for Windows applications.
It offers:

- **KeyboardController**: Control keyboard input programmatically
- **MouseController**: Control mouse input and movement
- **KeyboardObserver**: Monitor keyboard events globally or per-application  
- **MouseObserver**: Monitor mouse events globally or per-application

## Why was this created?

The original Henooh.DeviceEmulator library (NuGet package) has several bugs and the source code is not publicly available.
By decompiling it and bringing it into our own project, we can:

1. Fix existing bugs
2. Add custom functionality needed by AutoTyper
3. Have full control over the codebase
4. Remove external dependency on an unmaintained package

## Structure

```
AutoTyper.DeviceEmulator/
├── AutoTyper.DeviceEmulator.csproj
├── KeyboardController.cs
├── KeyboardObserver.cs  
├── MouseController.cs
├── MouseObserver.cs
├── A_NamespaceDoc.cs
└── Native/
    ├── AppMouseStruct.cs
    ├── AppObserver.cs
    ├── BaseController.cs
    ├── BaseObserver.cs
    ├── GlobalObserver.cs
    ├── HardwareInput.cs
    ├── Input.cs
    ├── InputType.cs
    ├── KeybdInput.cs
    ├── Keyboard.cs
    ├── KeyboardFlag.cs
    ├── KeyEventArgsExt.cs
    ├── KeyPressEventArgsExt.cs
    ├── Messages.cs
    ├── MouseButton.cs
    ├── MouseEventExtArgs.cs
    ├── MouseFlag.cs
    ├── MouseInput.cs
    ├── MouseKeybdHardwareInput.cs
    ├── MouseStruct.cs
    ├── ObserverAbstract.cs
    ├── SafeNativeMethods.cs
    ├── VirtualKeyCode.cs
    └── A_NamespaceDoc.cs
```

## Namespace Changes

All instances of `Henooh.DeviceEmulator` have been changed to `AutoTyper.DeviceEmulator`.  
All instances of `Henooh.DeviceEmulator.Native` have been changed to `AutoTyper.DeviceEmulator.Native`.

## Original Library Information

- **Original Package**: Henooh.DeviceEmulator v1.1.8
- **Original Namespace**: Henooh.DeviceEmulator  
- **Target Framework**: Originally .NET Framework 4.5, now targeting net10.0-windows

## License

The original library's license terms apply. This is a derived work for fixing bugs in an otherwise unavailable codebase.
