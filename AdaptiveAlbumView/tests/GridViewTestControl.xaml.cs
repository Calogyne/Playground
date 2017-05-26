using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AdaptiveAlbumView.tests
{
    public sealed partial class GridViewTestControl : UserControl
    {
        Random rnd;
        Color[] c = new Color[] { Colors.DarkOrange, Colors.DeepSkyBlue, Colors.Lime, Colors.DarkOrchid,  Colors.DeepPink, Colors.DarkMagenta, Colors.Crimson };

        public GridViewTestControl()
        {
            this.InitializeComponent();
            rnd = new Random();
            addButton.Click += (sender, args) => myGridView.OnChildAdded();

            var rect1_visual = rect1.GetVisual();
            var compositor = rect1_visual.Compositor;

            var sprite = compositor.CreateSpriteVisual();
            sprite.Brush = compositor.CreateColorBrush(Colors.AliceBlue);
            sprite.Size = new System.Numerics.Vector2(150, 150);
            rect1.SetElementChildVisual(sprite);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var colorRect = new Windows.UI.Xaml.Shapes.Rectangle
            {
                Fill = new SolidColorBrush(c[rnd.Next(0, c.Length - 1)]),
                Transitions = new TransitionCollection() { new EntranceThemeTransition() }
            };

            myGridView.Children.Add(colorRect);
        }
    }
}
