using System;
using System.Numerics;
using static System.Math;
using static System.Diagnostics.Debug;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Playground
{
    partial class MainPage : Page
    {
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
            WriteLine(timingInfo);

            var r = 70.0f * (float)Abs(Cos((t.UpdateCount % 240L) / 240.0 * 2 * PI));
            var o = new Vector2(250, 250);
            ds.DrawCircle(o, r, Colors.DarkViolet, 7);
        }
    }
}
