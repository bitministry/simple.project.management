
using System.ComponentModel.DataAnnotations;
using BitMinistry.Common;

namespace Glen.Domain.Entities
{
    public class Address 
    {
        public virtual int Id { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string formatted_address { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string locality{ get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string postal_town { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public virtual string Line { get; set; }

        public virtual string City()
        {
            return locality ?? postal_town;
        }

    }
}
