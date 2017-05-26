using System;
using static System.Diagnostics.Debug;
using System.Numerics;
using static System.Math;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AdaptiveAlbumView.XAML
{
    partial class AdaptiveSquareGridView : Panel
    {
        Compositor compositor;
        ImplicitAnimationCollection layoutImplicitAnimations;
        int columnCount;
        double currentWidth;

        public AdaptiveSquareGridView() : base()
        {
            SetupComposition();
            this.UseLayoutRounding = false;
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

        public void OnChildAdded()
        {
            foreach (var child in Children)
                child.GetVisual().ImplicitAnimations = layoutImplicitAnimations;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0) return base.ArrangeOverride(finalSize);

            (int i, int row) current = (0, 0);
            
            foreach (UIElement item in Children)
            {
                //double additional = 

                double x = current.i % columnCount * currentWidth + ChildElementGap,
                       y = current.row * currentWidth + ChildElementGap;

                Rect newRect = new Rect(x, y, currentWidth - ChildElementGap, currentWidth - ChildElementGap);

                item.Arrange(newRect);

                current.row += 
                    (current.i + 1) >= columnCount && (current.i + 1) % columnCount == 0 ? 1 : 0;
                current.i += 1;
            }
            
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            columnCount = 
                (int)Ceiling((availableSize.Width - ChildElementGap) / (ChildElementMaxWidth + ChildElementGap));
            currentWidth = (availableSize.Width - ChildElementGap) / columnCount;

            var availableSizeForChild = 
                new Size(currentWidth - ChildElementGap, currentWidth - ChildElementGap);
            foreach (var child in Children)
                child.Measure(availableSizeForChild);

            var rowCount = (Children.Count % columnCount == 0 ? 0 : 1) + Children.Count / columnCount;
            var newSize = 
                new Size(availableSize.Width, rowCount * (currentWidth + ChildElementGap) + ChildElementGap);

            return newSize;
        }
    }
}
