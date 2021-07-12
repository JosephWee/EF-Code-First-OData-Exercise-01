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
    }
}