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
using System.Windows.Shapes;
using System.Xml;
using System.Web;

namespace NNMFSearchResultClustering
{
	public partial class MainWindow : System.Windows.Window
	{
		public delegate void SimpleDelegate();
		List<DownloadResult> _resultList = new List<DownloadResult>();
		Matrix _weightMatrix = null, _featureMatrix = null;
		float _topWeight = 0;
		Color[] _colourList = {
			Color.FromRgb(0xff, 0x90, 0x00),
			Color.FromRgb(0x62, 0xBA, 0x5B),
			Color.FromRgb(0xCF, 0x5B, 0x4B),
			Color.FromRgb(0x78, 0xA9, 0xCC),
			Color.FromRgb(0xFF, 0xCD, 0x20),
			Color.FromRgb(0x76, 0x56, 0xB7),
			Color.FromRgb(0xF2, 0x7F, 0xD8)
		};
		const int FEATURE_COUNT = 7;

		public MainWindow()
		{
			InitializeComponent();

			searchTextBox.KeyDown += new KeyEventHandler(OnSearchTextBoxKeyDown);
			searchTextBox.TextChanged += new TextChangedEventHandler(OnSearchTextChanged);
			searchBtn.Click += new RoutedEventHandler(OnSearchButtonClick);

			_DownloadResource("http://feeds.delicious.com/v2/xml/popular", new AsyncDownload.CallbackDelegate(_ParseDelicousXml));
		}

