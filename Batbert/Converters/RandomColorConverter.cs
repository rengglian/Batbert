using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Batbert.Converters
{
    public class RandomColorConverter : IValueConverter
	{
        readonly Random r = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((string)value).StartsWith("0"))
            {
                return new SolidColorBrush(Color.FromArgb((byte)(255.0 / 3.0), (byte)r.Next(1, 255),
                          (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
            }
            var test = Application.Current.Resources["Batbert.Views.GroupBox.ButtonBackground"];
            return test;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
