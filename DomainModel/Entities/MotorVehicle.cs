using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class MotorVehicle
    {
        public Guid Id { get; set; }

        [NotMapped]
        public VehicleMake VehicleMake
        {
            get
            {
                if (MotorVehicleModel == null)
                    return null;

                return MotorVehicleModel.VehicleMake;
            }
        }

        public Guid MotorVehicleModelId { get; set; }
        public virtual MotorVehicleModel MotorVehicleModel { get; protected set; }

        [Required]
        [Index]
        [Range(1900, 2200)]
        public int Year { get; set; }

        [Required]
        [Index]
        [MinLength(17)]
        [MaxLength(17)]
        [RegularExpression(@"[A-Z0-9]{17}")]
        public string VIN { get; set; }

        [Index]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression(@"S[A-Z]{2}[0-9]{4}[A-Z]{1}")]
        public string Registration { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [Required]
        public bool HasNavSystem { get; set; }

        [Required]
        public bool HasDashCam { get; set; }

        [Required]
        public bool HasReversingCam { get; set; }

        [Required]
        public bool HasForwardParkingSensor { get; set; }

        [Required]
        public bool HasRearParkingSensor { get; set; }

        [Required]
        public bool HasBlindspotMonitoring { get; set; }

        [Required]
        public bool HasAutomaticEmergencyBrake { get; set; }

        [Column("PickupCargoAccessoryType")]
        public PickupCargoAccessoryType _PickupCargoAccessoryType
        {
            get;
            protected set;
        }

        [NotMapped]
        public PickupCargoAccessoryType PickupCargoAccessoryType
        {
            get
            {
                if (MotorVehicleModel != null && MotorVehicleModel.MotorVehicleType != MotorVehicleType.Pickup)
                {
                    _PickupCargoAccessoryType = PickupCargoAccessoryType.None;
                }

                return _PickupCargoAccessoryType;
            }
            set
            {
                if (MotorVehicleModel == null || MotorVehicleModel.MotorVehicleType == MotorVehicleType.Pickup)
                {
                    _PickupCargoAccessoryType = value;
                }
                else
                {
                    _PickupCargoAccessoryType = PickupCargoAccessoryType.None;
                }
            }
        }

        public Guid? LocationId { get; set; }
        public virtual Location Location { get; protected set; }
    }
}
