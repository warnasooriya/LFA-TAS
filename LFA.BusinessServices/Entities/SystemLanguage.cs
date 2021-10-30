using System;

namespace TAS.Services.Entities
{
    public class SystemLanguage
    {
        public virtual Guid Id { get; set; }
        public virtual string LanguageCode { get; set; }
        public virtual string Language { get; set; }
    }
}
