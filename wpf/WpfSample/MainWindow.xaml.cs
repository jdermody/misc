using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSample
{
    /// <summary>
    /// Window that allows the user to generate some random numbers and calculate basic statistics on the numbers
    /// </summary>
    public partial class MainWindow : Window
    {
        sealed class ViewModel : INotifyPropertyChanged
        {
            int _count = 52;
            ObservableCollection<int> _results = new ObservableCollection<int>();
            NumericAnalysis _analysis;

            public int Count {
                get => _count;
                set {
                    _count = value;
                    OnPropertyChanged();
                }
            }

            public ObservableCollection<int> Numbers {
                get => _results;
                set {
                    _results = value;
                    OnPropertyChanged();
                }
            }

            public NumericAnalysis Analysis {
                get => _analysis;
                set {
                    _analysis = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        readonly ViewModel _viewModel = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _viewModel;
        }

        private void GenerateNumbers(object sender, RoutedEventArgs e)
        {
            _viewModel.Numbers.Clear();
            NumericAnalysis analysis = null;

            if (_viewModel.Count > 0) {
                var newArray = RandomNumberGenerator.Generate(1, _viewModel.Count);
                analysis = new NumericAnalysis();
                foreach (var item in newArray) {
                    _viewModel.Numbers.Add(item);
                    analysis.Add(item);
                }
            }

            _viewModel.Analysis = analysis;
        }
    }
}
