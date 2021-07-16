using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using ODat = Microsoft.AspNet.OData;
using Ent = DomainModel.Entities;
using Microsoft.AspNet.OData.Batch;

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

            //Read the following for more information regarding $batch
            //https://devblogs.microsoft.com/odata/all-in-one-with-odata-batch/
            //https://docs.microsoft.com/en-us/odata/webapi/batch
            //https://docs.microsoft.com/en-us/odata/client/batch-operations
            //https://docs.oasis-open.org/odata/odata-json-format/v4.01/os/odata-json-format-v4.01-os.html#sec_BatchRequestsandResponses
            var odataBatchHandler = new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer);
            odataBatchHandler.MessageQuotas.MaxNestingDepth = 2;
            odataBatchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;
            odataBatchHandler.MessageQuotas.MaxReceivedMessageSize = 100;

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel(),
                batchHandler: odataBatchHandler);
        }
    }
}
