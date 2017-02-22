using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using BrightWire;
using BrightWire.Helper;
using System.Linq;
using Search3D.Models;
using System.IO;
using BrightWire.Models.Input;
using System.Net;
using System.Collections.ObjectModel;

namespace Search3D
{
    public partial class Window1 : Window
    {
        readonly ObservableCollection<string> _statusMessage = new ObservableCollection<string>();
        readonly ModelViewer.Trackball trackball = new ModelViewer.Trackball();
        SearchResult[] _searchResult = null;
        Cube[] _cube = null;
        static Color[] COLOUR_LIST = {
            Color.FromRgb(0xff, 0x90, 0x00),
            Color.FromRgb(0x62, 0xBA, 0x5B),
            Color.FromRgb(0xCF, 0x5B, 0x4B),
            Color.FromRgb(0x78, 0xA9, 0xCC),
            Color.FromRgb(0xFF, 0xCD, 0x20),
            Color.FromRgb(0x76, 0x56, 0xB7),
            Color.FromRgb(0xF2, 0x7F, 0xD8),
            Color.FromRgb(0x91, 0x96, 0x49),
            Color.FromRgb(0x4F, 0x90, 0x93),
            Color.FromRgb(0xAC, 0x76, 0x40),
        };
        Cube _currentHilightedCube = null;

        public Window1()
        {
            InitializeComponent();

            // setup event handlers
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            myPerspectiveCamera.Transform = trackball.Transform;
            directionalLight.Transform = trackball.Transform;
            borderCapture.MouseMove += new MouseEventHandler(_CheckMouseInModel);
            icStatus.ItemsSource = _statusMessage;

            Task.Run(() => _AnalyseDataset());
        }

