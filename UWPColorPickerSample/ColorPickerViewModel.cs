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
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Practices.Prism.Mvvm;

namespace UWPColorPickerSample
{
    /// <summary>
    /// Color picker ViewModel
    /// </summary>
    public partial class ColorPickerViewModel : BindableBase
    {
        /// <summary>
        /// Picker height
        /// </summary>
        private static readonly double PickerHeight = 150d;

        /// <summary>
        /// Picker width
        /// </summary>
        private static readonly double PickerWidth = 150d;

        /// <summary>
        /// String color code to color converter
        /// </summary>
        private static readonly ColorConverter Converter = new ColorConverter();

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorPickerViewModel()
        {
            this.color = "#FFFF0000";
            this.pickPointX = 150d;
            this.pickPointY = 0d;
            this.colorSpectrumPoint = 0d;
            this.UpdateColor(Colors.Red);
            this.UpdatePickPoint();
        }

        /// <summary>
        /// Update color properties
        /// </summary>
        /// <param name="color">new color</param>
        public void UpdateColor(Color color)
        {
            this.color = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
            this.alpha = color.A;
            this.alphaString = this.alpha.ToString();
            this.red = color.R;
            this.redString = this.red.ToString();
            this.green = color.G;
            this.greenString = this.green.ToString();
            this.blue = color.B;
            this.blueString = this.blue.ToString();

            this.redStartColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, 0, color.G, color.B);
            this.redEndColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, 0xff, color.G, color.B);
            this.greenStartColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, color.R, 0, color.B);
            this.greenEndColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, color.R, 0xff, color.B);
            this.blueStartColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, color.R, color.G, 0);
            this.blueEndColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, color.R, color.G, 0xff);
            this.alphaStartColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0, color.R, color.G, color.B);
            this.alphaEndColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 0xff, color.R, color.G, color.B);

            var hsv = ToHSV(color);
            var h = FromHsv(hsv[0], 1f, 1f);
            this.hueColor = string.Format("#FF{0:X2}{1:X2}{2:X2}", h.R, h.G, h.B);

            this.OnPropertyChanged("Color");
            this.OnPropertyChanged("Red");
            this.OnPropertyChanged("RedString");
            this.OnPropertyChanged("Green");
            this.OnPropertyChanged("GreenString");
            this.OnPropertyChanged("Blue");
            this.OnPropertyChanged("BlueString");
            this.OnPropertyChanged("Alpha");
            this.OnPropertyChanged("AlphaString");
            this.OnPropertyChanged("RedStartColor");
            this.OnPropertyChanged("RedEndColor");
            this.OnPropertyChanged("GreenStartColor");
            this.OnPropertyChanged("GreenEndColor");
            this.OnPropertyChanged("BlueStartColor");
            this.OnPropertyChanged("BlueEndColor");
            this.OnPropertyChanged("AlphaStartColor");
            this.OnPropertyChanged("AlphaEndColor");
            this.OnPropertyChanged("HueColor");
        }

        /// <summary>
        /// Update pick color point
        /// </summary>
        public void UpdatePickPoint()
        {
            var hsv = ToHSV((Color)Converter.Convert(this.color, typeof(Color), null, null));
            this.pickPointX = PickerWidth * hsv[1];
            this.pickPointY = PickerHeight * (1 - hsv[2]);
            this.colorSpectrumPoint = PickerHeight * hsv[0] / 360f;

            this.OnPropertyChanged("PickPointX");
            this.OnPropertyChanged("PickPointY");
            this.OnPropertyChanged("ColorSpectrumPoint");
        }

        /// <summary>
        /// Convert rgb to hsv color
        /// </summary>
        /// <param name="color">rgb color</param>
        /// <returns>hsv color</returns>
        private static float[] ToHSV(Color color)
        {
            var rgb = new float[]
            {
                color.R / 255f, color.G / 255f, color.B / 255f
            };

            // RGB to HSV
            float max = rgb.Max();
            float min = rgb.Min();

            float h, s, v;
            if (max == min)
            {
                h = 0f;
            }
            else if (max == rgb[0])
            {
                h = (60f * (rgb[1] - rgb[2]) / (max - min) + 360f) % 360f;
            }
            else if (max == rgb[1])
            {
                h = 60f * (rgb[2] - rgb[0]) / (max - min) + 120f;
            }
            else
            {
                h = 60f * (rgb[0] - rgb[1]) / (max - min) + 240f;
            }

            if (max == 0d)
            {
                s = 0f;
            }
            else
            {
                s = (max - min) / max;
            }
            v = max;

            return new float[] { h, s, v };
        }

        /// <summary>
        /// Convert hsv to rgb color
        /// </summary>
        /// <param name="hue">hue</param>
        /// <param name="saturation">saturation</param>
        /// <param name="brightness">brightness</param>
        /// <returns>Color</returns>
        private static Color FromHsv(float hue, float saturation, float brightness)
        {
            if (saturation == 0)
            {
                var c = (byte)Math.Round(brightness * 255f, MidpointRounding.AwayFromZero);
                return ColorHelper.FromArgb(0xff, c, c, c);
            }

            var hi = ((int)(hue / 60f)) % 6;
            var f = hue / 60f - (int)(hue / 60d);
            var p = brightness * (1 - saturation);
            var q = brightness * (1 - f * saturation);
            var t = brightness * (1 - (1 - f) * saturation);

            float r, g, b;
            switch (hi)
            {
                case 0:
                    r = brightness;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = brightness;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = brightness;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = brightness;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = brightness;
                    break;

                case 5:
                    r = brightness;
                    g = p;
                    b = q;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return ColorHelper.FromArgb(
                0xff,
                (byte)Math.Round(r * 255d),
                (byte)Math.Round(g * 255d),
                (byte)Math.Round(b * 255d));
        }

        /// <summary>
        /// Process in changing color
        /// </summary>
        partial void OnColorChanged()
        {
            this.UpdateColor((Color)Converter.Convert(this.color, typeof(Color), null, null));
            this.UpdatePickPoint();
        }

        /// <summary>
        /// Process in changing alpha channel string
        /// </summary>
        partial void OnAlphaStringChanged()
        {
            int parsed;
            if (int.TryParse(this.alphaString, out parsed))
            {
                this.Alpha = parsed;
            }
            else
            {
                this.AlphaString = this.alpha.ToString();
            }
        }

        /// <summary>
        /// Process in changing alpha channel
        /// </summary>
        partial void OnAlphaChanged()
        {
            var updated = (Color)Converter.Convert(this.color, typeof(Color), null, null);
            updated.A = (byte)Math.Max(0, this.alpha);
            updated.A = Math.Min((byte)0xff, updated.A);
            this.UpdateColor(updated);
            this.UpdatePickPoint();
        }

        /// <summary>
        /// Process in changing red string
        /// </summary>
        partial void OnRedStringChanged()
        {
            int parsed;
            if (int.TryParse(this.redString, out parsed))
            {
                this.Red = parsed;
            }
            else
            {
                this.RedString = this.red.ToString();
            }
        }

        /// <summary>
        /// Process in changing red
        /// </summary>
        partial void OnRedChanged()
        {
            var updated = (Color)Converter.Convert(this.color, typeof(Color), null, null);
            updated.R = (byte)Math.Max(0, this.red);
            updated.R = Math.Min((byte)0xff, updated.R);
            this.UpdateColor(updated);
            this.UpdatePickPoint();
        }

        /// <summary>
        /// Process in changing green string
        /// </summary>
        partial void OnGreenStringChanged()
        {
            int parsed;
            if (int.TryParse(this.greenString, out parsed))
            {
                this.Green = parsed;
            }
            else
            {
                this.GreenString = this.green.ToString();
            }
        }

        /// <summary>
        /// Process in changing green
        /// </summary>
        partial void OnGreenChanged()
        {
            var updated = (Color)Converter.Convert(this.color, typeof(Color), null, null);
            updated.G = (byte)Math.Max(0, this.green);
            updated.G = Math.Min((byte)0xff, updated.G);
            this.UpdateColor(updated);
            this.UpdatePickPoint();
        }

        /// <summary>
        /// Process in changing blue string
        /// </summary>
        partial void OnBlueStringChanged()
        {
            int parsed;
            if (int.TryParse(this.blueString, out parsed))
            {
                this.Blue = parsed;
            }
            else
            {
                this.BlueString = this.blue.ToString();
            }
        }

        /// <summary>
        /// Process in changing blue
        /// </summary>
        partial void OnBlueChanged()
        {
            var updated = (Color)Converter.Convert(this.color, typeof(Color), null, null);
            updated.B = (byte)Math.Max(0, this.blue);
            updated.B = Math.Min((byte)0xff, updated.B);
            this.UpdateColor(updated);
            this.UpdatePickPoint();
        }

        /// <summary>
        /// Process in changing color spectrum point
        /// </summary>
        partial void OnColorSpectrumPointChanged()
        {
            var old = (Color)Converter.Convert(this.color, typeof(Color), null, null);
            var hsv = ToHSV(old);
            hsv[0] = (float)(this.colorSpectrumPoint * 360f / PickerHeight);
            var updated = FromHsv(hsv[0], hsv[1], hsv[2]);
            this.UpdateColor(updated);
        }

        /// <summary>
        /// Process in changing color pick point
        /// </summary>
        public void OnPickPointChanged()
        {
            var old = (Color)Converter.Convert(this.color, typeof(Color), null, null);
            var hsv = ToHSV(old);
            var updated = FromHsv(hsv[0], (float)(this.pickPointX / PickerWidth), 1f - (float)(this.pickPointY / PickerHeight));
            this.UpdateColor(updated);
        }
    }
}
