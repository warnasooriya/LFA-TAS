using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class AttachmentsResponseDto
    {
        public List<AttachmentResponseDto> Attachments { get; set; }

    }
    public class AttachmentResponseDto
    {
        public Guid Id { get; set; }
        public String FileServerRef { get; set; }
        public String FileName { get; set; }
        public String AttachmentSizeKB { get; set; }
        public String AttachmentSection { get; set; }
        public String AttachmentType { get; set; }
        public String DocumentType { get; set; }
        public String ClaimNumber { get; set; }
        public String DateOfAttachment { get; set; }


    }

    public class AttachmentsByUsersResponseDto {
        public List<AttachmentResponseDto> DealerAttachments { get; set; }
        public List<AttachmentResponseDto> AdminAttachments { get; set; }
    }
}
