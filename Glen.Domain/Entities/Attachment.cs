using System;

namespace Glen.Domain.Entities
{
    public class Attachment
    {
        public virtual int Id { get; set; }

        public virtual string FileName { get; set; }
        public virtual long Size { get; set; }

        public virtual DateTime? Created { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
