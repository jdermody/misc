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
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BrightWire;
using NNMFSearchResultClustering.Models;
using System.Linq;
using BrightWire.Helper;
using BrightWire.Models.Input;
using System.Collections.ObjectModel;

namespace NNMFSearchResultClustering
{
	public partial class MainWindow : System.Windows.Window
	{
		public delegate void SimpleDelegate();
		List<Result> _resultList = new List<Result>();
		Color[] _clusterColour = {
			Color.FromRgb(128, 200, 180),
			Color.FromRgb(135, 70, 194),
			Color.FromRgb(140, 210, 90),
            Color.FromRgb(200, 80, 147),
            Color.FromRgb(201, 169, 79),
			Color.FromRgb(112, 127, 189),
			Color.FromRgb(192, 82, 58),
			Color.FromRgb(83, 99, 60),
            Color.FromRgb(78, 45, 69),
            Color.FromRgb(202, 161, 169)
        };
        readonly ObservableCollection<string> _statusMessage = new ObservableCollection<string>();

		public MainWindow()
		{
			InitializeComponent();

            icMessage.ItemsSource = _statusMessage;
            Task.Run(() => _ClusterDataset());
		}

        void _ClusterDataset()
        {
            Dispatcher.Invoke((SimpleDelegate)delegate () {
                _statusMessage.Add("Downloading dataset...");
            });
            var uri = new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/00307/%5bUCI%5d%20AAAI-14%20Accepted%20Papers%20-%20Papers.csv");
            var KEYWORD_SPLIT = " \n".ToCharArray();
            var TOPIC_SPLIT = "\n".ToCharArray();

            // download the document list
            var docList = new List<AAAIDocument>();
            using (var client = new WebClient()) {
                var data = client.DownloadData(uri);

                Dispatcher.Invoke((SimpleDelegate)delegate () {
                    _statusMessage.Add("Building data table...");
                });

                // parse the file CSV
                var dataTable = new StreamReader(new MemoryStream(data)).ParseCSV(',');
                
                // create strongly typed documents from the data table
                dataTable.ForEach(row => docList.Add(new AAAIDocument {
                    Abstract = row.GetField<string>(5),
                    Keyword = row.GetField<string>(3).Split(KEYWORD_SPLIT, StringSplitOptions.RemoveEmptyEntries).Select(str => str.ToLower()).ToArray(),
                    Topic = row.GetField<string>(4).Split(TOPIC_SPLIT, StringSplitOptions.RemoveEmptyEntries),
                    Group = row.GetField<string>(2).Split(TOPIC_SPLIT, StringSplitOptions.RemoveEmptyEntries),
                    Title = row.GetField<string>(0)
                }));
            }

            // create a document lookup table
            var docTable = docList.ToDictionary(d => d.Title, d => d);

            // extract features from the document's metadata
            var stringTable = new StringTableBuilder();
            var classificationSet = new SparseVectorClassificationSet {
                Classification = docList.Select(d => d.AsClassification(stringTable)).ToArray()
            };

            // normalise the document/t
            var encodings = classificationSet.Vectorise(true);

            // convert the sparse feature vectors into dense vectors
            var documentClusterList = new List<DocumentCluster>();
            using (var lap = Provider.CreateLinearAlgebra()) {
                var lookupTable = encodings
                    .Select(d => Tuple.Create(d, lap.Create(d.Data).AsIndexable()))
                    .ToDictionary(d => d.Item2, d => docTable[d.Item1.Classification])
                ;
                var vectorList = lookupTable.Select(d => d.Key).ToList();

                Dispatcher.Invoke((SimpleDelegate)delegate () {
                    _statusMessage.Add("Clustering data...");
                });

                // cluster the dense vectors
                using (var nnmf = new NNMF(lap, vectorList, _clusterColour.Length)) {
                    var clusters = nnmf.Cluster(40, cost => {
                        Dispatcher.Invoke((SimpleDelegate)delegate () {
                            _statusMessage.Add("NNMF error: " + cost.ToString());
                        });
                    });

                    // create document clusters from the NNMF results
                    int index = 0;
                    foreach (var cluster in clusters) {
                        var documentCluster = new List<AAAIDocument>();
                        foreach (var item in cluster) {
                            var document = lookupTable[item];
                            documentCluster.Add(document);
                        }
                        var desc = String.Join(", ", nnmf.GetRankedFeatures(index++)
                            .Select(i => stringTable.GetString(i))
                            .Take(32)
                        );
                        documentClusterList.Add(new DocumentCluster(documentCluster, desc));
                    }

                    // collect the cluster membership for each document
                    for (int i = 0, len = vectorList.Count; i < len; i++)
                        lookupTable[vectorList[i]].ClusterMembership = nnmf.GetClusterMembership(i);
                }
            }

            Dispatcher.Invoke((SimpleDelegate)delegate () {
                _UpdateUI(documentClusterList);
            });
        }

        void _ShowResults(IEnumerable<Result> documentList)
        {
            panelResults.Children.Clear();
            foreach (var document in documentList)
                panelResults.Children.Add(document);
        }

		private void _UpdateUI(IReadOnlyList<DocumentCluster> clusters)
		{
			progressBar.Visibility = Visibility.Collapsed;
            svResults.Visibility = Visibility.Visible;
            icMessage.Visibility = Visibility.Collapsed;

            for (int i = 0; i < _clusterColour.Length; i++) {
                var item = clusters[i];
                var cluster = new Cluster(item.Description, _clusterColour[i % 7], i);
                cluster.Clicked += new Cluster.ClickedDelegate(Cluster_Clicked);
                tagCloudContainer.Children.Add(cluster);
            }
            _resultList.AddRange(clusters.SelectMany(c => c.Document.Select(d => {
                var ret = new Result(d, _clusterColour);
                ret.Clicked += Result_Clicked;
                return ret;
            })));
            _ShowResults(_resultList);
        }

		void Cluster_Clicked(object sender, int featureIndex)
		{
            _ShowResults(_resultList.OrderByDescending(r => r.Document.ClusterMembership[featureIndex]));
		}

		void Result_Clicked(object sender, string query)
		{
			System.Diagnostics.Process.Start("http://www.google.com/search?q=" + Uri.EscapeDataString(query));
		}
	}
}