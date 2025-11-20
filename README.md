# AutoTyper

AutoTyper is a Windows automation tool that simulates keyboard input to type text content. It's available as both a command-line application and a GUI application for managing text snippets.

## 🖥️ GUI Application

AutoTyper provides a user-friendly WPF application for managing and executing text snippets with automated typing.

### Download

**[Download AutoTyper GUI for Windows](https://api.velopack.io/v1/download/Keboo.AutoTyper/win)**

### Features

- **Snippet Management**: Create, edit, and delete text/code snippets
- **Automated Typing**: Execute snippets with configurable delays
- **Fast Typing Mode**: Remove delays between keystrokes for rapid input
- **Persistent Storage**: All snippets saved automatically
- **Modern UI**: Material Design interface with always-on-top option
- **Keyboard Shortcuts**: Quick access with `Ctrl+N` for new snippets

### Usage

1. Launch AutoTyper.UI.exe
2. Click "ADD SNIPPET" or press `Ctrl+N`
3. Enter snippet name and content
4. Configure typing options (delay, fast typing, append new line)
5. Click a snippet to execute - focus your target window during countdown

Snippets are stored in: `%APPDATA%\AutoTyper\snippets.json`

## 💻 Command-Line Application

A simple command-line tool to type the contents of your clipboard or specified text.

### Usage

```bash
# Type clipboard content after 3 second delay (default)
AutoTyper

# Type clipboard content after custom delay
AutoTyper --delay 5

# Type specific content
AutoTyper --content "Hello, World!"

# Type with fast typing (no delay between keystrokes)
AutoTyper --fast-typing

# Append Enter key after typing
AutoTyper --append-new-line
```

### Options

- `--delay, -d`: Time in seconds to wait before typing (default: 3.0)
- `--content, -c`: Specific content to type instead of clipboard
- `--append-new-line, -n`: Press Enter after typing
- `--fast-typing, -f`: Remove delay between keystrokes
- `--verbose, -v`: Print additional information

## Platform

**Windows only** - Uses Win32 APIs for window detection and keyboard simulation.

## License

MIT License