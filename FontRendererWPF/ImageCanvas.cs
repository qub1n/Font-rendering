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

        public int Zoom { get; set; }

        public ImageCanvas()
        {
            Zoom = 1;
            this.VisualEdgeMode = EdgeMode.Aliased;

            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

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

            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = Zoom;
            scaleTransform.ScaleY = Zoom;            

            dc.PushTransform(scaleTransform);

            RenderOptions.SetEdgeMode(_source, EdgeMode.Unspecified);
            RenderOptions.SetBitmapScalingMode(_source, BitmapScalingMode.NearestNeighbor);

            int w = (int)(_source.Width * Zoom);
            int h = (int)(Source.Height * Zoom);

            dc.DrawImage(_source, new Rect(0, 0, _source.Width, _source.Height));

            dc.Pop();

            if(Zoom > 1)
            {
                for (int i = 0; i < w; i+=Zoom)
                {
                    dc.DrawLine(new Pen(Brushes.White, 1), new Point(i, 0), new Point(i, h));
                }

                for (int j = 0; j < h; j += Zoom)
                {
                    dc.DrawLine(new Pen(Brushes.White, 1), new Point(0,j), new Point(w, j));
                }
            }
        }
    }
}
