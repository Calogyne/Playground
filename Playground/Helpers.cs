using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Playground
{
    static class Helpers
    {
        public static Visual GetVisual(this UIElement _this)
            => ElementCompositionPreview.GetElementVisual(_this);

        public static void SetElementChildVisual(this UIElement _this, Visual visual)
            => ElementCompositionPreview.SetElementChildVisual(_this, visual);

        public static DropShadow SetupShadow(this SpriteVisual sprite)
        {
            var Compositor = sprite.Compositor;
            var shadow = Compositor.CreateDropShadow();
            var mask = Compositor.CreateMaskBrush();
            shadow.Offset = new Vector3(0.0f, 0.0f, 0.0f);
            shadow.Color = Windows.UI.Colors.Black;
            shadow.BlurRadius = 0.0f;
            shadow.Opacity = 0.90f;
            sprite.Shadow = shadow;
            return shadow;
        }

        /// <summary>
        /// Shamelessly copyed from StackOverflow anyway, it does voodoo magic so to give your visual perspective
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="viewingWindow"></param>
        public static void UpdatePerspective(this Visual visual)
        {
            Matrix4x4 view = Matrix4x4.CreateLookAt(new Vector3(0, 0, 20), Vector3.Zero, Vector3.UnitY);
            Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(.1f, 1, .1f, 100f);
            visual.TransformMatrix =
                view * projection;
        }

    }
}
