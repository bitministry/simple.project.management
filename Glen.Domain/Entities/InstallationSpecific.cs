using System;
using System.Collections.Generic;

namespace Glen.Domain.Entities
{

    public partial class Installation 
    {

        public enum BlindTypeEnum { RL, Vert, WD, FW, Alu25 };
        public enum SideEnum { Left, Right };


        public virtual BlindTypeEnum? BlindType { get; set; } = BlindTypeEnum.RL;

        public virtual string Width { get; set; }
        public virtual string Height { get; set; }

        public virtual string Size()
        {
            if (Width != null && Height != null)
                return $"{Width} * {Height}";
            
            return $"{Width}{Height}";
        }

        public virtual string MeasureInstructions { get; set; }
        public virtual string SpecialInstructions { get; set; }
        public virtual string Material { get; set; }
        public virtual SideEnum? Side { get; set; }

        public virtual void NextStatus()
        {
            switch (Status)
            {
                case StatusEnum.Order:
                    InStockDate = DateTime.Now;;
                    break;

                case StatusEnum.InStock:
                    InstalledDate = DateTime.Now;
                    break;

                case StatusEnum.Installed:
                    ArchivedDate = DateTime.Now;
                    break;
            }
        }

        public virtual void CheckStatus()
        {
            if (ArchivedDate != null)
            {
                Status = StatusEnum.Archived;
                return;
            }

            if (InstalledDate == null && Status > StatusEnum.InStock)
                Status = StatusEnum.InStock;

            if (InStockDate == null && Status > StatusEnum.Order)
                Status = StatusEnum.Order;

            if ((OrderDate == null || Start == null || Deadline == null) && Status > StatusEnum.Proposal)
            {
                OrderDate = null;
                Start = null;
                Deadline = null;
                Status = StatusEnum.Proposal;
            }
            if ((Price == null || ProposalDate == null) && Status > StatusEnum.Proposal)
            {
                ProposalDate = null;
                Status = StatusEnum.Inquiry;
            }

            switch (Status)
            {
                case StatusEnum.Inquiry:
                    if (Price > 0 && ProposalDate == null)
                    {
                        ProposalDate = DateTime.Now;
                        Status = StatusEnum.Proposal;
                    }
                    break;
                case StatusEnum.Proposal:

                    if (Start != null && Deadline != null)
                    {
                        OrderDate = DateTime.Now;
                        Status = StatusEnum.Order;
                    }
                    break;

                case StatusEnum.Order:

                    if (InStockDate != null)
                        Status = StatusEnum.InStock;
                    break;

                case StatusEnum.InStock:

                    if (InstalledDate != null)
                        Status = StatusEnum.Installed;
                    break;

            }

        }



    }


}
