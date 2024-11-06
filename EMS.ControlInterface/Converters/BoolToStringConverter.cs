using System.Globalization;
using System.Windows.Data;

namespace EMS.ControlInterface.Converters;

public class BoolToStringConverter : IValueConverter
{
    public string TrueText { get; set; } = "Годный";
    public string FalseText { get; set; } = "Брак";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b) return b ? TrueText : FalseText;
        return FalseText;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            if (s == TrueText) return true;
            if (s == FalseText) return false;
        }

        return false;
    }
}