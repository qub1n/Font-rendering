using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FontRendererWPF
{
    public class ImageCanvas : Canvas
    {       
        ImageSource _source;

        public ImageSource Source
        {
            get { return _source; }
            set
            {
                _source = value;
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_source == null)
                return;
            dc.DrawImage(_source, new Rect(0, 0, _source.Width, _source.Height));
        }
    }
}
