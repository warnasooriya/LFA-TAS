using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxRequestDto
    {
        public virtual Guid Id { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual bool IsConformed { get; set; }
        public virtual bool IsProcessed { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }

        public bool BordxInsertion
        {
            get;
            set;
        }

    }
}
