using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Playground
{

    /// <summary>
    /// A word-in-progress, have yet figured it out.
    /// </summary>
    [TemplatePart(Name = Base, Type = typeof(Rectangle))]
    public sealed partial class InteractiveShadowBase : ContentControl
    {
        const string Base = "baseRect";
        Rectangle baseRect;
        Compositor compositor;
        SpriteVisual baseVisual;
        DropShadow shadow;

        public InteractiveShadowBase()
        {
            this.DefaultStyleKey = typeof(InteractiveShadowBase);
            compositor = this.GetVisual().Compositor;
            baseVisual = compositor.CreateSpriteVisual();
            baseVisual.Size = new Vector2(50);
            baseVisual.Brush = compositor.CreateColorBrush(Colors.AliceBlue);
            baseVisual.Offset = new Vector3(100, 100, 0);   
            shadow = compositor.CreateDropShadow();
            shadow.Opacity = 0.9f;
            shadow.BlurRadius = 95.0f;
            baseVisual.Shadow = shadow;
        }

        protected override void OnApplyTemplate()
        {
            baseRect = this.GetTemplateChild(Base) as Rectangle;
            var grid = (Grid)this.GetTemplateChild("grid");
            if (baseRect != null)
            {
                baseRect.SetElementChildVisual(baseVisual);
            }
            this.SetElementChildVisual(baseVisual);
            UpdateShadow();

            base.OnApplyTemplate();
        }

        void UpdateShadow()
        {
            // local function to save typing lel
            void setSize(FrameworkElement element) => 
                baseVisual.Size = new Vector2((float)element.Width, (float)element.Height);
            switch (this.Content)
            {
                case null:
                    break;
                case Image img:
                    setSize(img);
                    shadow.Mask = img.GetAlphaMask();
                    break;
                case Shape x:
                    setSize(x);
                    shadow.Mask = x.GetAlphaMask();
                    break;
                case FrameworkElement element:
                    setSize(element);
                    break;
                default:
                    break;
            }
    }
}
