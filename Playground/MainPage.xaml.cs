using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

// This is all rubbish.

namespace Playground
{
    public sealed partial class MainPage : Page
    {
        Compositor Compositor;
        ContainerVisual canvasRootVisual;

        public MainPage()
        {
            InitializeComponent();
            SetupComposition();

            var shadowSprite = Compositor.CreateSpriteVisual();
            shadowSprite.Offset = new Vector3(300, 200, 0);
            shadowSprite.Size = new Vector2(150);
            shadowSprite.Brush = Compositor.CreateColorBrush(Colors.DarkViolet);
            var shadow = SetupShadow(shadowSprite);

            var shadowIntensify = Compositor.CreateScalarKeyFrameAnimation();
            shadowIntensify.InsertKeyFrame(1.0f, 90.0f);
            shadowIntensify.Duration = TimeSpan.FromMilliseconds(1200.0);
            var easing = Compositor.CreateCubicBezierEasingFunction(new Vector2(0.0f, 1.05f), new Vector2(0.2f, 1.0f));
            var linearEasing = Compositor.CreateLinearEasingFunction();

            var rectPointerState = ElementCompositionPreview.GetPointerPositionPropertySet(canvas);
            canvas.PointerMoved += (sender, args) =>
            {
                switch (rectPointerState.TryGetVector3("Position", out var val))
                {
                    case CompositionGetValueStatus.Succeeded:
                        System.Diagnostics.Debug.WriteLine(val);
                        break;
                    case CompositionGetValueStatus.NotFound:
                        System.Diagnostics.Debug.WriteLine("Nothing");
                        break;
                    case CompositionGetValueStatus.TypeMismatch:
                        System.Diagnostics.Debug.WriteLine("type mismatch");
                        break;
                }
            };

            btn.PointerEntered += (sender, args) =>
            {
                shadowIntensify.ClearAllParameters();
                shadowIntensify.InsertKeyFrame(1.0f, 90.0f, easing);
                shadow.StartAnimation("BlurRadius", shadowIntensify);
            };

            btn.PointerExited += (sender, args) =>
            {
                shadowIntensify.ClearAllParameters();
                shadowIntensify.InsertKeyFrame(1.0f, 0.0f, easing);
                shadow.StartAnimation("BlurRadius", shadowIntensify);
            };

            btn.Click += (sender, args) =>
            {
                shadowSprite.RotationAxis = new Vector3(0.0f, 1.0f, 0.0f);
                shadowSprite.CenterPoint = new Vector3(shadowSprite.Size / 2, -20.0f);
                shadowSprite.TransformMatrix =
                    Matrix4x4.Identity;
                var rotate = Compositor.CreateScalarKeyFrameAnimation();
                rotate.Duration = TimeSpan.FromMilliseconds(500.0);
                rotate.InsertKeyFrame(1.0f, 30.0f, easing);
                shadowSprite.StartAnimation(nameof(shadowSprite.RotationAngleInDegrees), rotate);

            };

            canvasRootVisual.Children.InsertAtTop(shadowSprite);
        }

        void SetupComposition()
        {
            Compositor = this.GetVisual().Compositor;
            canvasRootVisual = Compositor.CreateContainerVisual();
            canvas.SetElementChildVisual(canvasRootVisual);
        }

        DropShadow SetupShadow(SpriteVisual sprite)
        {
            var shadow = Compositor.CreateDropShadow();
            var mask = Compositor.CreateMaskBrush();
            shadow.Offset = new Vector3(0.0f, 0.0f, 0.0f);
            shadow.Color = Colors.Black;
            shadow.BlurRadius = 0.0f;
            shadow.Opacity = 0.90f;
            sprite.Shadow = shadow;
            return shadow;
        }

        void SetupShittyTiltingEffect(FrameworkElement _this)
        {
            var _visual = _this.GetVisual();
            var _compositor = Compositor;
            _visual.CenterPoint = new Vector3((float)_this.Width / 2, (float)_this.Height / 2, 0.0f);
            _visual.TransformMatrix = Matrix4x4.CreateLookAt(
                cameraPosition: new Vector3(0, 0, 1),
                cameraTarget: new Vector3(0, 0, 0),
                cameraUpVector: new Vector3(0, 1, 0));

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

            _this.PointerExited += (sender, args) =>
            {
                _visual.RotationAngleInDegrees = 0.0f;
            };
        }
    }
}
