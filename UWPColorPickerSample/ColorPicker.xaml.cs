#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2015.
// </copyright>
//-----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPColorPickerSample
{
    /// <summary>
    /// Color picker settings view model
    /// </summary>
    public sealed partial class ColorPicker : UserControl
    {
        /// <summary>
        /// View model
        /// </summary>
        public ColorPickerViewModel ViewModel { get; set; } = new ColorPickerViewModel();

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorPicker()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// DataContext changed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnDataContextChanged(object sender, DataContextChangedEventArgs e)
        {
            this.ViewModel = this.DataContext as ColorPickerViewModel;
        }

        /// <summary>
        /// Picker area pointer pressed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnPickerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.PickColor(e.GetCurrentPoint(this.pickerCanvas).Position);
            this.pickerCanvas.CapturePointer(e.Pointer);

            PointerEventHandler moved = null;
            moved = (s, args) =>
            {
                this.PickColor(args.GetCurrentPoint(this.pickerCanvas).Position);
            };
            PointerEventHandler released = null;
            released = (s, args) =>
            {
                this.pickerCanvas.ReleasePointerCapture(args.Pointer);
                this.PickColor(args.GetCurrentPoint(this.pickerCanvas).Position);
                this.pickerCanvas.PointerMoved -= moved;
                this.pickerCanvas.PointerReleased -= released;
            };

            this.pickerCanvas.PointerMoved += moved;
            this.pickerCanvas.PointerReleased += released;
        }

        /// <summary>
        /// Pick color
        /// </summary>
        /// <param name="point">pick point</param>
        private void PickColor(Point point)
        {
            var px = Math.Max(0d, point.X);
            px = Math.Min(this.pickerCanvas.ActualWidth, px);
            var py = Math.Max(0d, point.Y);
            py = Math.Min(this.pickerCanvas.ActualHeight, py);

            this.ViewModel.PickPointX = Math.Round(px, MidpointRounding.AwayFromZero);
            this.ViewModel.PickPointY = Math.Round(py, MidpointRounding.AwayFromZero);
            this.ViewModel.OnPickPointChanged();
        }

        /// <summary>
        /// Color spectrum area pointer pressed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnHuePressed(object sender, PointerRoutedEventArgs e)
        {
            this.ChangeHue(e.GetCurrentPoint(this.colorSpectrum).Position.Y);
            this.colorSpectrum.CapturePointer(e.Pointer);

            PointerEventHandler moved = null;
            moved = (s, args) =>
            {
                this.ChangeHue(args.GetCurrentPoint(this.colorSpectrum).Position.Y);
            };
            PointerEventHandler released = null;
            released = (s, args) =>
            {
                this.colorSpectrum.ReleasePointerCapture(args.Pointer);
                this.ChangeHue(args.GetCurrentPoint(this.colorSpectrum).Position.Y);
                this.colorSpectrum.PointerMoved -= moved;
                this.colorSpectrum.PointerReleased -= released;
            };
            this.colorSpectrum.PointerMoved += moved;
            this.colorSpectrum.PointerReleased += released;
        }

        /// <summary>
        /// Change color hue
        /// </summary>
        /// <param name="y">change point</param>
        private void ChangeHue(double y)
        {
            var py = Math.Max(0d, y);
            py = Math.Min(this.colorSpectrum.ActualHeight, py);

            this.ViewModel.ColorSpectrumPoint = Math.Round(py, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Red value bar pointer pressed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnRedPressed(object sender, PointerRoutedEventArgs e)
        {
            this.ViewModel.Red = this.ArrangeArgb(e.GetCurrentPoint(this.red).Position.X, this.red.ActualWidth);
            this.red.CapturePointer(e.Pointer);

            PointerEventHandler moved = null;
            moved = (s, args) =>
            {
                this.ViewModel.Red = this.ArrangeArgb(e.GetCurrentPoint(this.red).Position.X, this.red.ActualWidth);
            };
            PointerEventHandler released = null;
            released = (s, args) =>
            {
                this.red.ReleasePointerCapture(args.Pointer);
                this.ViewModel.Red = this.ArrangeArgb(e.GetCurrentPoint(this.red).Position.X, this.red.ActualWidth);
                this.red.PointerMoved -= moved;
                this.red.PointerReleased -= released;
            };
            this.red.PointerMoved += moved;
            this.red.PointerReleased += released;
        }

        /// <summary>
        /// Green value bar pointer pressed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnGreenPressed(object sender, PointerRoutedEventArgs e)
        {
            this.ViewModel.Green = this.ArrangeArgb(e.GetCurrentPoint(this.green).Position.X, this.green.ActualWidth);
            this.green.CapturePointer(e.Pointer);

            PointerEventHandler moved = null;
            moved = (s, args) =>
            {
                this.ViewModel.Green = this.ArrangeArgb(e.GetCurrentPoint(this.green).Position.X, this.green.ActualWidth);
            };
            PointerEventHandler released = null;
            released = (s, args) =>
            {
                this.green.ReleasePointerCapture(args.Pointer);
                this.ViewModel.Green = this.ArrangeArgb(e.GetCurrentPoint(this.green).Position.X, this.green.ActualWidth);
                this.green.PointerMoved -= moved;
                this.green.PointerReleased -= released;
            };
            this.green.PointerMoved += moved;
            this.green.PointerReleased += released;
        }

        /// <summary>
        /// Blue value bar pointer pressed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnBluePressed(object sender, PointerRoutedEventArgs e)
        {
            this.ViewModel.Blue = this.ArrangeArgb(e.GetCurrentPoint(this.blue).Position.X, this.blue.ActualWidth);
            this.blue.CapturePointer(e.Pointer);

            PointerEventHandler moved = null;
            moved = (s, args) =>
            {
                this.ViewModel.Blue = this.ArrangeArgb(e.GetCurrentPoint(this.blue).Position.X, this.blue.ActualWidth);
            };
            PointerEventHandler released = null;
            released = (s, args) =>
            {
                this.blue.ReleasePointerCapture(args.Pointer);
                this.ViewModel.Blue = this.ArrangeArgb(e.GetCurrentPoint(this.blue).Position.X, this.blue.ActualWidth);
                this.blue.PointerMoved -= moved;
                this.blue.PointerReleased -= released;
            };
            this.blue.PointerMoved += moved;
            this.blue.PointerReleased += released;
        }

        /// <summary>
        /// Alpha value bar pointer pressed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event aruments</param>
        private void OnAlphaPressed(object sender, PointerRoutedEventArgs e)
        {
            this.ViewModel.Alpha = this.ArrangeArgb(e.GetCurrentPoint(this.alpha).Position.X, this.alpha.ActualWidth);
            this.alpha.CapturePointer(e.Pointer);

            PointerEventHandler moved = null;
            moved = (s, args) =>
            {
                this.ViewModel.Alpha = this.ArrangeArgb(e.GetCurrentPoint(this.alpha).Position.X, this.alpha.ActualWidth);
            };
            PointerEventHandler released = null;
            released = (s, args) =>
            {
                this.alpha.ReleasePointerCapture(args.Pointer);
                this.ViewModel.Alpha = this.ArrangeArgb(e.GetCurrentPoint(this.alpha).Position.X, this.alpha.ActualWidth);
                this.alpha.PointerMoved -= moved;
                this.alpha.PointerReleased -= released;
            };
            this.alpha.PointerMoved += moved;
            this.alpha.PointerReleased += released;
        }

        /// <summary>
        /// Arrange color element value
        /// </summary>
        /// <param name="x">change point</param>
        /// <param name="max">change point max</param>
        private int ArrangeArgb(double x, double max)
        {
            var px = x * 255d / max;
            px = Math.Max(0d, px);
            px = Math.Min(255d, px);

            return (int)Math.Round(px, MidpointRounding.AwayFromZero);
        }
    }
}
