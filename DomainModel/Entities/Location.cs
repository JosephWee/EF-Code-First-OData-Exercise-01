using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Location
    {
        public Location()
        {
            Fleet = new List<MotorVehicle>();
        }

        public Guid LocationId { get; set; }

        [Index]
        [MaxLength(50)]
        public string Name { get; set; }

        public Guid AddressId { get; set; }
        public virtual Address Address { get; protected set; }

        [Range(0, int.MaxValue)]
        public int ParkingCapacity { get; set; }

        public virtual ICollection<MotorVehicle> Fleet { get; set; }
    }
}
