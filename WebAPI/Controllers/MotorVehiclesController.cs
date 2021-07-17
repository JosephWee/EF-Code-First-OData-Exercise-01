using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.OData;
using ODat = Microsoft.AspNet.OData;
using DomainModel;
using Ent = DomainModel.Entities;

namespace WebAPI.Controllers
{
    /// <summary>
    /// For more information please see:
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint
    /// </summary>
    public class MotorVehiclesController : ODataController
    {
        VehicleRentalContext context = new VehicleRentalContext();

        private bool MotorVehicleExists(Guid key)
        {
            return context.MotorVehicles.Any(mv => mv.Id == key);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery]
        public IQueryable<Ent.MotorVehicle> Get()
        {
            return context.MotorVehicles;
        }

        [EnableQuery]
        public SingleResult<Ent.MotorVehicle> Get([FromODataUri] Guid key)
        {
            IQueryable<Ent.MotorVehicle> result = context.MotorVehicles.Where(mv => mv.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Ent.MotorVehicle motorVehicle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            context.MotorVehicles.Add(motorVehicle);
            await context.SaveChangesAsync();
            return Created(motorVehicle);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Ent.MotorVehicle> motorVehicle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var entity = await context.MotorVehicles.FindAsync(key);
            if (entity == null)
                return NotFound();
            motorVehicle.Patch(entity);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotorVehicleExists(key))
                    return NotFound();
                else
                    throw;
            }
            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Ent.MotorVehicle motorVehicle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (key != motorVehicle.Id)
                return BadRequest();
            var entry = context.Entry(motorVehicle);
            if (entry == null)
                return BadRequest();
            else
                entry.State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotorVehicleExists(key))
                    return NotFound();
                else
                    throw;
            }
            return Updated(motorVehicle);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            var motorVehicle = await context.MotorVehicles.FindAsync(key);
            if (motorVehicle == null)
                return NotFound();
            context.MotorVehicles.Remove(motorVehicle);
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}