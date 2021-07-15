using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using ODat = Microsoft.AspNet.OData;
using Ent = DomainModel.Entities;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "ODataRoute",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.OrderBy().Count().MaxTop(null).SkipToken().Filter().Expand();
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Ent.VehicleMake>("VehicleMakes");
            builder.EntitySet<Ent.MotorVehicleModel>("MotorVehicleModels");
            builder.EntitySet<Ent.MotorVehicle>("MotorVehicles");
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel());
        }
    }
}
