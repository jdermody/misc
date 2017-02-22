using Search3D.Models;
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

namespace Search3D
{
	/// <summary>
	/// A search result shows the title, text etc
	/// </summary>
	public partial class SearchResult : UserControl
	{
		readonly AAAIDocument _document;
		readonly int _index;

		public SearchResult(AAAIDocument document, int index)
		{
            _index = index;
            _document = document;

			InitializeComponent();
			tbTitle.Text = document.Title;
			tbText.Text = document.Abstract;
			tbUrl.Text = String.Join(", ", document.Group);

			borderMain.MouseLeftButtonDown += new MouseButtonEventHandler(borderMain_MouseLeftButtonDown);
			borderMain.MouseEnter += new MouseEventHandler(borderMain_MouseEnter);
			borderMain.MouseLeave += new MouseEventHandler(borderMain_MouseLeave);
		}

		void borderMain_MouseLeave(object sender, MouseEventArgs e)
		{
            MouseHoverEvent?.Invoke(_index, false, gsBottom.Color);
        }

		void borderMain_MouseEnter(object sender, MouseEventArgs e)
		{
            MouseHoverEvent?.Invoke(_index, true, gsBottom.Color);
        }

		void borderMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            System.Diagnostics.Process.Start("http://www.google.com/search?q=" + Uri.EscapeDataString(_document.Title));
        }

		public Color Colour
		{
			set
			{
				gsBottom.Color = value;
			}
			get
			{
				return gsBottom.Color;
			}
		}

		public delegate void MouseHoverDelegate(int index, bool hover, Color defaultColour);
		public event MouseHoverDelegate MouseHoverEvent;
	}
}