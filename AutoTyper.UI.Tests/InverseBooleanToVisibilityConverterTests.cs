using System.Globalization;
using System.Windows;

using AutoTyper.UI.Converters;

namespace AutoTyper.UI.Tests;

public class InverseBooleanToVisibilityConverterTests
{
    [Fact]
    public void Convert_TrueValue_ReturnsCollapsed()
    {
        // Arrange
        var converter = new InverseBooleanToVisibilityConverter();

        // Act
        var result = converter.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void Convert_FalseValue_ReturnsVisible()
    {
        // Arrange
        var converter = new InverseBooleanToVisibilityConverter();

        // Act
        var result = converter.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Visible, result);
    }

    [Fact]
    public void Convert_NullValue_ReturnsVisible()
    {
        // Arrange
        var converter = new InverseBooleanToVisibilityConverter();

        // Act
        var result = converter.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Visible, result);
    }

    [Fact]
    public void ConvertBack_Visible_ReturnsFalse()
    {
        // Arrange
        var converter = new InverseBooleanToVisibilityConverter();

        // Act
        var result = converter.ConvertBack(Visibility.Visible, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBack_Collapsed_ReturnsTrue()
    {
        // Arrange
        var converter = new InverseBooleanToVisibilityConverter();

        // Act
        var result = converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(true, result);
    }
}
