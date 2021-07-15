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
    }
}