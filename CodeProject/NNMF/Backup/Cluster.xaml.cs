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
	/// Interaction logic for Cluster.xaml
	/// </summary>

	public partial class Cluster : Border
	{
		public Cluster(string text, Color colour, int featureIndex)
		{
			InitializeComponent();

			this.Resources.Add("backColor", colour);
			backBrush.Color = colour;
			textContent.Inlines.Add(new Run(text));

			this.MouseDown += delegate(object sender, MouseButtonEventArgs e) {
				if(Clicked != null)
					Clicked(this, featureIndex);
			};
		}

		public delegate void ClickedDelegate(object sender, int featureIndex);
		public event ClickedDelegate Clicked;

	}
}