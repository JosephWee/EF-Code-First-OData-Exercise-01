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
    public class LocationsController : ODataController
    {
        VehicleRentalContext context = new VehicleRentalContext();

        private bool LocationExists(Guid key)
        {
            return context.Locations.Any(loc => loc.LocationId == key);
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
        public IQueryable<Ent.Location> Get()
        {
            return context.Locations;
        }
    }
}