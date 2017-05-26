using System;
using System.Linq;
using System.Diagnostics;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;


// This is all rubbish.

namespace Playground
{
    public sealed partial class MainPage : Page
    {
        Compositor Compositor;
        ContainerVisual canvasRootVisual;
        SpriteVisual shadowSprite;
        CubicBezierEasingFunction easeInOutQuad;

        public MainPage()
        {
            InitializeComponent();
            SetupComposition();

            var (btn_onPointerEntered, btn_onPointerExited) = _setupButtonHoveringBehavior();
            var btn_onClicked = _setupButtonClickingBehavior();

            //var taylorSwift_1989 = 
            //    KnownFolders.MusicLibrary
            //    .GetFolderAsync("Purchases")
            //    .AsTask().Result
            //    .GetFolderAsync("Taylor Swift")
            //    .AsTask().Result
            //    .GetFolderAsync("1989")
            //    .AsTask().Result;
            //
            //var blank_space = taylorSwift_1989.GetFileAsync("02 Blank Space.mp3").AsTask().Result;
            //var coverStream =
            //    blank_space.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem)
            //    .AsTask().Result.CloneStream();
            //
            //var coverBitmap = new BitmapImage();
            //coverBitmap.SetSource(coverStream);
            //
            //albumImage.Source = coverBitmap;

            // commented out for now to prevent it from polluting debug console.
            //canvas.PointerMoved += reportPointerPositionRelativeTo_canvas;

            SetupShittyTiltingEffect(grid);
        }

        // via the composition api, which doesn't seem to work.
        void reportPointerPositionRelativeTo_canvas(object sender, PointerRoutedEventArgs args)
        {
            var rectPointerState = ElementCompositionPreview.GetPointerPositionPropertySet(canvas);
            switch (rectPointerState.TryGetVector3("Position", out var val))
            {
                case CompositionGetValueStatus.Succeeded:
                    Debug.WriteLine(val);
                    break;
                case CompositionGetValueStatus.NotFound:
                    Debug.WriteLine("Nothing");
                    break;
                case CompositionGetValueStatus.TypeMismatch:
                    Debug.WriteLine("type mismatch");
                    break;
            } 
        }

        (PointerEventHandler, PointerEventHandler) _setupButtonHoveringBehavior()
        {
            shadowSprite = Compositor.CreateSpriteVisual();

            shadowSprite.Offset = new Vector3(300, 200, 0);
            shadowSprite.Size = new Vector2(150);
            shadowSprite.CenterPoint = new Vector3(75.0f, 75.0f, 0.0f);
            shadowSprite.Brush = Compositor.CreateColorBrush(Colors.DarkMagenta); //Compositor.CreateGaussianBlurCompositionBrush(15.0f);

            var shadow = shadowSprite.SetupShadow();

            var animationDuration = TimeSpan.FromMilliseconds(1200.0);

            var shadowIntensify = Compositor.CreateScalarKeyFrameAnimation();
            shadowIntensify.InsertExpressionKeyFrame(1.0f, "isAppearing ? 70.0f : 0.0f", easeInOutQuad);
            shadowIntensify.Duration = animationDuration;

            var shadowAppears = Compositor.CreateScalarKeyFrameAnimation();
            shadowAppears.InsertExpressionKeyFrame(1.0f, "isAppearing ? 1.0f : 0.1f", easeInOutQuad);
            shadowAppears.Duration = animationDuration;

            var rising = Compositor.CreateVector3KeyFrameAnimation();
            rising.InsertExpressionKeyFrame(1.0f, "isRising ? Vector3(1.1f, 1.1f, 1.0f) : Vector3(1.0f, 1.0f, 1.0f)", easeInOutQuad);
            rising.Duration = animationDuration;

            void onPointerEntered(object sender, PointerRoutedEventArgs args)
            {
                shadowIntensify.SetBooleanParameter("isAppearing", true);
                shadow.StartAnimation("BlurRadius", shadowIntensify);

                shadowAppears.SetBooleanParameter("isAppearing", true);
                shadow.StartAnimation("Opacity", shadowAppears);

                rising.SetBooleanParameter("isRising", true);
                shadowSprite.StartAnimation("Scale", rising);
            };

            void onPointerExited(object sender, PointerRoutedEventArgs args)
            {
                shadowIntensify.SetBooleanParameter("isAppearing", false);
                shadow.StartAnimation("BlurRadius", shadowIntensify);

                shadowAppears.SetBooleanParameter("isAppearing", false);
                shadow.StartAnimation("Opacity", shadowAppears);

                rising.SetBooleanParameter("isRising", false);
                shadowSprite.StartAnimation("Scale", rising);
            };

            btn.PointerEntered += onPointerEntered;
            btn.PointerExited += onPointerExited;

            canvasRootVisual.Children.InsertAtTop(shadowSprite);

            return(onPointerEntered, onPointerExited);
        }

