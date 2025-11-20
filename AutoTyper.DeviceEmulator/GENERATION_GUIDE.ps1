# Complete File Generation Script
# This script should be run AFTER the MCP decompiler server is loaded with Henooh.DeviceEmulator

Write-Host "AutoTyper.DeviceEmulator File Generation Helper" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

Write-Host "The decompiler MCP server should have Henooh.DeviceEmulator v1.1.8 loaded."
Write-Host "You need to use the MCP decompiler tools to fetch each type's source code."
Write-Host ""

$types = @(
    @{ Name = "KeyboardController"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000003:T"; Path = "KeyboardController.cs" },
    @{ Name = "KeyboardObserver"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000005:T"; Path = "KeyboardObserver.cs" },
    @{ Name = "MouseController"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000004:T"; Path = "MouseController.cs" },
    @{ Name = "MouseObserver"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000006:T"; Path = "MouseObserver.cs" },
    @{ Name = "A_NamespaceDoc"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000002:T"; Path = "A_NamespaceDoc.cs" },
    @{ Name = "AppMouseStruct"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200001D:T"; Path = "Native\AppMouseStruct.cs" },
    @{ Name = "AppObserver"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200000A:T"; Path = "Native\AppObserver.cs" },
    @{ Name = "BaseController"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000007:T"; Path = "Native\BaseController.cs" },
    @{ Name = "BaseObserver"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200000B:T"; Path = "Native\BaseObserver.cs" },
    @{ Name = "GlobalObserver"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200000C:T"; Path = "Native\GlobalObserver.cs" },
    @{ Name = "HardwareInput"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200000D:T"; Path = "Native\HardwareInput.cs" },
    @{ Name = "Input"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000010:T"; Path = "Native\Input.cs" },
    @{ Name = "InputType"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000011:T"; Path = "Native\InputType.cs" },
    @{ Name = "KeybdInput"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000012:T"; Path = "Native\KeybdInput.cs" },
    @{ Name = "Keyboard"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000013:T"; Path = "Native\Keyboard.cs" },
    @{ Name = "KeyboardFlag"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000014:T"; Path = "Native\KeyboardFlag.cs" },
    @{ Name = "KeyEventArgsExt"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000016:T"; Path = "Native\KeyEventArgsExt.cs" },
    @{ Name = "KeyPressEventArgsExt"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000015:T"; Path = "Native\KeyPressEventArgsExt.cs" },
    @{ Name = "Messages"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000017:T"; Path = "Native\Messages.cs" },
    @{ Name = "MouseButton"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000008:T"; Path = "Native\MouseButton.cs" },
    @{ Name = "MouseEventExtArgs"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000018:T"; Path = "Native\MouseEventExtArgs.cs" },
    @{ Name = "MouseFlag"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000019:T"; Path = "Native\MouseFlag.cs" },
    @{ Name = "MouseInput"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200001A:T"; Path = "Native\MouseInput.cs" },
    @{ Name = "MouseKeybdHardwareInput"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200001B:T"; Path = "Native\MouseKeybdHardwareInput.cs" },
    @{ Name = "MouseStruct"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200001C:T"; Path = "Native\MouseStruct.cs" },
    @{ Name = "ObserverAbstract"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200000F:T"; Path = "Native\ObserverAbstract.cs" },
    @{ Name = "SafeNativeMethods"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:02000009:T"; Path = "Native\SafeNativeMethods.cs" },
    @{ Name = "VirtualKeyCode"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200001E:T"; Path = "Native\VirtualKeyCode.cs" },
    @{ Name = "Native.A_NamespaceDoc"; MemberId = "acd3faeec2764099bb360d8de6e0fdd4:0200000E:T"; Path = "Native\A_NamespaceDoc.cs" }
)

Write-Host "Files to create: $($types.Count)" -ForegroundColor Yellow
Write-Host ""

$baseDir = "d:\Dev\AutoTyper\AutoTyper.DeviceEmulator"

foreach ($type in $types) {
    $fullPath = Join-Path $baseDir $type.Path
    Write-Host "[ ] $($type.Name.PadRight(30)) -> $($type.Path)" -ForegroundColor Gray
    Write-Host "    MemberId: $($type.MemberId)" -ForegroundColor DarkGray
}

Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. Use GitHub Copilot or MCP decompiler to fetch each type's source"
Write-Host "2. Replace 'Henooh.DeviceEmulator' with 'AutoTyper.DeviceEmulator' in the source"
Write-Host "3. Save each file to the path shown above"
Write-Host ""
Write-Host "Alternatively, ask GitHub Copilot to:"
Write-Host "  'Use the MCP decompiler to get source for each member ID and create the files'"
