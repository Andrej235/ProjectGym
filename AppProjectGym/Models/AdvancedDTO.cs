using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Models
{
    public class AdvancedDTO<T>
    {
        public int BatchSize { get; set; }
        public string PreviousBatchURLExtension { get; set; }
        public string NextBatchURLExtension { get; set; }
        public List<T> Values { get; set; } = null!;
    }
}
