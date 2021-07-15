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
            ListAutoVehicleModels().Wait();

            Console.ReadKey();
        }

        static async Task<List<Ent.MotorVehicleModel>> ListAutoVehicleModels()
        {
            var serviceRoot = "https://localhost:44339/";
            var context = new Default.Container(new Uri(serviceRoot));

            IEnumerable<Ent.MotorVehicleModel> vehicleModels = await context.MotorVehicleModels.Expand(mvm => mvm.VehicleMake).ExecuteAsync();

            var autoModels =
                vehicleModels
                .Where(
                    mvm =>
                        mvm.TransmissionType == Ent.TransmissionType.Auto)
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
