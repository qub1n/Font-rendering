using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FontRendererWPF
{
    public class FontRendering
    {
        public static RenderTargetBitmap RenderBitMap(int heightPixel, Typeface typeface, double fontSize, string text)
        {
            int bitmapWidth = 600;
            int bitmapHeight = 100;
            double dpiX = 72;
            double dpiY = 72;

            RenderTargetBitmap bm = new RenderTargetBitmap(bitmapWidth, bitmapHeight, dpiX, dpiY, PixelFormats.Pbgra32);

            DrawingVisual drawing_visual = new DrawingVisual();

            RenderOptions.SetEdgeMode(drawing_visual, EdgeMode.Unspecified);
            RenderOptions.SetBitmapScalingMode(drawing_visual, BitmapScalingMode.Linear);

            RenderOptions.SetEdgeMode(bm, EdgeMode.Unspecified);
            RenderOptions.SetBitmapScalingMode(bm, BitmapScalingMode.Linear);

            TextOptions.SetTextRenderingMode(drawing_visual, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(drawing_visual, TextFormattingMode.Display);
            TextOptions.SetTextRenderingMode(bm, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(bm, TextFormattingMode.Display);
            

            DrawingContext drawing_context = drawing_visual.RenderOpen();

            FormattedText ft = new FormattedText(text, CultureInfo.InvariantCulture, System.Windows.FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            drawing_context.DrawText(ft, new Point(0, 0));

            drawing_context.Close();
            bm.Render(drawing_visual);

            return bm;
        }

        public static void SaveImageToFile(string filePath, BitmapSource image)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}
