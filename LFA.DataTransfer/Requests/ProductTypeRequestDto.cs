using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ProductTypeRequestDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool ProductTypeInsertion
        {
            get;
            set;
        }

    }
}
