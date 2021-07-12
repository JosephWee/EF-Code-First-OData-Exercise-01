using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public enum MotorVehicleType
    {
        Scooter = 0,
        Motorcycle = 1,
        HatchBack = 2,
        Sedan = 3,
        Wagon = 4,
        CompactSUV = 5,
        SUV = 6,
        MiniBus = 7,
        Van = 8,
        Pickup = 9
    }

    public enum EngineType
    {
        Diesel = 0,
        Petrol = 1,
        Hybrid_Electric_Diesel = 2,
        Hybrid_Electric_Petrol = 3,
        Electric = 4
    }

    public enum TransmissionType
    {
        Auto = 0,
        Manual = 1
    }

    public enum PickupCargoAccessoryType
    {
        None = 0,
        TonneauCover = 1,
        TruckCap = 2
    }
}
