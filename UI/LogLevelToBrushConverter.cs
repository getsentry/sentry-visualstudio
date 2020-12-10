using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace VSSentry.UI
{
    public class LogLevelToBrushConverter : IValueConverter
    {
        private static Dictionary<string, Brush> _colorDict = new Dictionary<string, Brush>()
        {
            ["trace"] = Brushes.Gray,
            ["debug"] = Brushes.LightSkyBlue,
            ["info"] = Brushes.Teal,
            ["warning"] = Brushes.Yellow,
            ["error"] = Brushes.Red,
            ["fatal"] = Brushes.DarkRed,
        };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string level && _colorDict.TryGetValue(level, out var brush))
            {
                return brush;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
