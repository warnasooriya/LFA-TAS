using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxAlignmentDto
    {
        public int Index { get; set; }
        public string Alignment { get; set; }
        public bool IsActive { get; set; }
        public string keyName { get; set; }
        public int Sequance { get; set; }
        public double ColumnWidth { get; set; }
    }
}