        PointerEventHandler _setupButtonClickingBehavior()
        {
            void onPointerClicked(object sender, RoutedEventArgs args)
            {
                shadowSprite.RotationAxis = new Vector3(1.0f, 0.0f, 0.0f);
                shadowSprite.CenterPoint = new Vector3(shadowSprite.Size / 2, 0.0f);
                var rotate = Compositor.CreateScalarKeyFrameAnimation();
                rotate.Duration = TimeSpan.FromMilliseconds(500.0);
                rotate.InsertKeyFrame(1.0f, 30.0f, easeInOutQuad);
                shadowSprite.StartAnimation(nameof(shadowSprite.RotationAngleInDegrees), rotate);
                //(blurryRect.Fill as GaussianBlurBrush).BlurAmount = 3.0f;
            };

            btn.Click += onPointerClicked;
            return onPointerClicked;
        }

        void SetupComposition()
        {
            Compositor = this.GetVisual().Compositor;
            canvasRootVisual = Compositor.CreateContainerVisual();
            canvas.SetElementChildVisual(canvasRootVisual);
            easeInOutQuad = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.0f, 1.05f), new Vector2(0.2f, 1.0f));
        }

        void SetupShittyTiltingEffect(FrameworkElement _this)
        {
            var _visual = _this.GetVisual();
            var _compositor = Compositor;
            _visual.CenterPoint = new Vector3((float)_this.Width / 2, (float)_this.Height / 2, 0.0f);
            _visual.TransformMatrix =
                Matrix4x4.Identity;

            _this.PointerMoved += (sender, args) =>
            {
                var position = args.GetCurrentPoint((FrameworkElement)sender).Position.ToVector2();
                var relativeP =
                    position - new Vector2((float)_this.Width, (float)_this.Height) / 2.0f;
                var axis = new Vector3(-relativeP.Y, relativeP.X, 0.0f);

                _visual.RotationAxis = axis;
                var rotation = _compositor.CreateScalarKeyFrameAnimation();
                rotation.InsertKeyFrame(1.0f, 25.0f, _compositor.CreateLinearEasingFunction());
                rotation.Duration = TimeSpan.FromMilliseconds(16.0f);
                _visual.StartAnimation(nameof(_visual.RotationAngleInDegrees), rotation);

                Debug.WriteLine($"Pointer: {position},\nRelative: {relativeP},\nRotation Axis: {axis}");
            };

            _this.PointerExited += (sender, args) => _visual.RotationAngleInDegrees = 0.0f;
        }

        private void grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var delta = new Vector3(e.Delta.Translation.ToVector2() * 100, 0.0f);
            var move = Compositor.CreateVector3KeyFrameAnimation();
            move.InsertExpressionKeyFrame(1.0f, "this.StartingValue + delta");
            //move.Duration = TimeSpan.FromMilliseconds(16.0);
            move.SetVector3Parameter("delta", delta);
            grid.GetVisual().StartAnimation("Offset", move);
        }
    }
}
