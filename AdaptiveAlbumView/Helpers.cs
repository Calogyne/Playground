using System;
using System.Numerics;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;


namespace AdaptiveAlbumView
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

        public static CompositionBrush Create(this Compositor compositor, float blurAmount)
        {
            var blurEffect = new GaussianBlurEffect
            {
                Source = new CompositionEffectSourceParameter("backdrop"),
                BlurAmount = blurAmount,
                Name = "Blur"
            };

            var backdropBrush = compositor.CreateHostBackdropBrush();
            var effectFactory = compositor.CreateEffectFactory(blurEffect, new[] { "Blur.BlurAmount" });
            var brush = effectFactory.CreateBrush();
            brush.SetSourceParameter("backdrop", backdropBrush);

            return brush;
        }

        public static CubicBezierEasingFunction CreateEaseInOutQuadFunction(this Compositor compositor) =>
            compositor.CreateCubicBezierEasingFunction(new Vector2(0.0f, 1.05f), new Vector2(0.2f, 1.0f));
    }
}
