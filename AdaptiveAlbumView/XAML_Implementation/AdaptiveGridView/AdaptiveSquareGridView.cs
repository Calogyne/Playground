using System;
using static System.Diagnostics.Debug;
using System.Numerics;
using Windows.Foundation.Collections;
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
        Compositor compositor;
        ImplicitAnimationCollection layoutImplicitAnimations;

        public AdaptiveSquareGridView() : base()
        {
            SetupComposition();
            //this.Children.VectorChanged += OnItemsModified;
            
        }

        void SetupComposition()
        {
            compositor = this.GetVisual().Compositor;

            var easeInOutQuad = compositor.CreateEaseInOutQuadFunction(); 

            var repositionAnimation = compositor.CreateVector3KeyFrameAnimation();
            repositionAnimation.Target = "Offset";
            repositionAnimation.Duration = LayoutAnimationDuration;
            repositionAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", easeInOutQuad);

            var resizeAnimation = compositor.CreateVector2KeyFrameAnimation();
            resizeAnimation.Target = "Size";
            resizeAnimation.Duration = LayoutAnimationDuration;
            repositionAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", easeInOutQuad);

            layoutImplicitAnimations = compositor.CreateImplicitAnimationCollection();
            layoutImplicitAnimations["Offset"] = repositionAnimation;
            layoutImplicitAnimations["Size"] = resizeAnimation;
        }

        void OnItemsModified(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            this.InvalidateMeasure();
            switch (@event.CollectionChange)
            {
                case CollectionChange.ItemInserted:
                    var newItem = (UIElement)sender[(int)@event.Index];
                    var _visual = newItem.GetVisual();
                    _visual.ImplicitAnimations = layoutImplicitAnimations;
                    break;
                default:
                    break;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var side = availableSize.Width / this.DesiredColumnCount;
            var itemAvailableSize = new Size(side, side);
            foreach (UIElement item in Children)
                item.Measure(itemAvailableSize);

            var rowCount = (Children.Count % DesiredColumnCount == 0 ? 0 : 1) + Children.Count / DesiredColumnCount;
            var newSize = new Size(availableSize.Width, rowCount * side);
            return newSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0) return base.ArrangeOverride(finalSize);

            var l = finalSize.Width / DesiredColumnCount;

            (int i, int row) current = (0, 0);
            
            foreach (UIElement item in Children)
            {
                double x = current.i % DesiredColumnCount * l,
                       y = current.row * l;
                Rect newRect = new Rect(x, y, l, l);
                WriteLine($"{x}, {y}");
                item.Arrange(newRect);

                current.row += 
                    (current.i + 1) >= DesiredColumnCount && (current.i + 1) % DesiredColumnCount == 0 ? 1 : 0;
                current.i += 1;
            }
            
            return finalSize;
        }
    }
}
