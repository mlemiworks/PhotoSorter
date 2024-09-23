using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PhotoSorter.Converters
{
    public class ImageSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double size && parameter is string dimension)
            {
                // Here, we'll return a proportional size based on window dimensions
                if (dimension == "Width")
                {
                    // Return adjusted width based on the window height
                    return size * 0.9; // Scale width relative to height
                }
                else if (dimension == "Height")
                {
                    // Return adjusted height based on the window width
                    return size * 0.9; // Scale height relative to width
                }
            }

            return value; // Return original value as fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
