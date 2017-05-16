using System;
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
    }
}
