using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TempBulkHeader
    {
        public virtual Guid TempBulkHeaderId { get; set; }
        public virtual string FileName { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual Guid? UserId { get; set; }
        public virtual DateTime? EntryDateTime { get; set; }
        public virtual bool IsUploaded { get; set; }
        public virtual bool IsDetailsRemoved { get; set; }
        public virtual bool IsUploadCompleted { get; set; }
        public virtual DateTime? UploadDateTime { get; set; }
        public virtual DateTime? UploadCompletedDateTime { get; set; }
        public virtual Guid? TPAId { get; set; }
        public virtual Guid? CommodityTypeId { get; set; }
        public virtual Guid? ProductId { get; set; }

        public virtual List<TempBulkUpload> TempBulkUploads { get; set; }
    }
}
