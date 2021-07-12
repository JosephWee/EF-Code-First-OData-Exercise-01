using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Suburb
    {
        public Suburb()
        {
            Addresses = new List<Address>();
        }

        public Guid SuburbId { get; set; }

        public Guid CityId { get; set; }
        public virtual City City { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; protected set; }
    }
}
