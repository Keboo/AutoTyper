# GitHub Copilot Instructions for AutoTyper

## Project Overview

AutoTyper is a Windows-only keyboard automation tool that allows users to automatically type text from the clipboard or predefined snippets. The project consists of:

- **AutoTyper CLI**: A command-line application that types clipboard content into the active window
- **AutoTyper.UI**: A WPF-based graphical interface for managing and executing text snippets
- **AutoTyper.DeviceEmulator**: A shared library for low-level keyboard input simulation using Windows APIs

## Technology Stack

- **.NET**: Version 10.0
- **Target Platform**: Windows only (`net10.0-windows`)
- **UI Framework**: WPF (Windows Presentation Foundation)
- **CLI Framework**: System.CommandLine
- **Testing**: xUnit with Moq and Moq.AutoMock
- **MVVM**: CommunityToolkit.Mvvm for observable objects and commands
- **Package Management**: Central Package Management (CPM) via `Directory.Packages.props`

## Project Structure

```
AutoTyper/
├── AutoTyper/                      # CLI application (dotnet global tool)
├── AutoTyper.UI/                   # WPF desktop application
│   ├── Services/                   # Application services (storage, typing, theme)
│   ├── ViewModels/                 # MVVM view models
│   └── Models/                     # Data models (e.g., Snippet)
├── AutoTyper.DeviceEmulator/       # Shared library for keyboard/mouse simulation
│   └── Native/                     # P/Invoke declarations for Windows APIs
├── AutoTyper.Tests/                # Unit tests for CLI
└── AutoTyper.UI.Tests/             # Unit tests for UI application
```

## Build and Test Commands

### Building
```bash
dotnet restore
dotnet build --configuration Release --no-restore
```

### Testing
```bash
dotnet test --configuration Release --no-build
```

### Packaging
```bash
# CLI tool as NuGet package
dotnet pack --configuration Release --no-build -o ./NuGet

# UI application with Velopack
dotnet publish AutoTyper.UI/AutoTyper.UI.csproj -c Release --self-contained -r win-x64 -o ./publish
dotnet vpk pack --packId Keboo.AutoTyper --packVersion <version> --packDir ./publish --mainExe AutoTyper.UI.exe
```

## Code Style and Conventions

### EditorConfig Settings
- **C# files**: 4 spaces indentation, CRLF line endings
- **XAML files**: 2 spaces indentation
- **MSBuild files** (`.csproj`, `.targets`, `.props`): 2 spaces indentation
- **Usings**: System directives first, separated from other usings
- **C# Language Version**: 14 (latest features enabled)
- **Nullable Reference Types**: Enabled for all projects
- **Implicit Usings**: Enabled

### Naming Conventions
- Use PascalCase for public members
- Use camelCase with underscore prefix (`_fieldName`) for private fields
- Prefer language keywords over BCL types (e.g., `int` not `Int32`)
- Do not use `this.` qualifier unless necessary

### Code Patterns

#### MVVM Pattern (UI Project)
- ViewModels inherit from `ObservableObject` (CommunityToolkit.Mvvm)
- Use `[ObservableProperty]` attribute for bindable properties
- Use `[RelayCommand]` or `AsyncRelayCommand` for commands
- Use `[NotifyCanExecuteChangedFor]` to link properties with command execution state

Example:
```csharp
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSnippetCommand))]
    private bool _isExecuting;
    
    [RelayCommand(CanExecute = nameof(CanExecuteSnippet))]
    private async Task ExecuteSnippet(Snippet snippet)
    {
        // Implementation
    }
}
```

#### Testing Patterns
- Use xUnit for all tests
- Use Moq.AutoMock (`AutoMocker`) for dependency injection in tests
- Test file naming: `{ClassName}Tests.cs`
- Test method naming: `{MethodName}_{Scenario}_{ExpectedBehavior}`
- Use `[ConstructorTests]` attribute to automatically generate null-check tests for constructors

Example:
```csharp
[ConstructorTests(typeof(MainWindowViewModel))]
public partial class MainWindowViewModelTests
{
    [Fact]
    public async Task AddSnippet_AddsToCollection()
    {
        // Arrange
        AutoMocker mocker = new();
        mocker.GetMock<IService>().Setup(x => x.Method()).Returns(value);
        var sut = mocker.CreateInstance<ClassName>();
        
        // Act
        await sut.MethodAsync();
        
        // Assert
        Assert.Equal(expected, actual);
    }
}
```

#### P/Invoke and Unsafe Code
- Windows API calls are centralized in `AutoTyper.DeviceEmulator/Native/` directory
- Use `[LibraryImport]` instead of `[DllImport]` for better performance
- Mark classes with P/Invoke as `partial`
- Allow unsafe code where needed for performance (e.g., string buffers with `stackalloc`)

Example:
```csharp
private static partial class NativeMethods
{
    [LibraryImport("user32.dll", SetLastError = true)]
    internal static partial IntPtr GetForegroundWindow();
}
```

## Dependencies

All package versions are managed centrally in `Directory.Packages.props`. To add or update a package:

1. Add/update the version in `Directory.Packages.props`
2. Reference it in the project file without a version number:
   ```xml
   <PackageReference Include="PackageName" />
   ```

## Key Architectural Decisions

1. **Windows-Only**: The application uses Windows-specific APIs and will not run on other platforms
2. **Unsafe Code**: Allowed for performance-critical operations (e.g., keyboard input simulation)
3. **Central Package Management**: All NuGet package versions controlled in one location
4. **Global Tool**: CLI application is packaged as a .NET global tool
5. **Source Generators**: CommunityToolkit.Mvvm uses source generators for MVVM pattern implementation

## Common Tasks

### Adding a New Snippet Property
1. Add property to `AutoTyper.UI/Models/Snippet.cs`
2. Update `SnippetStorageService` serialization if needed
3. Update UI in `MainWindow.xaml` and related views
4. Add tests in `AutoTyper.UI.Tests/MainWindowViewModelTests.cs`

### Adding a New Command-Line Option
1. Add option to `Program.GetRootCommand()` in `AutoTyper/Program.cs`
2. Handle the option in the command action handler
3. Add tests in `AutoTyper.Tests/ProgramTests.cs`

### Working with Windows APIs
1. Add P/Invoke declarations to appropriate file in `AutoTyper.DeviceEmulator/Native/`
2. Use `[LibraryImport]` attribute with appropriate marshalling
3. Mark class as `partial`
4. Test on Windows platform

## CI/CD

- **Build Workflow**: `.github/workflows/build-and-deploy.yml`
  - Runs on `windows-latest` runner (required for Windows-specific builds)
  - Steps: restore → build → test → pack → publish
  - Code coverage reports generated with ReportGenerator
- **Auto-merge**: Dependabot PRs are automatically merged if checks pass
- **Deployment**: NuGet packages pushed to nuget.org on main branch commits

## Important Notes

- This project **requires Windows** to build and run due to WPF and Windows API dependencies
- Tests must be run on Windows
- Use `EnableWindowsTargeting` property for cross-platform tooling compatibility
- The CLI tool can be installed globally: `dotnet tool install -g AutoTyper`
