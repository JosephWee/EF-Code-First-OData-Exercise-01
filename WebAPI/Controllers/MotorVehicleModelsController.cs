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
    public class MotorVehicleModelsController : ODataController
    {
        VehicleRentalContext context = new VehicleRentalContext();

        private bool MotorVehicleModelExists(Guid key)
        {
            return context.MotorVehicleModels.Any(mvm => mvm.MotorVehicleModelId == key);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery(
            AllowedOrderByProperties = "MotorVehicleType,Name",
            MaxTop = int.MaxValue,
            MaxSkip = int.MaxValue)]
        public IQueryable<Ent.MotorVehicleModel> Get()
        {
            return context.MotorVehicleModels;
        }

        [EnableQuery]
        public SingleResult<Ent.MotorVehicleModel> Get([FromODataUri] Guid key)
        {
            IQueryable<Ent.MotorVehicleModel> result = context.MotorVehicleModels.Where(mvm => mvm.MotorVehicleModelId == key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<Ent.MotorVehicle> GetMotorVehicles([FromODataUri] Guid key)
        {
            var vehicleModel = context.MotorVehicleModels.FirstOrDefault(mvm => mvm.MotorVehicleModelId == key);
            if (vehicleModel == null)
                return null;
            return vehicleModel.MotorVehicles.AsQueryable();
        }

        public async Task<IHttpActionResult> Post(Ent.MotorVehicleModel motorVehicleModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            context.MotorVehicleModels.Add(motorVehicleModel);
            await context.SaveChangesAsync();
            return Created(motorVehicleModel);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Ent.MotorVehicleModel> motorVehicleModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var entity = await context.MotorVehicleModels.FindAsync(key);
            if (entity == null)
                return NotFound();
            motorVehicleModel.Patch(entity);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotorVehicleModelExists(key))
                    return NotFound();
                else
                    throw;
            }
            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Ent.MotorVehicleModel motorVehicleModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (key != motorVehicleModel.VehicleMakeId)
                return BadRequest();
            var entry = context.Entry(motorVehicleModel);
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
                if (!MotorVehicleModelExists(key))
                    return NotFound();
                else
                    throw;
            }
            return Updated(motorVehicleModel);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            var motorVehicleModel = await context.MotorVehicleModels.FindAsync(key);
            if (motorVehicleModel == null)
                return NotFound();
            context.MotorVehicleModels.Remove(motorVehicleModel);
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}