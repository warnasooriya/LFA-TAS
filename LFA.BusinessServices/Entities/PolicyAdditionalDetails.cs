using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PolicyAdditionalDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual string NumberOfTyreCover { get; set; }
        public virtual string FrontWidth { get; set; }
        public virtual string FrontTyreProfile { get; set; }
        public virtual string FrontRadius { get; set; }
        public virtual string FrontSpeedRating { get; set; }
        public virtual string FrontDot { get; set; }
        public virtual string RearWidth { get; set; }
        public virtual string RearTyreProfile { get; set; }
        public virtual string RearRadius { get; set; }
        public virtual string RearSpeedRating { get; set; }
        public virtual string RearDot { get; set; }
        public virtual string TyreBrand { get; set; }

    }
}
