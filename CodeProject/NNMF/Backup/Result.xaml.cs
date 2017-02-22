using System;
using System.Collections.Generic;
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
		public Result(string title, string text, string href, string url, float[] weights, float topWeight, Color[] colourList)
		{
			InitializeComponent();

			float myTopWeight = float.MinValue;
			for(int i = 0; i < weights.Length; i++)
				if(weights[i] > myTopWeight)
					myTopWeight = weights[i];
			float weight = myTopWeight = (myTopWeight != 0) ? myTopWeight : 1;

			for(int i = 0; i < weights.Length; i++) {
				RectangleGeometry rect = new RectangleGeometry(new Rect(0, 0, 16, 16), 4, 4);
				Path path = new Path();
				path.Fill = new SolidColorBrush(colourList[i%colourList.GetLength(0)]);
				path.Margin = new Thickness(0, 0, 2, 2);
				path.Stroke = Brushes.DimGray;
				path.StrokeThickness = 1;
				path.Data = rect;
				path.Opacity = weights[i] / weight;
				path.SnapsToDevicePixels = true;
				topPanel.Children.Insert(0, path);
			}

			titleTextBlock.Text = title;
			if(String.IsNullOrEmpty(text))
				mainPanel.Children.Remove(textTextBlock);
			else
				textTextBlock.Text = text;
			urlTextBlock.Text = url;
			urlTextBlock.Text = href;

			this.MouseDown += delegate(object sender, MouseButtonEventArgs e) {
				if(Clicked != null)
					Clicked(this, url);
			};
		}

		public delegate void ClickedDelegate(object sender, string url);
		public event ClickedDelegate Clicked;

	}
}