using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyAdditionalDetailsResponseDto
    {
        public  Guid Id { get; set; }
        public  Guid PolicyId { get; set; }
        public  string NumberOfTyreCover { get; set; }
        public  string FrontWidth { get; set; }
        public  string FrontTyreProfile { get; set; }
        public  string FrontRadius { get; set; }
        public  string FrontSpeedRating { get; set; }
        public  string FrontDot { get; set; }
        public  string RearWidth { get; set; }
        public  string RearTyreProfile { get; set; }
        public  string RearRadius { get; set; }
        public  string RearSpeedRating { get; set; }
        public  string RearDot { get; set; }
        public  string TyreBrand { get; set; }
    }
}
