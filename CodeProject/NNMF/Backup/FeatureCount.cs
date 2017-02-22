using System;
using System.Collections.Generic;
using System.Text;

namespace NNMFSearchResultClustering
{
	/// <summary>
	/// Represents a feature and its occurrence
	/// </summary>
	struct FeatureCount
	{
		string _feature;
		uint _count;

		public FeatureCount(string feature, uint count)
		{
			_feature = feature;
			_count = count;
		}

		public string Feature { get { return _feature; } }
		public uint Count { get { return _count; } }
	}
}
