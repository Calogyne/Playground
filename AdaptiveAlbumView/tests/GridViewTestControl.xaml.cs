using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AdaptiveAlbumView.tests
{
    public sealed partial class GridViewTestControl : UserControl
    {
        Random rnd;
        Color[] c = new Color[] { Colors.DarkOrange, Colors.Navy, Colors.ForestGreen, Colors.Plum, Colors.DarkViolet, Colors.Azure, Colors.AntiqueWhite, Colors.Crimson };

        public GridViewTestControl()
        {
            this.InitializeComponent();
            rnd = new Random();
            addButton.Click += (sender, args) => myGridView.OnChildAdded();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var colorRect = new Windows.UI.Xaml.Shapes.Rectangle();
            colorRect.Fill = new SolidColorBrush(c[rnd.Next(0, c.Length - 1)]);

            myGridView.Children.Add(colorRect);
        }
    }
}
