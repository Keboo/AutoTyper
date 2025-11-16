# AutoTyper.UI - Snippet Manager

A WPF application for managing and executing code/text snippets with automated typing functionality.

## Features

### Snippet Management
- **Create Snippets**: Add new text/code snippets with custom settings via inline dialog
- **Edit Snippets**: Modify existing snippets in the same overlay dialog
- **Delete Snippets**: Remove unwanted snippets
- **Persistent Storage**: All snippets are saved to disk as JSON

### Snippet Properties
Each snippet includes:
- **Name**: Display name for easy identification
- **Content**: The text to type (supports multiline)
- **Delay**: Time in seconds to wait before typing begins (default: 3.0s)
- **Fast Typing**: Toggle to remove delays between keystrokes
- **Append New Line**: Automatically press Enter after typing

### UI Features
- **Always on Top**: Toggle to keep window above other applications
- **Material Design Dialog**: Inline overlay dialog using MaterialDesignInXAML's DialogHost
- **Visual Feedback**: 
  - Countdown timer before typing begins
  - Status messages for all operations
  - Toast notifications (snackbar)
- **Material Design**: Modern, clean UI with Material Design themes
- **Keyboard Shortcuts**: 
  - `Ctrl+N` - Add new snippet

## Architecture

### MVVM Pattern
The application follows the Model-View-ViewModel (MVVM) pattern using **CommunityToolkit.Mvvm**:

- **Models**: `Snippet.cs` - Data model for snippets
- **ViewModels**: 
  - `MainWindowViewModel.cs` - Main window logic and dialog management
  - `AddSnippetViewModel.cs` - Dialog data binding for adding/editing snippets
- **Views**: 
  - `MainWindow.xaml` - Main application window with embedded DialogHost
- **Services**:
  - `TypingService.cs` - Handles keyboard automation
  - `SnippetStorageService.cs` - Manages snippet persistence

### Dependency Injection
Uses Microsoft.Extensions.DependencyInjection for service registration and lifetime management.

### Dialog Pattern
Uses MaterialDesignInXAML's **DialogHost** for inline overlay dialogs instead of separate windows, providing a more modern UX with:
- Overlay dimming effect
- Smooth animations
- Click-away-to-close functionality
- No window management overhead

## Usage

1. **Launch the Application**: Run AutoTyper.UI.exe
2. **Add a Snippet**:
   - Click "ADD SNIPPET" button or press `Ctrl+N`
   - Fill in the overlay dialog that appears
   - Enter a name and content
   - Configure delay, fast typing, and new line options
   - Click "SAVE" or "CANCEL"
3. **Execute a Snippet**:
   - Click on any snippet in the list
   - Focus the target window during the countdown
   - The snippet will be typed automatically
4. **Edit/Delete**:
   - Use the pencil icon to edit (opens overlay dialog)
   - Use the trash icon to delete

## Data Storage

Snippets are stored in: `%APPDATA%\AutoTyper\snippets.json`

## Dependencies

- **CommunityToolkit.Mvvm**: MVVM framework with source generators
- **MaterialDesignThemes**: Material Design UI components including DialogHost
- **Microsoft.Extensions.Hosting**: Dependency injection and hosting
- **Henooh.DeviceEmulator**: Keyboard input simulation

## Platform

Windows only - Uses Win32 APIs for window detection and keyboard simulation.
