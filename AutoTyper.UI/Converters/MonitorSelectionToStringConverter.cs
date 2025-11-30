using System.Globalization;
using System.Windows.Data;

using AutoTyper.UI.Models;

namespace AutoTyper.UI.Converters;

public class MonitorSelectionToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MonitorSelection selection)
        {
            return selection switch
            {
                MonitorSelection.CursorMonitor => "Cursor Monitor",
                MonitorSelection.PrimaryMonitor => "Primary Monitor",
                MonitorSelection.MonitorByIndex => "Monitor by Index",
                _ => value.ToString() ?? string.Empty
            };
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
