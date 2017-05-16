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
        FontInfo font;
        string imagePath = "image.png";
        double fontSize = 14;

        public MainWindow()
        {
            InitializeComponent();

            canvasBig.Zoom = 10;
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
            if (font == null)
                return;

            RefreshFont(Convert(font), fontSize, textboxText.Text);
        }

        private void RefreshFont(Typeface typeface, double fontSize, string text)
        {
            DrawingImage image = FontRendering.CreateImage(9, typeface, fontSize, text);

            canvasNormal.Source = image;
                       
            FontRendering.SaveToFile(imagePath, image);

            var bitmap = new BitmapImage();
            using (var stream = File.OpenRead(imagePath))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();

                canvasBig.Source = bitmap;
            };
        }

        private void textboxText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshFont();
        }
    }
}