        void _AnalyseDataset()
        {
            Dispatcher.Invoke(() => {
                _statusMessage.Add("Downloading dataset...");
            });
            var uri = new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/00307/%5bUCI%5d%20AAAI-14%20Accepted%20Papers%20-%20Papers.csv");
            var KEYWORD_SPLIT = " \n".ToCharArray();
            var TOPIC_SPLIT = "\n".ToCharArray();

            // download the document list
            var docList = new List<AAAIDocument>();
            using (var client = new WebClient()) {
                var data = client.DownloadData(uri);

                Dispatcher.Invoke(() => {
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

            // create dense feature vectors and normalise along the way
            var encodings = classificationSet.Vectorise(true);

            using (var lap = Provider.CreateLinearAlgebra()) {
                var lookupTable = encodings.Select(d => Tuple.Create(d, lap.Create(d.Data))).ToDictionary(d => d.Item2, d => docTable[d.Item1.Classification]);
                var vectorList = lookupTable.Select(d => d.Key).ToList();

                // create a term/document matrix with terms as columns and documents as rows
                var matrix = lap.CreateMatrix(vectorList.Select(d => d.Data).ToList());

                Dispatcher.Invoke(() => {
                    _statusMessage.Add("Performing latent semantic analysis...");
                });

                // compute the SVD
                const int K = 3;
                var kIndices = Enumerable.Range(0, K).ToList();
                var matrixT = matrix.Transpose();
                var svd = matrixT.Svd();

                // create latent space
                var s = lap.CreateDiagonal(svd.S.AsIndexable().Values.Take(K).ToList());
                var v2 = svd.VT.GetNewMatrixFromRows(kIndices);
                using (var sv2 = s.Multiply(v2)) {
                    var vectorList2 = sv2.AsIndexable().Columns.ToList();
                    var lookupTable2 = vectorList2.Select((v, i) => Tuple.Create(v, vectorList[i])).ToDictionary(d => (IVector)d.Item1, d => lookupTable[d.Item2]);

                    // cluster the latent space
                    var clusters = vectorList2.KMeans(COLOUR_LIST.Length);
                    var clusterTable = clusters
                        .Select((l, i) => Tuple.Create(l, i))
                        .SelectMany(d => d.Item1.Select(v => Tuple.Create(v, d.Item2)))
                        .ToDictionary(d => d.Item1, d => COLOUR_LIST[d.Item2])
                    ;

                    // build the document list
                    var documentList = new List<Document>();
                    int index = 0;
                    double maxX = double.MinValue, minX = double.MaxValue, maxY = double.MinValue, minY = double.MaxValue, maxZ = double.MinValue, minZ = double.MaxValue;
                    foreach (var item in vectorList2) {
                        float x = item[0];
                        float y = item[1];
                        float z = item[2];
                        documentList.Add(new Document(x, y, z, index++, lookupTable2[item], clusterTable[item]));
                        if (x > maxX)
                            maxX = x;
                        if (x < minX)
                            minX = x;
                        if (y > maxY)
                            maxY = y;
                        if (y < minY)
                            minY = y;
                        if (z > maxZ)
                            maxZ = z;
                        if (z < minZ)
                            minZ = z;
                    }
                    double rangeX = maxX - minX;
                    double rangeY = maxY - minY;
                    double rangeZ = maxZ - minZ;
                    foreach (var document in documentList)
                        document.Normalise(minX, rangeX, minY, rangeY, minZ, rangeZ);

                    Dispatcher.Invoke(() => {
                        var numDocs = documentList.Count;
                        _cube = new Cube[numDocs];
                        _searchResult = new SearchResult[numDocs];

                        _statusMessage.Add("Creating 3D graph...");

                        var SCALE = 10;
                        for (var i = 0; i < numDocs; i++) {
                            var document = documentList[i];
                            var cube = _cube[i] = new Cube(SCALE * document.X, SCALE * document.Y, SCALE * document.Z, i);
                            var searchResult = _searchResult[i] = new SearchResult(document.AAAIDocument, i);
                            cube.Colour = document.Colour;
                            searchResult.Colour = document.Colour;

                            searchResult.MouseHoverEvent += new SearchResult.MouseHoverDelegate(searchResult_MouseHoverEvent);
                            viewPort.Children.Add(cube);
                        }

                        foreach (var item in _searchResult.OrderBy(sr => sr.Colour.GetHashCode()))
                            panelResults.Children.Add(item);

                        icStatus.Visibility = Visibility.Collapsed;
                        viewPort.Visibility = Visibility.Visible;
                        progress.Visibility = Visibility.Collapsed;
                    });
                }
            }
        }


        void _CheckMouseInModel(object sender, MouseEventArgs e)
        {
            // don't bother checking the mouse position if we are dragging
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                return;

            // see if the the mouse is over a cube
            // N.B we are looking at the border as it is superimposed over the viewPort
            Cube foundCube = null;
            SearchResult correspondingSearchResult = null;
            HitTestResult result = VisualTreeHelper.HitTest(viewPort, e.GetPosition(viewPort));
            RayHitTestResult rayResult = result as RayHitTestResult;
            if (rayResult != null) {
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
                if (rayMeshResult != null) {
                    GeometryModel3D model = rayMeshResult.ModelHit as GeometryModel3D;
                    for (int i = 0, len = _cube.Length; i < len; i++) {
                        var item = _cube[i];
                        if (item.Content == model) {
                            foundCube = item;
                            correspondingSearchResult = _searchResult[i];
                            break;
                        }
                    }
                }
            }

            // update the selection as appropriate
            if (foundCube != _currentHilightedCube) {
                if (_currentHilightedCube != null)
                    _currentHilightedCube.Reset();
                if (foundCube != null) {
                    correspondingSearchResult.BringIntoView();

                    SolidColorBrush brush = new SolidColorBrush(Colors.Red);
                    ColorAnimation animation = new ColorAnimation(Colors.Red, Color.FromRgb(0xff, 0xff, 0xff), new Duration(TimeSpan.FromMilliseconds(750)));
                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                    correspondingSearchResult.Background = brush;
                    foundCube.Ping();
                }
                _currentHilightedCube = foundCube;
            }
        }

        void searchResult_MouseHoverEvent(int index, bool hover, Color defaultColour)
        {
            var cube = _cube[index];
            if (hover)
                cube.Ping();
            else
                cube.Colour = defaultColour;
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            trackball.EventSource = borderCapture;
        }
    }
}