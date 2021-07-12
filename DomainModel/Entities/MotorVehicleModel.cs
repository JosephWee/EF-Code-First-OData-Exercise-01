using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class MotorVehicleModel
    {
        public MotorVehicleModel()
        {
            MotorVehicles = new List<MotorVehicle>();
        }

        public Guid MotorVehicleModelId { get; set; }

        public Guid VehicleMakeId { get; set; }
        public virtual VehicleMake VehicleMake { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public MotorVehicleType MotorVehicleType { get; set; }
        
        [Required]
        [Range(0D, Double.MaxValue)]
        public Double FuelCapacity { get; set; }

        /// <summary>
        /// Fuel Consumption in Liters per 100 Kilometers
        /// </summary>
        [Required]
        [Range(0D, Double.MaxValue)]
        public Double FuelConsumption { get; set; }

        [Required]
        [Index]
        public EngineType EngineType { get; set; }

        /// <summary>
        /// Max Power in Kilo Watt
        /// </summary>
        [Required]
        [Index]
        [Range(0, int.MaxValue)]
        public int MaxPower { get; set; }

        [Required]
        [Index]
        public TransmissionType TransmissionType { get; set; }

        [Required]
        public bool AWD { get; set; }

        /// <summary>
        /// Cargo Volume in Cubic Centimeters
        /// </summary>
        [Range(0, int.MaxValue)]
        public int CargoVolume { get; set; }

        public virtual ICollection<MotorVehicle> MotorVehicles { get; protected set; }
    }
}
