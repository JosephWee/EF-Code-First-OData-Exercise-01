using DomainModelHelpers;
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ent = DomainModel.Entities;

namespace ConsoleODataClient
{
    /// <summary>
    /// For more information please read:
    /// https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap
    /// https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                RunTasks(args).Wait();
                Console.WriteLine("Press ESC to quit and any other key to repeat.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        static async Task RunTasks(string[] arguments)
        {
            var motorVehicleModels = await ListAutoVehicleModels();
            Console.WriteLine(motorVehicleModels.ToString());
            
            var addedMotorVehicle = await AddMotorVehicleAsync();
            Console.WriteLine(addedMotorVehicle.ToString());

            var batchQueryAsync = await BatchQueryAsync();
            Console.WriteLine(batchQueryAsync.ToString());

            var moveMotorVehicleAsync = await MoveMotorVehicleAsync();
            Console.WriteLine(moveMotorVehicleAsync.ToString());

            return;
        }

        static async Task<StringBuilder> ListAutoVehicleModels()
        {
            DateTime dtStart = DateTime.UtcNow;

            var serviceRoot = "https://localhost:44339/";
            var context = new Default.Container(new Uri(serviceRoot));

            IEnumerable<Ent.MotorVehicleModel> vehicleModels =
                await context
                        .MotorVehicleModels
                        .AddQueryOption("$filter", "TransmissionType eq DomainModel.Entities.TransmissionType'Auto'")
                        .Expand(mvm => mvm.VehicleMake)
                        .ExecuteAsync();

            var autoModels =
                vehicleModels
                .OrderBy(mvm => mvm.VehicleMake.Name)
                .ThenBy(mvm => mvm.MotorVehicleType)
                .ThenBy(mvm => mvm.Name)
                .ToList();

            StringBuilder consoleOutput = new StringBuilder();

            for (int i = 0; i < autoModels.Count; i++)
            {
                var vehicleModel = autoModels[i];

                consoleOutput.AppendLine(
                    string.Format(
                        "Make: {0}\r\nModel: {1}\r\nTransmission: {2}\r\nType: {3}\r\n",
                        vehicleModel.VehicleMake.Name,
                        vehicleModel.Name,
                        vehicleModel.TransmissionType,
                        vehicleModel.MotorVehicleType
                    )
                );
            }

            DateTime dtEnd = DateTime.UtcNow;

            TimeSpan timeElapsed = dtEnd.Subtract(dtStart);

            consoleOutput.AppendLine(
                string.Format(
                    "ListAutoVehicleModels\r\nTime Elapsed: {0}:{1}:{2}.{3}\r\n\r\n",
                    timeElapsed.Hours,
                    timeElapsed.Minutes,
                    timeElapsed.Seconds,
                    timeElapsed.Milliseconds)
            );

            return consoleOutput;
        }

        static async Task<StringBuilder> AddMotorVehicleAsync()
        {
            DateTime dtStart = DateTime.UtcNow;

            var serviceRoot = "https://localhost:44339/";
            var context = new Default.Container(new Uri(serviceRoot));

            IEnumerable<Ent.MotorVehicleModel> motorVehicleModels =
                await context.MotorVehicleModels
                    .AddQueryOption("$top", "1")
                    .AddQueryOption("$skip", "0")
                    .AddQueryOption("$orderby", "Name")
                    .Expand(mvm => mvm.VehicleMake)
                    .ExecuteAsync();

            var vehicleModels = motorVehicleModels.ToList();

            if (!vehicleModels.Any())
                return null;

            var vehicleModel = vehicleModels.First();

            IEnumerable<Ent.MotorVehicle> motorVehicles =
                await context.MotorVehicles
                .ExecuteAsync();

            var existingVehicles = motorVehicles.ToList();

            var existingVINs =
                existingVehicles
                .Select(mv => mv.VIN)
                .Distinct()
                .ToList();

            var existingRegistrations =
                existingVehicles
                .Select(mv => mv.Registration)
                .Distinct()
                .ToList();

            Ent.MotorVehicle newVehicle =
                new Ent.MotorVehicle()
                {
                    Id = Guid.NewGuid(),
                    MotorVehicleModelId = vehicleModel.MotorVehicleModelId,
                    VIN = InfoGenerationHelper.GenerateVIN(existingVINs),
                    Registration = InfoGenerationHelper.GenerateVehicleRegistration(existingRegistrations),
                    Year = DateTime.UtcNow.Year,
                    Mileage = InfoGenerationHelper.GenerateInteger(10000, 100000),
                    HasAutomaticEmergencyBrake = InfoGenerationHelper.GenerateBoolean(),
                    HasBlindspotMonitoring = InfoGenerationHelper.GenerateBoolean(),
                    HasDashCam = InfoGenerationHelper.GenerateBoolean(),
                    HasForwardParkingSensor = InfoGenerationHelper.GenerateBoolean(),
                    HasNavSystem = InfoGenerationHelper.GenerateBoolean(),
                    HasRearParkingSensor = InfoGenerationHelper.GenerateBoolean(),
                    HasReversingCam = InfoGenerationHelper.GenerateBoolean(),
                    LocationId = null
                };

            context.AddToMotorVehicles(newVehicle);
            var response = await context.SaveChangesAsync();

            IEnumerable<Ent.MotorVehicle> motorVehicles1 =
                await context.MotorVehicles
                .AddQueryOption(
                    "$filter",
                    string.Format(
                        "Id eq {0:D} and MotorVehicleModelId eq {1:D}",
                        newVehicle.Id,
                        newVehicle.MotorVehicleModelId)
                )
                .Expand(mv => mv.MotorVehicleModel)
                .ExecuteAsync();

            var matchingVehicles = motorVehicles1.ToList();

            StringBuilder consoleOutput = new StringBuilder();

            if (matchingVehicles.Count == 1)
                newVehicle = matchingVehicles.First();
            else
            {
                consoleOutput.AppendLine("Something went wrong...\r\nSave Unsuccessful");
                return consoleOutput;
            }
            
            consoleOutput.AppendLine(
                string.Format(
                    "Make: {0}\r\nModel: {1}\r\nTransmission: {2}\r\nType: {3}\r\nVIN#: {4}\r\nRegistration#: {5}\r\n",
                    vehicleModel.VehicleMake.Name,
                    vehicleModel.Name,
                    vehicleModel.TransmissionType,
                    vehicleModel.MotorVehicleType,
                    newVehicle.VIN,
                    newVehicle.Registration
                )
            );

            DateTime dtEnd = DateTime.UtcNow;

            TimeSpan timeElapsed = dtEnd.Subtract(dtStart);

            consoleOutput.AppendLine(
                string.Format(
                    "AddMotorVehicleAsync\r\nTime Elapsed: {0}:{1}:{2}.{3}\r\n\r\n",
                    timeElapsed.Hours,
                    timeElapsed.Minutes,
                    timeElapsed.Seconds,
                    timeElapsed.Milliseconds)
            );

            return consoleOutput;
        }

        static async Task<StringBuilder> BatchQueryAsync()
        {
            DateTime dtStart = DateTime.UtcNow;

            var serviceRoot = "https://localhost:44339/";
            var context = new Default.Container(new Uri(serviceRoot));

            var queryVehicleMakes =
                context.VehicleMakes
                .AddQueryOption("$count", "true")
                .AddQueryOption("$orderby", "Name");

            var queryVehicleModels =
                context.MotorVehicleModels
                .AddQueryOption("$count", "true")
                .AddQueryOption("$top", "5")
                .AddQueryOption("$skip", "0")
                .AddQueryOption("$orderby", "Name")
                .Expand(mvm => mvm.VehicleMake);

            DataServiceResponse dataServiceResponse =
                await context.ExecuteBatchAsync(queryVehicleMakes, queryVehicleModels);

            StringBuilder consoleOutput = new StringBuilder();

            if (dataServiceResponse != null)
            {
                if (dataServiceResponse.BatchStatusCode == (int)HttpStatusCode.OK)
                {
                    foreach (var r in dataServiceResponse)
                    {
                        QueryOperationResponse<Ent.VehicleMake> vehicleMakes = r as QueryOperationResponse<Ent.VehicleMake>;
                        if (vehicleMakes != null)
                        {
                            foreach (var vehicleMake in vehicleMakes)
                            {
                                consoleOutput.AppendLine(
                                    string.Format(
                                        "Make: {0}\r\n",
                                        vehicleMake.Name
                                    )
                                );
                            }
                        }

                        QueryOperationResponse<Ent.MotorVehicleModel> motorVehicleModels = r as QueryOperationResponse<Ent.MotorVehicleModel>;
                        if (motorVehicleModels != null)
                        {
                            foreach (var motorVehicleModel in motorVehicleModels)
                            {
                                consoleOutput.AppendLine(
                                    string.Format(
                                        "Make: {0}\r\nModel: {1}\r\nTransmission: {2}\r\n",
                                        motorVehicleModel.VehicleMake.Name,
                                        motorVehicleModel.Name,
                                        motorVehicleModel.TransmissionType
                                    )
                                );
                            }
                        }
                    }
                }
            }
            
            DateTime dtEnd = DateTime.UtcNow;

            TimeSpan timeElapsed = dtEnd.Subtract(dtStart);

            consoleOutput.AppendLine(
                string.Format(
                    "BatchQueryAsync\r\nTime Elapsed: {0}:{1}:{2}.{3}\r\n\r\n",
                    timeElapsed.Hours,
                    timeElapsed.Minutes,
                    timeElapsed.Seconds,
                    timeElapsed.Milliseconds)
            );

            return consoleOutput;
        }

        static async Task<StringBuilder> MoveMotorVehicleAsync()
        {
            DateTime dtStart = DateTime.UtcNow;

            var serviceRoot = "https://localhost:44339/";
            var context = new Default.Container(new Uri(serviceRoot));

            var queryMotorVehicles =
                await context.MotorVehicles
                .AddQueryOption("$count", "true")
                .AddQueryOption("$top", "5")
                .AddQueryOption("$filter", "LocationId ne null")
                .AddQueryOption("$orderby", "LocationId")
                .Expand(mv => mv.Location)
                .ExecuteAsync();

            var motorVehicles = queryMotorVehicles.ToList();

            var excludedLocationIds =
                motorVehicles
                .Where(mv => mv.LocationId.HasValue)
                .Select(mv => string.Format("LocationId ne {0:D}", mv.LocationId.Value))
                .Distinct()
                .ToList();

            var filterArgument =
                string.Format(
                    "({0})",
                    string.Join(" and ", excludedLocationIds)
                );

            var queryLocations =
                await context.Locations
                .AddQueryOption("$count", "true")
                .AddQueryOption("$top", "5")
                .AddQueryOption("$filter", filterArgument)
                .AddQueryOption("$orderby", "Name")
                .Expand(loc => loc.Fleet)
                .ExecuteAsync();

            var locations = queryLocations.ToList();

            var motorVehicle = motorVehicles.FirstOrDefault();
            var toLocation = locations.FirstOrDefault();

            string requestJson = $"{{ \"toLocation\": \"{toLocation.LocationId.ToString("D")}\" }}";
            StringContent stringContent =
                new StringContent(
                    requestJson,
                    Encoding.UTF8,
                    "application/json");

            HttpClient httpClient = new HttpClient();

            string requestUri = "https://localhost:44339/svc/MotorVehicles/Move/" + $"{motorVehicle.Id.ToString("D")}";
            HttpResponseMessage httpResponse =
                await httpClient.PostAsync(
                    requestUri,
                    stringContent
                );

            StringBuilder consoleOutput = new StringBuilder();

            if (httpResponse != null)
            {
                consoleOutput.AppendLine(
                    string.Format(
                        "Request:\r\n{0}\r\nBody: {1}\r\n\r\nResponse:\r\nStatusCode: {2}\r\nResponse Content: {3}\r\n",
                        httpResponse.RequestMessage,
                        requestJson,
                        httpResponse.StatusCode,
                        await httpResponse.Content.ReadAsStringAsync()
                    )
                );
            }

            DateTime dtEnd = DateTime.UtcNow;

            TimeSpan timeElapsed = dtEnd.Subtract(dtStart);

            consoleOutput.AppendLine(
                string.Format(
                    "MoveMotorVehicleAsync\r\nTime Elapsed: {0}:{1}:{2}.{3}\r\n\r\n",
                    timeElapsed.Hours,
                    timeElapsed.Minutes,
                    timeElapsed.Seconds,
                    timeElapsed.Milliseconds)
            );

            return consoleOutput;
        }
    }
}
