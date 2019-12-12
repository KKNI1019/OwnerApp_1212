using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace owner.Model
{
    public class ZoomPositionToValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            if (double.IsNaN((double)value))
                return 0;

            var zoomPosition = (double)value;
            var zoomFactor = (parameter as NumericalAxis).ZoomFactor;
            var zoomEndValue = zoomPosition + zoomFactor;

            return zoomEndValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.IsNaN((double)value))
                return 0;
            return value;
        }
    }
}
