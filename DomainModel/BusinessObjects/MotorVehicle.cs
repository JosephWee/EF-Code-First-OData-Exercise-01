using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ent = DomainModel.Entities;

namespace DomainModel.BusinessObjects
{
    public static class MotorVehicle
    {
        public static async Task MoveAsync(VehicleRentalContext context, Ent.MotorVehicle motorVehicle, Ent.Location toLocation)
        {
            if (motorVehicle == null)
                throw new ArgumentNullException("motorVehicle cannot be null.");

            if (toLocation == null)
                throw new ArgumentNullException("toLocation cannot be null.");

            var mv = context.Entry(motorVehicle);
            var loc = context.Entry(toLocation);

            if (mv == null)
                throw new ArgumentException("Cannot find motor vehicle specified.");
            else if (mv.State != EntityState.Unchanged)
                throw new ArgumentException("Motor vehicle must be in unchanged state.");

            if (loc == null)
                throw new ArgumentException("Cannot find location specified.");
            else if (loc.State != EntityState.Unchanged)
                throw new ArgumentException("Location must be in unchanged state.");

            if (toLocation.Fleet.Count >= toLocation.ParkingCapacity)
                throw new ArgumentException("Location is at capacity.");

            if (!motorVehicle.LocationId.HasValue)
                throw new InvalidOperationException("Vehicle is not in a location right now, use drop off operation instead.");

            if (motorVehicle.LocationId == toLocation.LocationId)
                throw new InvalidOperationException("Vehicle is already in the location.");

            motorVehicle.LocationId = toLocation.LocationId;

            //Allow concurrency exceptions to bubble up
            await context.SaveChangesAsync();
        }
    }
}
