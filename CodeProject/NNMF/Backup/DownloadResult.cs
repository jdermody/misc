using System;
using System.Collections.Generic;
using System.Text;

namespace NNMFSearchResultClustering
{
	/// <summary>
	/// A search result or article summary that contains a title, a URL and some text.  Also features, if applicable
	/// </summary>
	class DownloadResult
	{
		string _title, _href, _text, _url;
		int _index = -1;

		public DownloadResult(string title, string href, int index)
		{
			_title = title;
			_href = _url = href;
			_text = "";
			_index = index;
		}
		public DownloadResult(string title, string href, string url, string text, int index)
			: this(title, href, index)
		{
			_text = text;
			_url = url;
		}

		public string Title
		{
			get { return _title; }
		}
		public string Text
		{
			get { return _text; }
		}
		public string Href
		{
			get { return _href; }
		}
		public string Url
		{
			get { return _url; }
		}
		public int Index
		{
			get { return _index; }
		}
	}
}
