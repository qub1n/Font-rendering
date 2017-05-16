using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FontRendererWPF
{
    public class FontRendering
    {
        //public static RenderTargetBitmap RenderBitMap(int heightPixel, Typeface typeface, double fontSize, string text)
        //{
        //    int bitmapWidth = 600;
        //    int bitmapHeight = 100;
        //    double dpiX = 72;
        //    double dpiY = 72;

        //    RenderTargetBitmap bm = new RenderTargetBitmap(bitmapWidth, bitmapHeight, dpiX, dpiY, PixelFormats.Pbgra32);

        //    DrawingVisual drawing_visual = new DrawingVisual();

        //    RenderOptions.SetEdgeMode(drawing_visual, EdgeMode.Unspecified);
        //    RenderOptions.SetBitmapScalingMode(drawing_visual, BitmapScalingMode.Linear);

        //    RenderOptions.SetEdgeMode(bm, EdgeMode.Unspecified);
        //    RenderOptions.SetBitmapScalingMode(bm, BitmapScalingMode.Linear);

        //    TextOptions.SetTextRenderingMode(drawing_visual, TextRenderingMode.Grayscale);
        //    TextOptions.SetTextFormattingMode(drawing_visual, TextFormattingMode.Display);
        //    TextOptions.SetTextRenderingMode(bm, TextRenderingMode.Grayscale);
        //    TextOptions.SetTextFormattingMode(bm, TextFormattingMode.Display);
            

        //    DrawingContext drawing_context = drawing_visual.RenderOpen();

        //    FormattedText ft = new FormattedText(text, CultureInfo.InvariantCulture, System.Windows.FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
        //    drawing_context.DrawText(ft, new Point(0, 0));

        //    drawing_context.Close();
        //    bm.Render(drawing_visual);

        //    return bm;
        //}

        public static DrawingImage CreateImage(int heightPixel, Typeface typeface, double fontSize, string text)
        {
            var group = new DrawingGroup();
            using (var context = group.Open())
            {
                var ft = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black, null,
                    TextFormattingMode.Display); // this is the important line here!
                context.DrawText(ft, new Point(0, 0));
            }

            return new DrawingImage(group);
        }

        //public static void SaveImageToFile(string filePath, BitmapSource image)
        //{
        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        BitmapEncoder encoder = new PngBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(image));
        //        encoder.Save(fileStream);
        //    }
        //}

        public static void SaveToFile(string filePath, DrawingImage drawing)
        {
            var image = new Image { Source = drawing };
            image.Arrange(new Rect(new Size(drawing.Width, drawing.Height)));

            // this is the important line here!
            TextOptions.SetTextRenderingMode(image, TextRenderingMode.Aliased);

            // note if you use a different DPI than the screen (96 here), you still will have anti-aliasing
            // as the system is pretty automatic anyway
            var bitmap = new RenderTargetBitmap((int)drawing.Width, (int)drawing.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(image);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fileStream);
            }
        }      
    }
}
