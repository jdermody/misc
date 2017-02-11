using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public class SensePointer
    {
        public uint Id { get; set; }
        public PointerType Type { get; set; }
        public byte Source { get; set; }
        public byte Target { get; set; }
    }
}
