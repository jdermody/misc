using BrightWire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNMFSearchResultClustering
{
    class NNMF : IDisposable
    {
        readonly int _numClusters;
        readonly ILinearAlgebraProvider _lap;
        readonly IErrorMetric _costFunction;
        readonly IReadOnlyList<IIndexableVector> _data;
        readonly IMatrix _dataMatrix;
        IMatrix _weights, _features;

        public NNMF(ILinearAlgebraProvider lap, IReadOnlyList<IIndexableVector> data, int numClusters, IErrorMetric costFunction = null)
        {
            _lap = lap;
            _data = data;
            _numClusters = numClusters;
            _costFunction = costFunction ?? ErrorMetricType.RMSE.Create();

            // create the main matrix
            var rand = new Random();
            _dataMatrix = _lap.Create(data.Count, data.First().Count, (x, y) => data[x][y]);

            // create the weights and features
            _weights = _lap.Create(_dataMatrix.RowCount, _numClusters, (x, y) => Convert.ToSingle(rand.NextDouble()));
            _features = _lap.Create(_numClusters, _dataMatrix.ColumnCount, (x, y) => Convert.ToSingle(rand.NextDouble()));
        }

        public void Dispose()
        {
            _dataMatrix.Dispose();
            _weights.Dispose();
            _features.Dispose();
        }

        public IReadOnlyList<IReadOnlyList<IIndexableVector>> Cluster(int numIterations, Action<float> callback, float errorThreshold = 0.001f)
        {
            for (int i = 0; i < numIterations; i++) {
                using (var wh = _weights.Multiply(_features)) {
                    var cost = _DifferenceCost(_dataMatrix, wh);
                    callback(cost);
                    if (cost <= errorThreshold)
                        break;

                    using (var wT = _weights.Transpose())
                    using (var hn = wT.Multiply(_dataMatrix))
                    using (var wTw = wT.Multiply(_weights))
                    using (var hd = wTw.Multiply(_features))
                    using (var fhn = _features.PointwiseMultiply(hn)) {
                        _features.Dispose();
                        _features = fhn.PointwiseDivide(hd);
                    }

                    using (var fT = _features.Transpose())
                    using (var wn = _dataMatrix.Multiply(fT))
                    using (var wf = _weights.Multiply(_features))
                    using (var wd = wf.Multiply(fT))
                    using (var wwn = _weights.PointwiseMultiply(wn)) {
                        _weights.Dispose();
                        _weights = wwn.PointwiseDivide(wd);
                    }
                }
            }

            // weights gives cluster membership
            return _weights.AsIndexable().Rows
                .Select((c, i) => Tuple.Create(i, c.MaximumIndex()))
                .GroupBy(d => d.Item2)
                .Select(g => g.Select(d => _data[d.Item1]).ToArray())
                .ToList()
            ;
        }

        public IEnumerable<uint> GetRankedFeatures(int clusterIndex)
        {
            return _features
                .Row(clusterIndex)
                .AsIndexable()
                .Values
                .Select((v, i) => Tuple.Create((uint)i, v))
                .OrderByDescending(d => d.Item2)
                .Select(d => d.Item1)
            ;
        }

        public float[] GetClusterMembership(int documentIndex)
        {
            return _weights
                .Row(documentIndex)
                .AsIndexable()
                .Values
                .ToArray()
            ;
        }

        float _DifferenceCost(IMatrix m1, IMatrix m2)
        {
            return m1.AsIndexable().Rows
                .Zip(m2.AsIndexable().Rows, (r1, r2) => _costFunction.Compute(r1, r2))
                .Average()
            ;
        }
    }
}
