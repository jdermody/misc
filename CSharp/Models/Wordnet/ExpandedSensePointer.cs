using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public class ExpandedSensePointer : SenseIndex
    {
        public PointerType Type { get; set; }
        public byte Source { get; set; }
        public byte Target { get; set; }
    }
}
