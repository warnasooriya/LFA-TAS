using System;

namespace TAS.DataTransfer.Responses
{
    public class SystemLanguageResponseDto
    {
        public virtual Guid Id { get; set; }
        public virtual string LanguageCode { get; set; }
        public virtual string Language { get; set; }
    }
}
