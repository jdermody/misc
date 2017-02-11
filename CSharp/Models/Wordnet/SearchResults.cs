using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public class SearchResults
    {
        public string[] Suggestion { get; set; }
        public SenseIndex[] Sense { get; set; }
    }
}
