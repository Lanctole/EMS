using EMS.ControlInterface.Converters;

namespace EMS.Tests;

public class BoolToStringConverterTests
{
    [Fact]
    public void Convert_ShouldReturnTrueText_WhenValueIsTrue()
    {
        var converter = new BoolToStringConverter { TrueText = "Годный", FalseText = "Брак" };

        var result = converter.Convert(true, null, null, null);

        Assert.Equal("Годный", result);
    }

    [Fact]
    public void Convert_ShouldReturnFalseText_WhenValueIsFalse()
    {
        var converter = new BoolToStringConverter { TrueText = "Годный", FalseText = "Брак" };

        var result = converter.Convert(false, null, null, null);

        Assert.Equal("Брак", result);
    }

    [Fact]
    public void ConvertBack_ShouldReturnTrue_WhenValueIsTrueText()
    {
        var converter = new BoolToStringConverter { TrueText = "Годный", FalseText = "Брак" };

        var result = converter.ConvertBack("Годный", null, null, null);

        Assert.True((bool)result);
    }

    [Fact]
    public void ConvertBack_ShouldReturnFalse_WhenValueIsFalseText()
    {
        var converter = new BoolToStringConverter { TrueText = "Годный", FalseText = "Брак" };

        var result = converter.ConvertBack("Брак", null, null, null);

        Assert.False((bool)result);
    }
}