using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class TempBulkHeaderRequestDto
    {
        public Guid TempBulkHeaderId { get; set; }
        public string FileName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public bool IsUploaded { get; set; }
        public bool IsDetailsRemoved { get; set; }
        public bool IsUploadCompleted { get; set; }
        public DateTime? UploadDateTime { get; set; }
        public DateTime? UploadCompletedDateTime { get; set; }
        public Guid? TPAId { get; set; }
        public Guid? CommodityTypeId { get; set; }
        public Guid? ProductId { get; set; }

        public List<TempBulkUploadRequestDto> TempBulkUploads { get; set; }
    }
}
