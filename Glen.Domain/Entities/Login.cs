using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glen.Domain.Entities
{
    public class Login
    {
        public virtual int Id { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual bool KeepMeLoggedIn { get; set; }
    }

}
