using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class TimeZoneResponseDto
    {
        public List<TimeZones> TimeZones { get; set; }
    }

    public class TimeZoneTypesResponseDto
    {
        public List<TimeZonesTypesResponseDto> TimeZonesTypes { get; set; }
    }

    public class TimeZones
    {
        public Guid Id { get; set; }
        public float Sequence { get; set; }
        public String NameofTimeZone { get; set; }
        public String Time { get; set; }
    }

    [Serializable]
    public class TimeZoneResponseDtos
    {
        public Guid Id { get; set; }
        public float Sequence { get; set; }
        public String NameofTimeZone { get; set; }
        public String Time { get; set; }
    }
    public class TimeZonesTypesResponseDto
    {
        public Guid Id { get; set; }
        public float Index { get; set; }
        public String NameofTimeZone { get; set; }
        public String Time { get; set; }
    }
}
