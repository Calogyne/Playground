using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Playground
{
    public sealed partial class InteractiveShadowBase : ContentControl
    {
        static DependencyProperty ShadowThicknessProperty =
            DependencyProperty.Register(
                "ShadowThickness", typeof(float), typeof(InteractiveShadowBase),
                new PropertyMetadata(
                    0.9f,
                    (_this, args) => ((InteractiveShadowBase)_this).shadow.Opacity = (float)args.NewValue));

        public float ShadowThickness
        {
            get { return (float)this.GetValue(ShadowThicknessProperty); }
            set { this.SetValue(ShadowThicknessProperty, value); }
        }
    }
}
