using System;
using System.Numerics;
using static System.Math;
using static System.Diagnostics.Debug;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Effects;

namespace Playground
{
    partial class MainPage : Page
    {
        GaussianBlurEffect _blur;
        CanvasBitmap lauren;
        CanvasImageBrush lauren_brush;

        // called when the CanvasAnimatedControl control first starts drawing.
        void drawingBoard_Setup(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            var filelocation = new Uri("ms-appx:///Assets/Pictures/lauren_mayberry.jpg"); //343 × 515

            // previously:
            // lauren = await CanvasBitmap.LoadAsync(sender, filelocation);
            lauren = CanvasBitmap.LoadAsync(sender, filelocation).AsTask().Result;
             
            _blur = new GaussianBlurEffect { BlurAmount = 3.0f, Source = lauren };
            
            lauren_brush = new CanvasImageBrush(sender, _blur)
            {
                SourceRectangle = new Rect(0, 0, 343, 343), // I have no idea about these params.
                Opacity = 0.9f,
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Wrap
            };
        }

        // draw stuff onto the other pivot page here! @60FPS!
        private void drawingBoard_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var ds = args.DrawingSession;
            var t = args.Timing;

            // frame count of (this) second.
            var fraCount = t.UpdateCount % 60L;
            var timingInfo =
                $"<Elapsed time: {t.ElapsedTime}, Total time: {t.TotalTime}, " +
                $"\nFrame cout. of current second: {fraCount}>";
            if (t.UpdateCount % 10L == 0) WriteLine(timingInfo);

            var r = 70.0f * (float)Abs(Sin((t.UpdateCount % 240L) / 240.0 * 2 * PI));
            var o = new Vector2(250, 250);
            ds.DrawCircle(o, r, Colors.DarkViolet, 7);
        
            var lauren_rect = new Rect(100, 250, 100, 100);
            ds.FillRectangle(lauren_rect, lauren_brush);
            ds.DrawRectangle(lauren_rect, Colors.DarkGray); //draw a border around it
        }
    }
}
