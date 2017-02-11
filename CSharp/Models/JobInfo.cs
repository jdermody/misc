using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models
{
    public class JobInfo
    {
        public int Id { get; set; }
        public JobType Type { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public bool HasCompleted { get; set; }
    }
}
