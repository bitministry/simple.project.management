using System;
using System.Collections.Generic;

namespace Glen.Domain.Entities
{
    

    public partial class Installation 
    {
        public enum StatusEnum { Inquiry, Proposal, Order, InStock, Installed, Archived };

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }


        public virtual Customer Customer { get; set; }

        public virtual DateTime? InquiryDate { get; set; }
        public virtual DateTime? ProposalDate { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? Start { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? InStockDate { get; set; }
        public virtual DateTime? InstalledDate { get; set; }
        public virtual DateTime? ArchivedDate { get; set; }

        public virtual decimal? Price { get; set; }
        


        public virtual StatusEnum Status { get; set; } = StatusEnum.Inquiry;
        

    
    }


}
