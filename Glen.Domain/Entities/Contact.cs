
using System.ComponentModel.DataAnnotations;

namespace Glen.Domain.Entities
{
    
    public partial class Contact // : EntityBase<Contact>
    {

        public virtual int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Phone { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Phone2 { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Email { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }


    }
}
