using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPTiledImageSample
{
    /// <summary>
    /// Tiled sourceImage rendering panel
    /// </summary>
    public class ImageTile : Panel
    {
        /// <summary>
        /// Image source dependency property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(ImageSource),
            typeof(ImageTile),
            new PropertyMetadata(null, OnSourceChanged));

        /// <summary>
        /// Image source CLR property
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Image source changed event handler
        /// </summary>
        /// <param name="d">dependency object</param>
        /// <param name="e">event argument</param>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as ImageTile;
            if (panel == null)
            {
                return;
            }
            panel.sourceImage = new Image
            {
                Source = panel.Source,
                UseLayoutRounding = false,
                Stretch = Stretch.None,
            };
            panel.sourceImage.ImageOpened += panel.OnSourceImageOpened;
            panel.sourceImage.ImageFailed += panel.OnSourceImageFailed;

            // Open sourceImage by add a sourceImage on visual tree
            panel.Children.Add(panel.sourceImage);
        }

        /// <summary>
        /// Image open failed event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void OnSourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.sourceImage.ImageOpened -= this.OnSourceImageOpened;
            this.sourceImage.ImageFailed -= this.OnSourceImageFailed;
            this.sourceImage = null;
            this.Children.Clear();
        }

        /// <summary>
        /// Image opened event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void OnSourceImageOpened(object sender, RoutedEventArgs e)
        {
            this.sourceImage.ImageOpened -= this.OnSourceImageOpened;
            this.sourceImage.ImageFailed -= this.OnSourceImageFailed;
            this.sourceImage = null;

            // Children Images layout update
            this.InvalidateArrange();
        }

        /// <summary>
        /// Source sourceImage
        /// </summary>
        private Image sourceImage;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageTile()
        {
            this.Unloaded += this.OnUnloaded;
        }

        /// <summary>
        /// Unloaded event handler
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event arguments</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= this.OnUnloaded;
            if (this.sourceImage == null)
            {
                return;
            }
            this.sourceImage.ImageFailed -= this.OnSourceImageFailed;
            this.sourceImage.ImageOpened -= this.OnSourceImageOpened;
            this.sourceImage = null;
        }

        /// <summary>
        /// Arrange children elements layout
        /// </summary>
        /// <param name="finalSize">final layout size</param>
        /// <returns>decided layout size</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var bmp = this.Source as BitmapSource;
            if (bmp == null)
            {
                return base.ArrangeOverride(finalSize);
            }

            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;
            if (width == 0 || height == 0)
            {
                return base.ArrangeOverride(finalSize);
            }

            // Put images at tiled
            var index = 0;
            for (double x = 0; x < finalSize.Width; x += width)
            {
                for (double y = 0; y < finalSize.Height; y += height)
                {
                    Image image;
                    if (this.Children.Count > index)
                    {
                        image = (Image)this.Children[index];
                        image.Source = bmp;
                    }
                    else
                    {
                        image = new Image
                        {
                            Source = bmp,
                            UseLayoutRounding = false,
                            Stretch = Stretch.None
                        };
                        this.Children.Add(image);
                    }
                    image.Measure(new Size(width, height));
                    image.Arrange(new Rect(x, y, width, height));
                    index++;
                }
            }

            // Remove unnecessary images
            var count = this.Children.Count;
            for (var i = index; i < count; i++)
            {
                this.Children.RemoveAt(index);
            }

            // Clip pushed out images
            this.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, finalSize.Width, finalSize.Height)
            };
            return base.ArrangeOverride(finalSize);
        }
    }
}
