using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Country
    {
        public Country()
        {
            States = new List<State>();
        }

        public Guid CountryId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<State> States { get; protected set; }
    }
}
