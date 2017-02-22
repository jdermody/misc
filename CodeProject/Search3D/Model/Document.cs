using Search3D.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Search3D.Models
{
	/// <summary>
	/// Represents a document in 3D space
	/// </summary>
	struct Document
	{
		double _x, _y, _z;
        readonly int _index;
        readonly AAAIDocument _document;
        readonly Color _colour;

        public Document(double x, double y, double z, int index, AAAIDocument document, Color colour)
		{
			_index = index;
            _document = document;
            _colour = colour;
			_x = x;
			_y = y;
			_z = z;
		}
		public double X
		{
			get
			{
				return _x;
			}
		}
		public double Y
		{
			get
			{
				return _y;
			}
		}
		public double Z
		{
			get
			{
				return _z;
			}
		}
		public int Index
		{
			get
			{
				return _index;
			}
		}
		public void Normalise(double minX, double rangeX, double minY, double rangeY, double minZ, double rangeZ)
		{
			_x = (_x - minX) / rangeX;
			_y = (_y - minY) / rangeY;
			_z = (_z - minZ) / rangeZ;
		}

        public AAAIDocument AAAIDocument { get { return _document; } }
        public Color Colour { get { return _colour; } }
    }
}
