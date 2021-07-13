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

            IEnumerable<Ent.VehicleMake> vehicleMakes = await context.VehicleMakes.ExecuteAsync();
            IEnumerable<Ent.MotorVehicleModel> vehicleModels = await context.MotorVehicleModels.ExecuteAsync();

            var autoModels =
                vehicleModels
                .Where(
                    mvm =>
                        mvm.TransmissionType == Ent.TransmissionType.Auto)
                .ToList();

            var makeIDs =
                autoModels
                .Select(mvm => mvm.VehicleMakeId)
                .Distinct().ToList();

            var makes =
                vehicleMakes
                .Where(
                    vm =>
                        makeIDs.Contains(vm.VehicleMakeId)
                )
                .OrderBy(vm => vm.Name);

            foreach (var make in makes)
            {
                var autoModelsOfMake =
                    autoModels
                    .Where(mvm => mvm.VehicleMakeId == make.VehicleMakeId)
                    .OrderBy(mvm => mvm.MotorVehicleType)
                    .ThenBy(mvm => mvm.Name)
                    .ToList();

                for (int i = 0; i < autoModelsOfMake.Count; i++)
                {
                    var vehicleModel = autoModelsOfMake[i];

                    Console.WriteLine(
                        string.Format(
                            "Make: {0}\r\nModel: {1}\r\nTransmission: {2}\r\nType: {3}\r\n",
                            make.Name,
                            vehicleModel.Name,
                            vehicleModel.TransmissionType,
                            vehicleModel.MotorVehicleType
                        )
                    );
                }
            }

            return autoModels;
        }
    }
}
