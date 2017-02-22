using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNMFSearchResultClustering.Models
{
    class DocumentCluster
    {
        readonly IReadOnlyList<AAAIDocument> _document;
        readonly string _desc;

        public DocumentCluster(IReadOnlyList<AAAIDocument> document, string desc)
        {
            _document = document;
            _desc = desc;
        }

        public IReadOnlyList<AAAIDocument> Document { get { return _document; } }
        public string Description { get { return _desc; } }
    }
}
