using System;
using System.Numerics;
using static System.Math;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;

namespace AdaptiveAlbumView.XAML
{
    partial class AlbumItem : FrameworkElement
    {
        private class TextDrawer
        {
            public readonly CompositionDrawingSurface DrawingSurface;
            string text;
            string fontFamily;

            public TextDrawer(CompositionGraphicsDevice device)
            {
                DrawingSurface = device.CreateDrawingSurface(
                    new Size(1000, 200),
                    Windows.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized,
                    Windows.Graphics.DirectX.DirectXAlphaMode.Premultiplied);

                //this.text = text;
                //this.fontFamily = fontFamily;
                Draw();
            }

            void Draw()
            {
                using (CanvasDrawingSession ds = CanvasComposition.CreateDrawingSession(DrawingSurface))
                {
                    var format = new CanvasTextFormat
                    {
                        FontSize = 60
                    };
                    ds.DrawText("Fuck yeah baby", Vector2.Zero, Colors.DarkOrange, format);
                }
            }
        }
    }
}