		void OnSearchTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Return)
				_DoSearch();
		}

		void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			searchBtn.IsEnabled = (searchTextBox.Text.Length > 0);
		}

		void OnSearchButtonClick(object sender, RoutedEventArgs e)
		{
			_DoSearch();
		}

		private void _DoSearch()
		{
			string searchText = searchTextBox.Text;
			if(searchText.Length > 0) {
				string url = String.Format("http://www.dogbluesoftware.com/services/SearchAndTag.ashx?atf=1&q={0}", HttpUtility.UrlEncode(searchText));
				_DownloadResource(url, new AsyncDownload.CallbackDelegate(_ParseSearchResults));
			}
		}

		private void _ParseDelicousXml(string xml)
		{
			bool shouldUpdate = false;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			DocumentTermModel model = new DocumentTermModel();
			int docIndex = 0;
			foreach(XmlNode node in doc.SelectNodes("/rss/channel/item")) {
				XmlElement titleAtt = node["title"];
				XmlElement hrefAtt = node["link"];
				if(titleAtt != null && hrefAtt != null) {
					string title = titleAtt.InnerText, href = hrefAtt.InnerText;
					if(!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(href)) {
						DownloadResult result = new DownloadResult(title, href, docIndex++);
						DocumentTermModel.Document d = model.AddDocument(result);
						foreach(XmlNode catNode in node.SelectNodes("category")) {
							string text = catNode.InnerText;
							if(!String.IsNullOrEmpty(text))
								model.AddTerm(d, text, 1);
						}
						_resultList.Add(result);
						shouldUpdate = true;
					}
				}
			}
			if(shouldUpdate) {
				Dispatcher.Invoke((SimpleDelegate)delegate() {
					_UpdateUI(model);
				});
			}
		}

		private void _ParseSearchResults(string xml)
		{
			bool shouldUpdate = false;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			int docIndex = 0;
			DocumentTermModel model = new DocumentTermModel();
			foreach(XmlNode node in doc.SelectNodes("/data/result")) {
				XmlAttribute titleAtt = node.Attributes["title"];
				XmlAttribute hrefAtt = node.Attributes["disp"];
				XmlAttribute urlAtt = node.Attributes["url"];
				XmlAttribute textAtt = node.Attributes["text"];
				if(titleAtt != null && hrefAtt != null && textAtt != null) {
					string title = titleAtt.Value, href = hrefAtt.Value, text = textAtt.Value, url = urlAtt.Value;
					if(!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(href) && !String.IsNullOrEmpty(url)) {
						DownloadResult result = new DownloadResult(title, href, url, text, docIndex++);
						DocumentTermModel.Document d = model.AddDocument(result);
						foreach(XmlNode catNode in node.SelectNodes("t")) {
							string tagText = catNode.Attributes["text"].Value;
							uint count = 1;
							if(catNode.Attributes["c"] != null)
								count = Convert.ToUInt32(catNode.Attributes["c"].Value);
							if(!String.IsNullOrEmpty(tagText))
								model.AddTerm(d, tagText, count);
						}
						_resultList.Add(result);
						shouldUpdate = true;
					}
				}
			}
			if(shouldUpdate) {
				Dispatcher.Invoke((SimpleDelegate)delegate() {
					_UpdateUI(model);
				});
			}
		}

		/// <summary>
		/// Orders features from the feature matrix into a ranked list
		/// </summary>
		/// <param name="featureMatrix"></param>
		/// <param name="featureIndex"></param>
		/// <returns></returns>
		private List<int> _RankFeatureWordList(Matrix featureMatrix, int featureIndex)
		{
			// put the values into the tree
			SortedDictionary<float, List<int>> sorter = new SortedDictionary<float, List<int>>();
			for(int i = 0; i < featureMatrix.ShapeX; i++) {
				List<int> list = null;
				float val = featureMatrix[i, featureIndex];
				if(val > 0) {
					if(!sorter.TryGetValue(val, out list))
						sorter.Add(val, (list = new List<int>()));
					list.Add(i);
				}
			}

			// since we want a descending list, reverse the list at each point, so that the final reverse will preserve relative ordering
			List<int> ret = new List<int>();
			foreach(KeyValuePair<float, List<int>> item in sorter) {
				item.Value.Reverse();
				foreach(int index in item.Value)
					ret.Add(index);
			}
			ret.Reverse();
			return ret;
		}

		private void _DownloadResource(string url, AsyncDownload.CallbackDelegate callback)
		{
			_resultList.Clear();
			_weightMatrix = null;
			_featureMatrix = null;
			_topWeight = 0;
			tagCloudContainer.Children.Clear();

			progressBar.Visibility = Visibility.Visible;
			browserFrame.Visibility = Visibility.Collapsed;
			AsyncDownload.DownloadUrl(url, callback);
		}

		private void _UpdateUI(DocumentTermModel model)
		{
			progressBar.Visibility = Visibility.Collapsed;
			browserFrame.Visibility = Visibility.Visible;

			NNMFMatrix termDocument = model.GetNormalised();
			termDocument.Factorise(FEATURE_COUNT, out _weightMatrix, out _featureMatrix);

			// calculate the maximum weight across all documents
			_topWeight = 0;
			for(int i = 0; i < _weightMatrix.ShapeX; i++) {
				for(int j = 0; j < _weightMatrix.ShapeY; j++) {
					float val = _weightMatrix[i, j];
					if(val > _topWeight)
						_topWeight = val;
				}
			}

			// build the list of clusters
			for(int i = 0; i < FEATURE_COUNT; i++) {
				StringBuilder sb = new StringBuilder();
				List<int> wordList = _RankFeatureWordList(_featureMatrix, i);
				foreach(int index in wordList) {
					string word = model.GetTerm(index);
					if(sb.Length > 0)
						sb.Append(", ");
					sb.Append(word);
				}
				Cluster cluster = new Cluster(sb.ToString(), _colourList[i%7], i);
				cluster.Clicked += new Cluster.ClickedDelegate(Cluster_Clicked);
				tagCloudContainer.Children.Add(cluster);
			}

			_ShowResults(_resultList);
		}

		private void _ShowResults(List<DownloadResult> resultList)
		{
			ScrollViewer scrollViewer = new ScrollViewer();
			scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			StackPanel resultPanel = new StackPanel();
			scrollViewer.Content = resultPanel;
			resultPanel.Orientation = Orientation.Vertical;
			foreach(DownloadResult result in resultList) {
				Result r = new Result(result.Title.Trim(), result.Text.Trim(), result.Href.Trim(), result.Url, _weightMatrix.GetRow(result.Index), _topWeight, _colourList);
				r.Clicked += new Result.ClickedDelegate(Result_Clicked);
				resultPanel.Children.Add(r);
			}
			browserFrame.Navigate(scrollViewer);
		}

		void Cluster_Clicked(object sender, int featureIndex)
		{
			// sort the documents by membership in the specified cluster
			SortedDictionary<float, List<DownloadResult>> sorter = new SortedDictionary<float, List<DownloadResult>>();
			for(int i = 0; i < _weightMatrix.ShapeY; i++) {
				float weight = _weightMatrix[featureIndex, i];
				DownloadResult result = _resultList[i];
				List<DownloadResult> list;
				if(!sorter.TryGetValue(weight, out list))
					sorter.Add(weight, list = new List<DownloadResult>());
				list.Add(result);
			}

			List<DownloadResult> sortedList = new List<DownloadResult>();
			foreach(KeyValuePair<float, List<DownloadResult>> item in sorter) {
				item.Value.Reverse();
				foreach(DownloadResult result in item.Value)
					sortedList.Add(result);
			}
			sortedList.Reverse();
			_ShowResults(sortedList);
		}

		void Result_Clicked(object sender, string url)
		{
			System.Diagnostics.Process.Start(url);
		}
	}
}