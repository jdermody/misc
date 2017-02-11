using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public class ExpandedSenseIndex : SenseIndex
    {
        public List<ExpandedSensePointer> ExpandedPointer { get; set; }
    }
}
