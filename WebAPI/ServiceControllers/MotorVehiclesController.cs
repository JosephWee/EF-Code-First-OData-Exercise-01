using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using Ent = DomainModel.Entities;
using Biz = DomainModel.BusinessObjects;

namespace WebAPI.ServiceControllers
{
    /// <summary>
    /// For more information please see:
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint
    /// </summary>
    public class MotorVehiclesController : Controller
    {
        VehicleRentalContext context = new VehicleRentalContext();

        private bool MotorVehicleExists(Guid key)
        {
            return context.MotorVehicles.Any(mv => mv.Id == key);
        }

        [HttpPost]
        public async Task<ActionResult> Move(Guid id, Guid toLocation)
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var motorVehicle = await context.MotorVehicles.FindAsync(id);
            if (motorVehicle == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var location = await context.Locations.FindAsync(toLocation);
            if (location == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Unable to find location specified.");

            try
            {
                await Biz.MotorVehicle.MoveAsync(context, motorVehicle, location);
                motorVehicle = await context.MotorVehicles.FindAsync(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MotorVehicleExists(id))
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.ToString());
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}