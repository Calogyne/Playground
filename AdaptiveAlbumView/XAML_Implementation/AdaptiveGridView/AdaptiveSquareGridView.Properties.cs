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

    partial class AdaptiveSquareGridView : Panel
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

        static DependencyProperty LayoutAnimationDurationProperty = DependencyProperty.Register(
            "LayoutAnimationDuration", typeof(TimeSpan), typeof(AdaptiveSquareGridView),
            new PropertyMetadata(TimeSpan.FromMilliseconds(500.0),
                (d, args) => 
                {
                    var _thisAnimationCollection = (d as AdaptiveSquareGridView).layoutImplicitAnimations;
                    if (_thisAnimationCollection != null)
                    {
                        var offset = (Vector3KeyFrameAnimation)_thisAnimationCollection["Offset"];
                        var resize = (Vector2KeyFrameAnimation)_thisAnimationCollection["Size"];
                        var newVal = (TimeSpan)args.NewValue;

                        offset.Duration = newVal;
                        resize.Duration = newVal;
                    }
                }));

        public TimeSpan LayoutAnimationDuration
        {
            get => (TimeSpan)this.GetValue(LayoutAnimationDurationProperty);
            set => this.SetValue(LayoutAnimationDurationProperty, value);
        }
    }
}
