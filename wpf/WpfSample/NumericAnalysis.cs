using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample
{
    /// <summary>
    /// Online collection of numerical statistics of a sequence of numbers (std devation, mean, median etc)
    /// This was taken from my open source machine learning library: https://github.com/jdermody/brightwire
    /// </summary>
    class NumericAnalysis
    {
        readonly int _maxDistinct;
        readonly Dictionary<double, ulong> _distinct = new Dictionary<double, ulong>();
        double _mean = 0, _m2 = 0, _min = double.MaxValue, _max = double.MinValue, _mode = 0, _l1 = 0, _l2 = 0;
        ulong _total = 0, _highestCount = 0;

        public NumericAnalysis(int maxDistinct = 131072 * 4)
        {
            _maxDistinct = maxDistinct;
        }

        public void Add(double val)
        {
            ++_total;

            // online std deviation and mean 
            // https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Online_algorithm
            var delta = val - _mean;
            _mean += delta / _total;
            _m2 += delta * (val - _mean);

            // find the min and the max
            if (val < _min)
                _min = val;
            if (val > _max)
                _max = val;

            // add to distinct values
            if (_distinct.Count < _maxDistinct) {
                ulong count = 0;
                if (_distinct.TryGetValue(val, out ulong temp))
                    _distinct[val] = count = temp + 1;
                else
                    _distinct.Add(val, count = 1);
                if (count > _highestCount) {
                    _highestCount = count;
                    _mode = val;
                }
            }

            // calculate norms
            _l1 += Math.Abs(val);
            _l2 += val * val;
        }

        /// <summary>
        /// Calculated L1 Norm from the sequence of numbers
        /// </summary>
        public double L1Norm => _l1;

        /// <summary>
        /// Calculated L2 Norm from the sequence of numbers
        /// </summary>
	    public double L2Norm => Math.Sqrt(_l2);

        /// <summary>
        /// Minimum number found
        /// </summary>
        public double Min => _min;

        /// <summary>
        /// Maximum number found
        /// </summary>
	    public double Max => _max;

        /// <summary>
        /// Mean of the sequence of numbers
        /// </summary>
	    public double Mean => _mean;

        /// <summary>
        /// Calulcated variance from the sequence of numbers
        /// </summary>
	    public double? Variance => _total > 1 ? (_m2 / (_total - 1)) : (double?)null;

        /// <summary>
        /// Calculated standard deviation
        /// </summary>
        public double? StdDev {
            get
            {
                var variance = Variance;
                if (variance.HasValue)
                    return Math.Sqrt(variance.Value);
                return null;
            }
        }

        /// <summary>
        /// Median value in the sequence
        /// </summary>
        public double? Median
        {
            get
            {
                double? ret = null;
                if (_distinct.Count < _maxDistinct && _distinct.Any()) {
                    ulong middle = _total / 2, count = 0;
                    foreach (var item in _distinct.OrderBy(kv => kv.Key)) {
                        top:
                        if (count + item.Value >= middle) {
                            if (ret.HasValue) {
                                ret = (ret.Value + item.Key) / 2;
                                break;
                            }
                            else {
                                ret = item.Key;
                                if (_total % 2 == 0)
                                    break;
                                middle = middle + 1;
                                goto top;
                            }
                        }
                        count += item.Value;
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Mode of the sequence
        /// </summary>
        public double? Mode
        {
            get
            {
                if (_distinct.Count < _maxDistinct && _distinct.Any())
                    return _mode;
                return null;
            }
        }

        /// <summary>
        /// Number
        /// </summary>
        public int? NumDistinct => _distinct.Count < _maxDistinct ? _distinct.Count : (int?)null;
	    public IEnumerable<object> DistinctValues => _distinct.Count < _maxDistinct ? _distinct.Select(kv => (object)kv.Key) : null;
    }
}
