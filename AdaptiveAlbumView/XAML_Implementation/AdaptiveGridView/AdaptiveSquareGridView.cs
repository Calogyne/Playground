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
    partial class AdaptiveSquareGridView : GridView
    {
        Compositor compositor;
        ImplicitAnimationCollection reposition;
        public AdaptiveSquareGridView()
        {
            this.Items.VectorChanged += OnItemsModified;
        }

        void SetupComposition()
        {
            compositor = this.GetVisual().Compositor;

            var repositionAnimation = compositor.CreateVector3KeyFrameAnimation();
            repositionAnimation.Target = "Offset";
            repositionAnimation.Duration = TimeSpan.FromMilliseconds(800.0);
            repositionAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", 
                compositor.CreateEaseInOutQuadFunction());

            reposition = compositor.CreateImplicitAnimationCollection();
            reposition["Offset"] = repositionAnimation;
        }

        void OnItemsModified(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            switch (@event.CollectionChange)
            {
                case CollectionChange.ItemInserted:
                    var newItem = (UIElement)sender[(int)@event.Index];
                    var _visual = newItem.GetVisual();
                    _visual.ImplicitAnimations = reposition;
                    break;
                default:
                    break;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var side = availableSize.Width / this.DesiredColumnCount;
            var itemAvailableSize = new Size(side, side);
            foreach (UIElement item in Items)
                item.Measure(itemAvailableSize);

            var rowCount = (Items.Count % DesiredColumnCount == 0 ? 0 : 1) + Items.Count / DesiredColumnCount;
            var newSize = new Size(availableSize.Width, rowCount * side);
            return newSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Items.Count == 0) return base.ArrangeOverride(finalSize);

            var l = finalSize.Width / DesiredColumnCount / 2;

            (int i, int row) current = (1, 1);
            foreach (UIElement item in Items)
            {
                (double x, double y) itemOrigin =
                    ((current.i - 1) % DesiredColumnCount * item.DesiredSize.Width + l,
                     (current.row - 1) * DesiredSize.Height + l);

                var newRect = new Rect(
                    itemOrigin.x, itemOrigin.y, 
                    item.DesiredSize.Width, item.DesiredSize.Height);
                item.Arrange(newRect);

                current.row += current.i >= DesiredColumnCount && current.i % DesiredColumnCount == 0 ? 1 : 0;
                current.i += 1;
            }

            return finalSize;
        }
    }
}
