using System.Windows;
using System.Windows.Media;
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

        public MainWindow()
        {
            InitializeComponent();
            
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

                

                LoadFont(Convert(font), font.Size, textboxText.Text);
            }
        }

        private Typeface Convert(FontInfo fontInfo)
        {
            FamilyTypeface familyTypeface = fontInfo.Typeface;
            FontFamily family = new FontFamily(fontInfo.Family.ToString());
            return new Typeface(family, familyTypeface.Style, familyTypeface.Weight, familyTypeface.Stretch);
        }

        private void LoadFont(Typeface typeface, double fontSize, string text)
        {
            //var bitmap = FontRendering.RenderBitMap(9, typeface, fontSize, text);
            var image = FontRendering.CreateImage(9, typeface, fontSize, text);

            canvas.Source = image;

            //FontRendering.SaveImageToFile(imagePath, bitmap);            
            FontRendering.SaveToFile(imagePath, image);
        }
        
        
    }
}
