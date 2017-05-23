using System;
using System.Linq;
using System.Diagnostics;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.Foundation;

namespace AdaptiveAlbumView.XAML_Implementation
{

    partial class AdaptiveSquareGridView : GridView
    {
        static DependencyProperty DesiredColumnCountProperty = DependencyProperty.Register(
            "DesiredColumnCount", typeof(int), typeof(AdaptiveSquareGridView),
            new PropertyMetadata(4, 
                (d, args) =>
                {
                    if ((int)args.NewValue <= 0) throw new ArgumentException("Cannot be <= 0.");
                    var _this = (AdaptiveSquareGridView)d;
                    _this.InvalidateMeasure();
                }));

        public int DesiredColumnCount
        {
            get => (int)this.GetValue(DesiredColumnCountProperty);
            set => this.SetValue(DesiredColumnCountProperty, value);
        }
    }
}
