#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2015.
// </copyright>
//-----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace UWPColorPickerSample
{
    /// <summary>
    /// String to color converter
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// String to Converter
        /// </summary>
        /// <param name="value">color string</param>
        /// <param name="targetType">target type</param>
        /// <param name="parameter">parameter</param>
        /// <param name="language">language</param>
        /// <returns>color</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Colors.Transparent;
            }
            var colorString = value.ToString();
            var property = typeof(Colors).GetRuntimeProperty(colorString);
            if (property != null)
            {
                var color = property.GetValue(null);
                if (color is Color)
                {
                    return color;
                }
            }

            try
            {
                return Color.FromArgb(
                            System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(1, 2), 16)),
                            System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(3, 2), 16)),
                            System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(5, 2), 16)),
                            System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(7, 2), 16)));
            }
            catch (Exception)
            {
                // Invalid value
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}