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
    public class VehicleMakesController : ODataController
    {
        VehicleRentalContext context = new VehicleRentalContext();

        private bool VehicleMakeExists(Guid key)
        {
            return context.VehicleMakes.Any(vm => vm.VehicleMakeId == key);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery(
            AllowedOrderByProperties = "Name",
            MaxTop = int.MaxValue,
            MaxSkip = int.MaxValue)]
        public IQueryable<Ent.VehicleMake> Get()
        {
            return context.VehicleMakes;
        }

        [EnableQuery]
        public SingleResult<Ent.VehicleMake> Get([FromODataUri] Guid key)
        {
            IQueryable<Ent.VehicleMake> result = context.VehicleMakes.Where(vm => vm.VehicleMakeId == key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<Ent.MotorVehicleModel> GetMotorVehicleModels([FromODataUri] Guid key)
        {
            var vehicleMake = context.VehicleMakes.FirstOrDefault(vm => vm.VehicleMakeId == key);
            if (vehicleMake == null)
                return null;
            return vehicleMake.MotorVehicleModels.AsQueryable();
        }

        public async Task<IHttpActionResult> Post(Ent.VehicleMake vehicleMake)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            context.VehicleMakes.Add(vehicleMake);
            await context.SaveChangesAsync();
            return Created(vehicleMake);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Ent.VehicleMake> vehicleMake)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var entity = await context.VehicleMakes.FindAsync(key);
            if (entity == null)
                return NotFound();
            vehicleMake.Patch(entity);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleMakeExists(key))
                    return NotFound();
                else
                    throw;
            }
            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Ent.VehicleMake vehicleMake)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (key != vehicleMake.VehicleMakeId)
                return BadRequest();
            var entry = context.Entry(vehicleMake);
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
                if (!VehicleMakeExists(key))
                    return NotFound();
                else
                    throw;
            }
            return Updated(vehicleMake);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            var vehicleMake = await context.VehicleMakes.FindAsync(key);
            if (vehicleMake == null)
                return NotFound();
            context.VehicleMakes.Remove(vehicleMake);
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}