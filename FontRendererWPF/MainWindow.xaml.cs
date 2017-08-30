using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfColorFontDialog;

namespace FontRendererWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double fontSize = 14;

        FontInfo font = new FontInfo(new FontFamily("Arial"), 14.0, new FontStyle(), new FontStretch(), new FontWeight(), new SolidColorBrush(Colors.Black));


        public MainWindow()
        {
            InitializeComponent();

            canvasBig.Zoom = 10;

            UpdateFontName();
        }

        private void buttonChangeFont_Click(object sender, RoutedEventArgs e)
        {
            ColorFontDialog dialog = new ColorFontDialog();

            if (font != null)
                dialog.Font = font;

            var result = dialog.ShowDialog();

            if (result == true)
            {
                font = dialog.Font;
                fontSize = font.Size;
                RefreshFont();
            }
        }

        private Typeface Convert(FontInfo fontInfo)
        {
            FamilyTypeface familyTypeface = fontInfo.Typeface;
            FontFamily family = new FontFamily(fontInfo.Family.ToString());
            return new Typeface(family, familyTypeface.Style, familyTypeface.Weight, familyTypeface.Stretch);
        }

        private void RefreshFont()
        {
            UpdateFontName();

            if (font == null)
                return;

            RefreshFont(Convert(font), fontSize, textboxText.Text);
        }

        private void UpdateFontName()
        {
            if (fontName == null)
                return;

            if (font == null)
            {
                fontName.Content = null;
                return;
            }
            else
            {
                fontName.Content = string.Format("{0}, {1}, {2}", font.Family, font.Size, font.Style);
            }
        }

        private void RefreshFont(Typeface typeface, double fontSize, string text)
        {
            if (canvasNormal == null || canvasBig == null)
                return;

            DrawingImage image = FontRendering.CreateImage(9, typeface, fontSize, text);

            canvasNormal.Source = image;
            using (MemoryStream stream = new MemoryStream())
            {
                FontRendering.SaveTo(stream, image);

                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();

                canvasBig.Source = bitmap;
            }
        }

        private void textboxText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshFont();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            canvasBig.Zoom = (int)e.NewValue;

            labelZoom.Content = "Zoom " + (int)e.NewValue + "x";

            RefreshFont();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 about = new Window1();
            about.ShowDialog();
        }
    }
}
