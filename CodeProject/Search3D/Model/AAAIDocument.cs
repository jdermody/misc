using BrightWire.Helper;
using BrightWire.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search3D.Models
{
    public class AAAIDocument
    {
        /// <summary>
        /// Free text description of the document
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Free text; author-generated keywords
        /// </summary>
        public string[] Keyword { get; set; }

        /// <summary>
        /// Free text; author-selected, low-level keywords
        /// </summary>
        public string[] Topic { get; set; }

        /// <summary>
        /// Free text; paper abstracts
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Categorical; author-selected, high-level keyword(s)
        /// </summary>
        public string[] Group { get; set; }

        /// <summary>
        /// The degree to which this document belongs in clusters
        /// </summary>
        public float[] ClusterMembership { get; set; }

        public SparseVectorClassification AsClassification(StringTableBuilder stringTable)
        {
            var weightedIndex = new List<WeightedIndex>();
            foreach (var item in Keyword) {
                weightedIndex.Add(new WeightedIndex {
                    Index = stringTable.GetIndex(item),
                    Weight = 1f
                });
            }
            foreach (var item in Topic) {
                weightedIndex.Add(new WeightedIndex {
                    Index = stringTable.GetIndex(item),
                    Weight = 1f
                });
            }
            foreach (var item in Group) {
                weightedIndex.Add(new WeightedIndex {
                    Index = stringTable.GetIndex(item),
                    Weight = 1f
                });
            }
            return new SparseVectorClassification {
                Name = Title,
                Data = weightedIndex
                    .GroupBy(d => d.Index)
                    .Select(g => new WeightedIndex {
                        Index = g.Key,
                        Weight = g.Sum(d => d.Weight)
                    })
                    .ToArray()
            };
        }
    }
}
