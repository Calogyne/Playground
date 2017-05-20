using System;
using System.Numerics;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace Playground
{
    static class GaussianBlurBrushHelper
    {
        static public CompositionBrush CreateGaussianBlurCompositionBrush(this Compositor compositor, float blurAmount)
        {
            var blurEffect = new GaussianBlurEffect
            {
                Source = new CompositionEffectSourceParameter("backdrop"),
                BlurAmount = blurAmount,
                Name = "Blur"
            };

            var backdropBrush = compositor.CreateBackdropBrush();

            var effectFactory = compositor.CreateEffectFactory(blurEffect, new[] { "Blur.BlurAmount" });

            var effectBrush = effectFactory.CreateBrush();

            effectBrush.SetSourceParameter("backdrop", backdropBrush);

            return effectBrush;
        }
    }

    partial class GaussianBlurBrush : XamlCompositionBrushBase
    {
        float _blurAmount = 15.0f;

        public GaussianBlurBrush() { }

        protected override void OnConnected()
        {
            base.OnConnected();

            this.CompositionBrush = Window.Current.Compositor.CreateGaussianBlurCompositionBrush(_blurAmount);
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            this.CompositionBrush?.Dispose();
            this.CompositionBrush = null;
        }

        static void OnBlurAmountChanges(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            var _this = (GaussianBlurBrush)dp;

            var compositor = _this.CompositionBrush?.Compositor;

            var transition = compositor.CreateScalarKeyFrameAnimation();

            transition.InsertKeyFrame(
                value: (float)args.NewValue,
                normalizedProgressKey: 1.0f,
                easingFunction: compositor.CreateCubicBezierEasingFunction(new Vector2(0.0f, 1.05f), new Vector2(0.2f, 1.0f)));

            transition.Duration = TimeSpan.FromMilliseconds(800.0);

            _this.CompositionBrush.StartAnimation("BlurAmount", transition);
        }
    }
}
