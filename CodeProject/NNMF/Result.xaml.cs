using NNMFSearchResultClustering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NNMFSearchResultClustering
{
	/// <summary>
	/// Interaction logic for Result.xaml
	/// </summary>
	public partial class Result : Border
	{
        readonly AAAIDocument _document;

        public Result(AAAIDocument document, Color[] colourList)
		{
            _document = document;
			InitializeComponent();

            float topWeight = document.ClusterMembership.Max();
			for(int i = 0; i < document.ClusterMembership.Length; i++) {
				RectangleGeometry rect = new RectangleGeometry(new Rect(0, 0, 16, 16), 4, 4);
				Path path = new Path();
				path.Fill = new SolidColorBrush(colourList[i%colourList.GetLength(0)]);
				path.Margin = new Thickness(0, 0, 2, 2);
				path.Stroke = Brushes.DimGray;
				path.StrokeThickness = 1;
				path.Data = rect;
				path.Opacity = document.ClusterMembership[i] / topWeight;
				path.SnapsToDevicePixels = true;
				topPanel.Children.Insert(0, path);
			}

			titleTextBlock.Text = document.Title;
            textTextBlock.Text = String.Join(", ", document.Group);
            urlTextBlock.Text = document.Abstract;

            this.MouseDown += delegate (object sender, MouseButtonEventArgs e) {
                if (Clicked != null)
                    Clicked(this, document.Title);
            };
        }

        public AAAIDocument Document { get { return _document; } }

        public delegate void ClickedDelegate(object sender, string query);
		public event ClickedDelegate Clicked;

	}
}