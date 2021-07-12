using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class City
    {
        public City()
        {
            Suburbs = new List<Suburb>();
        }

        public Guid CityId { get; set; }

        public Guid StateId { get; set; }
        public virtual State State { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Suburb> Suburbs { get; protected set; }
    }
}
