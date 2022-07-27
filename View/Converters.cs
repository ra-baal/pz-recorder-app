using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Recorder.View
{
    //class BoolToTextConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        bool b = (bool)value;

    //        if (b == true)
    //            return "Kinect is ready.";
    //        else
    //            return "Kinect is not ready.";
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    class StateToBrushConverter : IValueConverter
    {
        private SolidColorBrush _brush = new SolidColorBrush();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string state = (string) value;
            _brush = state switch
            {
                "NoSensor" => Brushes.DimGray,
                "Ready" => Brushes.LightGray,
                "Preview" => Brushes.GreenYellow,
                "Recording" => Brushes.Red,
                _ => Brushes.Black
            };

            return _brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
