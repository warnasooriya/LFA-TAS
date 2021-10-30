using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace TAS.DataTransfer.Responses
{
    public class CustomersResponseDto
    {

        public List<CustomerResponseDto> Customers
        {
            get; 
            set; 
        }

    }

    public class OccupationsResponseDto
    {

        public List<OccupationResponseDto> Occupations
        {
            get;
            set;
        }

    }
    public class TitlesResponseDto
    {

        public List<TitleResponseDto> Titles
        {
            get;
            set;
        }

    }
    public class MarritalStatusesResponseDto
    {

        public List<MarritalStatusResponseDto> MarritalStatuses
        {
            get;
            set;
        }

    }
}
