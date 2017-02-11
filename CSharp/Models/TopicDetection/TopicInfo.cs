using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.TopicDetection
{
    public class TopicInfo
    {
        public uint TopicId { get; set; }
        public string Title { get; set; }
        public uint[] SenseIndex { get; set; }
    }
}
