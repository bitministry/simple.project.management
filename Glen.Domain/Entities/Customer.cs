using System.Collections.Generic;

namespace Glen.Domain.Entities
{
    public partial class Customer : Contact
    {
        public enum CustomerTypeEnum { Company = 0, PrivatePerson = 1 };

        public virtual CustomerTypeEnum CustomerType { get; set; }

        public virtual IList<Installation> Installations { get; set; }

        public virtual IList<Attachment> Attachments { get; set; }

        public virtual IList<Payment> Payments { get; set; }



    }
}
