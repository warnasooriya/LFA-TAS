using System;

namespace TAS.Services.Entities
{
    public class ArticleMapping
    {
        public virtual Guid Id { get; set; }
        public virtual string ArticleNo { get; set; }
        public virtual Guid AvailableTireSizeId { get; set; }
    }
}
