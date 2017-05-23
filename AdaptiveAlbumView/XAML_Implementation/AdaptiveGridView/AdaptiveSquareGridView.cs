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
        public AdaptiveSquareGridView()
        {

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
