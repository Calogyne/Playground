using System;
using System.Numerics;
using static System.Math;
using static System.Diagnostics.Debug;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.UI.Xaml.Input;

namespace AdaptiveAlbumView.XAML
{
    partial class AlbumItem : FrameworkElement
    {
        #region basic
        Compositor compositor;

        SpriteVisual background;
        #endregion

        #region content
        static CompositionColorBrush backgroundColorBrush =
            Window.Current.Compositor.CreateColorBrush(Color.FromArgb(205, 0, 0, 0));


        SpriteVisual cover;

        static readonly string fontFamily = "Segoe UI";

        static CompositionGraphicsDevice textDrawingDevice =
            CanvasComposition.CreateCompositionGraphicsDevice(
                Window.Current.Compositor, CanvasDevice.GetSharedDevice());

        SpriteVisual albumNameText;

        SpriteVisual artistText;

        SpriteVisual lengthText;

        bool IsMouseHovering = false;
        #endregion

        public (PointerEventHandler, PointerEventHandler) x;

        public AlbumItem() : base()
        {
            compositor = this.GetVisual().Compositor;
            background = compositor.CreateSpriteVisual();
            background.Brush = backgroundColorBrush;
            var recenter = compositor.CreateExpressionAnimation(
                "Vector3(This.Target.Size.X / 2, This.Target.Size.Y / 2, 0.0f)");
            background.StartAnimation("CenterPoint", recenter);
            x = setupHoveringBehavior();


            this.SetElementChildVisual(background);
            SizeChanged += (sender, args) => background.Size = args.NewSize.ToVector2();
            
            setupAlbumNameText();

        }
        

        void setupAlbumNameText()
        {
            albumNameText = compositor.CreateSpriteVisual();
            albumNameText.Size = new Vector2(500.0f, 100.0f);

            var reposition = compositor.CreateExpressionAnimation
                ("Vector3(20.0f, background.Size.Y - 100.0f, 0.0f)");
            reposition.SetReferenceParameter(nameof(background), background);

            albumNameText.StartAnimation("Offset", reposition);

            background.Children.InsertAtTop(albumNameText);

            var td = new TextDrawer(textDrawingDevice);
            albumNameText.Brush =
                compositor.CreateSurfaceBrush(td.DrawingSurface);

            //albumNameText.Brush = compositor.CreateColorBrush(Colors.Wheat);

        }

        (PointerEventHandler, PointerEventHandler) setupHoveringBehavior()
        {
            var shadow = background.SetupShadow();

            var easeInOutQuad = compositor.CreateCubicBezierEasingFunction(new Vector2(0.0f, 1.05f), new Vector2(0.2f, 1.0f));

            var animationDuration = TimeSpan.FromMilliseconds(1200.0);

            var shadowIntensify = compositor.CreateScalarKeyFrameAnimation();
            shadowIntensify.InsertExpressionKeyFrame(1.0f, "isAppearing ? 70.0f : 0.0f", easeInOutQuad);
            shadowIntensify.Duration = animationDuration;

            var shadowAppears = compositor.CreateScalarKeyFrameAnimation();
            shadowAppears.InsertExpressionKeyFrame(1.0f, "isAppearing ? 1.0f : 0.1f", easeInOutQuad);
            shadowAppears.Duration = animationDuration;

            var rising = compositor.CreateVector3KeyFrameAnimation();
            rising.InsertExpressionKeyFrame(1.0f, "isRising ? Vector3(1.1f, 1.1f, 1.0f) : Vector3(1.0f, 1.0f, 1.0f)", easeInOutQuad);
            rising.Duration = animationDuration;

            void onPointerEntered(object sender, PointerRoutedEventArgs args)
            {
                shadowIntensify.SetBooleanParameter("isAppearing", true);
                shadow.StartAnimation("BlurRadius", shadowIntensify);

                shadowAppears.SetBooleanParameter("isAppearing", true);
                shadow.StartAnimation("Opacity", shadowAppears);

                rising.SetBooleanParameter("isRising", true);
                background.StartAnimation("Scale", rising);
                WriteLine("cursor did enter");
            };

            void onPointerExited(object sender, PointerRoutedEventArgs args)
            {
                shadowIntensify.SetBooleanParameter("isAppearing", false);
                shadow.StartAnimation("BlurRadius", shadowIntensify);

                shadowAppears.SetBooleanParameter("isAppearing", false);
                shadow.StartAnimation("Opacity", shadowAppears);

                rising.SetBooleanParameter("isRising", false);
                background.StartAnimation("Scale", rising);
                WriteLine("cursor did exit");
            };

            this.PointerPressed += onPointerEntered;
            this.PointerReleased += onPointerExited;

            return (onPointerEntered, onPointerExited);
        }
    }
}
