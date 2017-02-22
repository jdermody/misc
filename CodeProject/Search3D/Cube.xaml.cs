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
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace Search3D
{
	/// <summary>
	/// A 3D model of a cube that can be assigned colours
	/// </summary>
	public partial class Cube : ModelVisual3D
	{
		int _index;
		Color _colour = Color.FromRgb(0xaa, 0xaa, 0xaa);

		public Cube()
		{
			InitializeComponent();
		}

		public Cube(double x, double y, double z, int index)
		{
			_index = index;

			InitializeComponent();

			transformMain.OffsetX = x;
			transformMain.OffsetY = y;
			transformMain.OffsetZ = z;
		}

		public double X
		{
			get
			{
				return transformMain.OffsetX;
			}
			set
			{
				transformMain.OffsetX = value;
			}
		}

		public double Y
		{
			get
			{
				return transformMain.OffsetY;
			}
			set
			{
				transformMain.OffsetY = value;
			}
		}

		public double Z
		{
			get
			{
				return transformMain.OffsetZ;
			}
			set
			{
				transformMain.OffsetZ = value;
			}
		}

		public Color Colour
		{
			set
			{
				_colour = value;
				brushMain.Brush = new SolidColorBrush(value);
			}
		}

		public int Index
		{
			get { return _index; }
		}

		public void Ping()
		{
			brushMain.Brush = new SolidColorBrush(Colors.Black);
		}

		public void Reset()
		{
			brushMain.Brush = new SolidColorBrush(_colour);
		}
	}
}