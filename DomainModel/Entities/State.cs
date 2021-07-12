using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class State
    {
        public State()
        {
            Cities = new List<City>();
        }

        public Guid StateId { get; set; }

        public Guid CountryId { get; set; }
        public virtual Country Country { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; protected set; }
    }
}
