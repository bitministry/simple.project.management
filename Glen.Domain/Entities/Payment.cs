using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glen.Domain.Entities
{
    public class Payment
    {
        public virtual int Id { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual DateTime XWhen { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
