using System;
using System.Numerics;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.Graphics.Effects;

namespace Playground
{
    partial class GaussianBlurBrush : XamlCompositionBrushBase
    {
        static DependencyProperty BlurAmountProperty =
            DependencyProperty.Register(
                "BlurAmount", typeof(float), typeof(GaussianBlurBrush),
                new PropertyMetadata(
                    defaultValue: 15.0f,
                    propertyChangedCallback: OnBlurAmountChanges));

        public float BlurAmount
        {
            get { return (float)this.GetValue(BlurAmountProperty); }
            set { this.SetValue(BlurAmountProperty, value); }
        }
    }
}
