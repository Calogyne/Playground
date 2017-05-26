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

namespace AdaptiveAlbumView.XAML
{
    partial class AdaptiveSquareGridView : Panel
    {
        static readonly DependencyProperty ChildElementMaxWidthProperty = DependencyProperty.Register(
            "ChildElementMaxWidth", typeof(double), typeof(AdaptiveSquareGridView),
            new PropertyMetadata(300.0, 
                (d, args) => (d as AdaptiveSquareGridView)?.InvalidateMeasure()));

        public double ChildElementMaxWidth
        {
            get => (double)GetValue(ChildElementMaxWidthProperty);
            set => SetValue(ChildElementMaxWidthProperty, value);
        }


        static readonly DependencyProperty ChildElementGapProperty = DependencyProperty.Register(
            "ChildElementGap", typeof(double), typeof(AdaptiveSquareGridView),
            new PropertyMetadata(0.0,
                (d, args) => (d as AdaptiveSquareGridView)?.InvalidateMeasure()));

        public double ChildElementGap
        {
            get => (double)GetValue(ChildElementGapProperty);
            set => SetValue(ChildElementGapProperty, value);
        }


        static readonly DependencyProperty LayoutAnimationDurationProperty = DependencyProperty.Register(
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
