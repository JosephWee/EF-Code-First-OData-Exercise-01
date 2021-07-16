using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ent = DomainModel.Entities;

namespace ConsoleODataClient
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dtStart = DateTime.UtcNow;

            ListAutoVehicleModels().Wait();

            DateTime dtEnd = DateTime.UtcNow;

            TimeSpan timeElapsed = dtEnd.Subtract(dtStart);

            Console.WriteLine(
                "Time Elapsed: {0}:{1}:{2}.{3}",
                timeElapsed.Hours,
                timeElapsed.Minutes,
                timeElapsed.Seconds,
                timeElapsed.Milliseconds
            );

            Console.ReadKey();
        }

        static async Task<List<Ent.MotorVehicleModel>> ListAutoVehicleModels()
        {
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

            for (int i = 0; i < autoModels.Count; i++)
            {
                var vehicleModel = autoModels[i];

                Console.WriteLine(
                    string.Format(
                        "Make: {0}\r\nModel: {1}\r\nTransmission: {2}\r\nType: {3}\r\n",
                        vehicleModel.VehicleMake.Name,
                        vehicleModel.Name,
                        vehicleModel.TransmissionType,
                        vehicleModel.MotorVehicleType
                    )
                );
            }

            return autoModels;
        }
    }
}
