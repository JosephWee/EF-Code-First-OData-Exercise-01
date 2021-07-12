using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }

        [Required]
        [MaxLength(50)]
        public string AddressLine1 { get; set; }

        [MaxLength(50)]
        public string AddressLine2 { get; set; }

        [NotMapped]
        public Country Country
        {
            get
            {
                if (State == null)
                    return null;

                return State.Country;
            }
        }

        [NotMapped]
        public State State
        {
            get
            {
                if (City == null)
                    return null;

                return City.State;
            }
        }

        [NotMapped]
        public City City
        {
            get
            {
                if (Suburb == null)
                    return null;

                return Suburb.City;
            }
        }

        public Guid SuburbId { get; set; }
        public virtual Suburb Suburb { get; protected set; }

        [Required]
        public string PostalCode { get; set; }
    }
}
