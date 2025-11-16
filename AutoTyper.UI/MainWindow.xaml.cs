using System.Windows.Input;

using Velopack;

namespace AutoTyper.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        UpdateManager updateManager = new(new Velopack.Sources.VelopackFlowSource());
        if (updateManager.IsInstalled && updateManager.CurrentVersion is { } version)
        {
            Title += $"{version.Major}.{version.Minor}.{version.Patch}";
        }
        else
        {
            Title += " - Local";
        }

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnClose));
    }

    private void OnClose(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}
