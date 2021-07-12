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
    }
}