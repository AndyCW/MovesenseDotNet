using SampleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SampleApp.Converters
{
    public class DeviceStatusColorBindingValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var deviceStatus = (DeviceStatus)value;
            var ret = Color.White;
            switch (deviceStatus)
            {
                case DeviceStatus.Undefined:
                    break;
                case DeviceStatus.Discovered:
                    ret = Color.LightBlue; break;
                case DeviceStatus.Connecting:
                    ret = Color.Yellow; break;
                case DeviceStatus.Connected:
                    ret = Color.Green; break;
                case DeviceStatus.Inactive:
                    ret = Color.Wheat; break;
                case DeviceStatus.Logging:
                    ret = Color.Orange; break;
                case DeviceStatus.Recording:
                    ret = Color.Orange; break;
                case DeviceStatus.RecordingDone:
                    ret = Color.Purple; break;
                case DeviceStatus.NotFound:
                    ret = Color.Red; break;
                case DeviceStatus.Error:
                    ret = Color.Red; break;
                default:
                    break;
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
