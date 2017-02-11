using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.TopicDetection
{
    public class TopicDetectionResults
    {
        public string[] Category { get; set; }
        public TopicInfo[] Topic { get; set; }
    }
}
