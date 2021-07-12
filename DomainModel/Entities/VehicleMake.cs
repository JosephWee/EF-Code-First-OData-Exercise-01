using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class VehicleMake
    {
        public VehicleMake()
        {
            MotorVehicleModels = new List<MotorVehicleModel>();
        }

        public Guid VehicleMakeId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<MotorVehicleModel> MotorVehicleModels { get; protected set; }
    }
}
