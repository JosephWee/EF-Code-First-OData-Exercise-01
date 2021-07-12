using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ent = DomainModel.Entities;
using DomainModel.Extensions;

namespace DomainModelTest
{
    public class VehicleRentalContextHelper
    {
        public static void EmptyDatabase(DomainModel.VehicleRentalContext context)
        {
            //We need to clear Tables for Entities that are
            //randomly generated:
            //MotorVehicles, MotorVehicleModels, VehicleMakes,
            //Locations, Addresses, Suburbs, Cities, States,
            //Countries...

            context.MotorVehicles.RemoveRange(
                context.MotorVehicles.ToList()
            );

            context.MotorVehicleModels.RemoveRange(
                context.MotorVehicleModels.ToList()
            );

            context.VehicleMakes.RemoveRange(
                context.VehicleMakes.ToList()
            );

            context.Locations.RemoveRange(
                context.Locations.ToList()
            );

            context.Addresses.RemoveRange(
                context.Addresses.ToList()
            );

            context.Suburbs.RemoveRange(
                context.Suburbs.ToList()
            );

            context.Cities.RemoveRange(
                context.Cities.ToList()
            );

            context.States.RemoveRange(
                context.States.ToList()
            );

            context.Countries.RemoveRange(
                context.Countries.ToList()
            );

            context.SaveChanges();
        }

        public static void PopulateDatabase(DomainModel.VehicleRentalContext context, DomainModel.VehicleRentalContext contextQuery)
        {
            List<Ent.VehicleMake> vehicleMakes = new List<Ent.VehicleMake>();
            List<Ent.MotorVehicleModel> vehicleModels = new List<Ent.MotorVehicleModel>();

            //Use Fictional Make
            var make_StarAuto = new Ent.VehicleMake() { VehicleMakeId = new Guid("8A103C71-1675-46F8-BDE8-4637CAF656F1"), Name = "StarAuto" };
            var make_GlobalVehicle = new Ent.VehicleMake() { VehicleMakeId = new Guid("794F01F1-0D0F-4BBC-8649-9DC8AF7E0C23"), Name = "GlobalVehicle" };

            vehicleMakes.Add(make_StarAuto);
            vehicleMakes.Add(make_GlobalVehicle);

            //Use Fictional Models
            var model_StarAuto_Light_Hatch =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("1D4F8989-FBF0-4738-A9E3-9F45D27A38AC"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Light-HA",
                    MotorVehicleType = Ent.MotorVehicleType.HatchBack,
                    FuelCapacity = 1.4,
                    FuelConsumption = 5.8,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 80,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_StarAuto_Light_Hatch);

#if DEBUG1
            var m1 = queryContext.MotorVehicleModels.FirstOrDefault(x => x.VehicleMake.VehicleMakeId == make_StarAuto.VehicleMakeId);
            var m2 = queryContext.MotorVehicleModels.FirstOrDefault(x => x.VehicleMake.VehicleMakeId == make_GlobalVehicle.VehicleMakeId);

            var m1m1 = m1 == null ? null : m1.VehicleMake;
            var m2m1 = m2 == null ? null : m2.VehicleMake;

            if (m1 != null)
            {
                if (m1.VehicleMakeId == make_GlobalVehicle.VehicleMakeId)
                    model_StarAuto_Light_Hatch.VehicleMakeId = make_StarAuto.VehicleMakeId;
                else
                    model_StarAuto_Light_Hatch.VehicleMakeId = make_GlobalVehicle.VehicleMakeId;
            }

            if (m2 != null)
            {
                if (m2.VehicleMakeId == make_GlobalVehicle.VehicleMakeId)
                    model_StarAuto_Light_Hatch.VehicleMakeId = make_StarAuto.VehicleMakeId;
                else
                    model_StarAuto_Light_Hatch.VehicleMakeId = make_GlobalVehicle.VehicleMakeId;
            }

            var m1m2 = m1 == null ? null : m1.VehicleMake;
            var m2m2 = m2 == null ? null : m2.VehicleMake;

            string pause = null;
#endif

            var model_StarAuto_Light_Sedan =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("4C326F79-9AE3-4F0B-B9A9-00F75BD60CE9"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Light-SA",
                    MotorVehicleType = Ent.MotorVehicleType.Sedan,
                    FuelCapacity = 1.4,
                    FuelConsumption = 5.8,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 80,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_StarAuto_Light_Sedan);

            var model_StarAuto_Mid_Hatch =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("6C0472D4-BC69-48BE-8212-47175884D0FC"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Midnight-HA",
                    MotorVehicleType = Ent.MotorVehicleType.HatchBack,
                    FuelCapacity = 2.5,
                    FuelConsumption = 6.5,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 154,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_StarAuto_Mid_Hatch);

            var model_StarAuto_Mid_Sedan =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("E04B8706-640A-4206-B4E1-2F06BBE70C84"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Midnight-SA",
                    MotorVehicleType = Ent.MotorVehicleType.Sedan,
                    FuelCapacity = 2.5,
                    FuelConsumption = 6.5,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 154,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_StarAuto_Mid_Sedan);

            var model_StarAuto_Mid_Hybrid =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("19AA1F11-B4C7-4A3D-88A6-B428449DC6B2"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Midnight-HH",
                    MotorVehicleType = Ent.MotorVehicleType.HatchBack,
                    FuelCapacity = 1.7,
                    FuelConsumption = 3.9,
                    EngineType = Ent.EngineType.Hybrid_Electric_Petrol,
                    MaxPower = 100,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_StarAuto_Mid_Hybrid);

            var model_StarAuto_CompactSUV =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("C5548210-DBA9-436D-9CB2-AC9D47C38BAB"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Comet-A",
                    MotorVehicleType = Ent.MotorVehicleType.CompactSUV,
                    FuelCapacity = 2.5,
                    FuelConsumption = 7.0,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 160,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = true
                };
            vehicleModels.Add(model_StarAuto_CompactSUV);

            var model_StarAuto_SUV =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("E1D74FAF-8DD8-42AB-94AD-6E488C2A7A47"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Storm-A",
                    MotorVehicleType = Ent.MotorVehicleType.SUV,
                    FuelCapacity = 2.0,
                    FuelConsumption = 6.0,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 127,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = true
                };
            vehicleModels.Add(model_StarAuto_SUV);

            var model_StarAuto_MiniBus =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("E54E62A1-E032-471D-96DC-A7B0CE9C9341"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Forrest-A",
                    MotorVehicleType = Ent.MotorVehicleType.MiniBus,
                    FuelCapacity = 2.5,
                    FuelConsumption = 8.5,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 135,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = true
                };
            vehicleModels.Add(model_StarAuto_MiniBus);

            var model_StarAuto_VanManual =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("BA484D5A-3120-4E9E-B2BF-8DDDD0459298"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Cargo-VM",
                    MotorVehicleType = Ent.MotorVehicleType.Van,
                    FuelCapacity = 2.5,
                    FuelConsumption = 5.0,
                    EngineType = Ent.EngineType.Diesel,
                    MaxPower = 106,
                    TransmissionType = Ent.TransmissionType.Manual,
                    AWD = false,
                    CargoVolume = 2982
                };
            vehicleModels.Add(model_StarAuto_VanManual);

            var model_StarAuto_VanAuto =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("D9236900-1DA7-48DD-9A66-66958A4BA977"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Cargo-VA",
                    MotorVehicleType = Ent.MotorVehicleType.Van,
                    FuelCapacity = 2.5,
                    FuelConsumption = 5.0,
                    EngineType = Ent.EngineType.Diesel,
                    MaxPower = 106,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false,
                    CargoVolume = 2982
                };
            vehicleModels.Add(model_StarAuto_VanAuto);

            var model_StarAuto_Pickup =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("58901907-16D9-49A1-8BA4-C4901FC8433D"),
                    VehicleMakeId = make_StarAuto.VehicleMakeId,
                    Name = "Taurus-PM",
                    MotorVehicleType = Ent.MotorVehicleType.Pickup,
                    FuelCapacity = 2.5,
                    FuelConsumption = 8.0,
                    EngineType = Ent.EngineType.Diesel,
                    MaxPower = 110,
                    TransmissionType = Ent.TransmissionType.Manual,
                    AWD = false,
                    CargoVolume = 2393
                };
            vehicleModels.Add(model_StarAuto_Pickup);

            var model_GlobalVehicle_Light_Hatch =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("A66A0835-A175-4E95-A3D4-D410BB0961A3"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Agile-HA",
                    MotorVehicleType = Ent.MotorVehicleType.HatchBack,
                    FuelCapacity = 1.4,
                    FuelConsumption = 5.9,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 82,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_GlobalVehicle_Light_Hatch);

            var model_GlobalVehicle_Light_Sedan =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("78F1A964-7038-41BE-9DA5-0F04FFC399E6"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Agile-SA",
                    MotorVehicleType = Ent.MotorVehicleType.Sedan,
                    FuelCapacity = 1.4,
                    FuelConsumption = 5.9,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 82,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_GlobalVehicle_Light_Sedan);

            var model_GlobalVehicle_Mid_Hatch =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("3C05C36C-94A3-4038-84AA-0A6D2048C9B5"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Central-HA",
                    MotorVehicleType = Ent.MotorVehicleType.HatchBack,
                    FuelCapacity = 2.5,
                    FuelConsumption = 6.6,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 155,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_GlobalVehicle_Mid_Hatch);

            var model_GlobalVehicle_Mid_Sedan =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("ABFFFDB6-114A-44BF-9A4E-43C371D59BDE"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Central-SA",
                    MotorVehicleType = Ent.MotorVehicleType.Sedan,
                    FuelCapacity = 2.5,
                    FuelConsumption = 6.6,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 155,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_GlobalVehicle_Mid_Sedan);

            var model_GlobalVehicle_Mid_Hybrid =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("46289B68-BF84-4C6D-8303-668A39E5FD92"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Central-HH",
                    MotorVehicleType = Ent.MotorVehicleType.HatchBack,
                    FuelCapacity = 1.7,
                    FuelConsumption = 4.0,
                    EngineType = Ent.EngineType.Hybrid_Electric_Petrol,
                    MaxPower = 105,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false
                };
            vehicleModels.Add(model_GlobalVehicle_Mid_Hybrid);

            var model_GlobalVehicle_CompactSUV =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("75A0CA9C-1769-405D-918D-C4D41C82C076"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Scout-A",
                    MotorVehicleType = Ent.MotorVehicleType.CompactSUV,
                    FuelCapacity = 2.5,
                    FuelConsumption = 7.2,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 165,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = true
                };
            vehicleModels.Add(model_GlobalVehicle_CompactSUV);

            var model_GlobalVehicle_SUV =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("E1244EC9-F9EE-4C30-9126-68D9CA5FF51A"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Explore-A",
                    MotorVehicleType = Ent.MotorVehicleType.SUV,
                    FuelCapacity = 2.0,
                    FuelConsumption = 6.2,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 130,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = true
                };
            vehicleModels.Add(model_GlobalVehicle_SUV);

            var model_GlobalVehicle_MiniBus =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("2094F987-423A-451A-BF22-8F0EBF93AAA2"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Social-A",
                    MotorVehicleType = Ent.MotorVehicleType.MiniBus,
                    FuelCapacity = 2.5,
                    FuelConsumption = 8.8,
                    EngineType = Ent.EngineType.Petrol,
                    MaxPower = 140,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = true
                };
            vehicleModels.Add(model_GlobalVehicle_MiniBus);

            var model_GlobalVehicle_VanManual =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("C9C3B06F-86A2-438E-96FC-6DD21A9969A4"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Caravan-VM",
                    MotorVehicleType = Ent.MotorVehicleType.Van,
                    FuelCapacity = 2.5,
                    FuelConsumption = 5.5,
                    EngineType = Ent.EngineType.Diesel,
                    MaxPower = 110,
                    TransmissionType = Ent.TransmissionType.Manual,
                    AWD = false,
                    CargoVolume = 2992
                };
            vehicleModels.Add(model_GlobalVehicle_VanManual);

            var model_GlobalVehicle_VanAuto =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("3DCDC64C-76DE-484A-8B35-6C7476AF5E85"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Caravan-VA",
                    MotorVehicleType = Ent.MotorVehicleType.Van,
                    FuelCapacity = 2.5,
                    FuelConsumption = 5.5,
                    EngineType = Ent.EngineType.Diesel,
                    MaxPower = 110,
                    TransmissionType = Ent.TransmissionType.Auto,
                    AWD = false,
                    CargoVolume = 2992
                };
            vehicleModels.Add(model_GlobalVehicle_VanAuto);

            var model_GlobalVehicle_Pickup =
                new Ent.MotorVehicleModel()
                {
                    MotorVehicleModelId = new Guid("09F010DE-2910-4322-9909-A20DD804AA7D"),
                    VehicleMakeId = make_GlobalVehicle.VehicleMakeId,
                    Name = "Atlas-PM",
                    MotorVehicleType = Ent.MotorVehicleType.Pickup,
                    FuelCapacity = 2.5,
                    FuelConsumption = 8.4,
                    EngineType = Ent.EngineType.Diesel,
                    MaxPower = 120,
                    TransmissionType = Ent.TransmissionType.Manual,
                    AWD = false,
                    CargoVolume = 2495
                };
            vehicleModels.Add(model_GlobalVehicle_Pickup);

            Queue<Ent.MotorVehicleModel> qModels = GenericListHelper.ToQueue(vehicleModels);
            
            //I chose a rather simplistic model for Address, Country, State, City and Suburbs
            //This is decided for simplicity's sake and I am not actually working with any
            //domain expert users (Jimmy Nilson: Applying Domain-Driven Design and Patterns) 

            //Fictional Country "Knotreel" and it's States, Cities and Suburbs
            //"Knotreel" is, at the time of creating this code, not listed in https://en.wikipedia.org/wiki/List_of_fictional_countries
            //However please inform me if it is already copyrighted and I will change it

            List<string> countryNames = new List<string>() { "Knotreel" };
            
            List<string> existingCountryNames = new List<string>();
            List<string> existingStateNames = new List<string>();
            List<string> existingCityNames = new List<string>();
            List<string> existingSuburbNames = new List<string>();
            List<string> existingStreetNames = new List<string>();
            List<string> existingVINs = new List<string>();
            List<string> existingVehicleRegistrations = new List<string>();

            List<Ent.Country> countries = new List<Ent.Country>();
            List<Ent.State> states = new List<Ent.State>();
            List<Ent.City> cities = new List<Ent.City>();
            List<Ent.Suburb> suburbs = new List<Ent.Suburb>();
            List<Ent.Address> addresses = new List<Ent.Address>();
            List<Ent.Location> locations = new List<Ent.Location>();
            List<Ent.MotorVehicle> vehicles = new List<Ent.MotorVehicle>();

            int stateCount = 7;
            int suburbCount = 5;
            for (int c = 0; c < countryNames.Count; c++)
            {
                var country =
                    new Ent.Country()
                    {
                        CountryId = Guid.NewGuid(),
                        Name = countryNames[c]
                    };
                countries.Add(country);

                for (int st = 0; st < stateCount; st++)
                {
                    var state =
                        new Ent.State()
                        {
                            StateId = Guid.NewGuid(),
                            CountryId = country.CountryId,
                            Name = InfoGenerationHelper.GenerateStateName(existingStateNames),
                        };
                    states.Add(state);

                    var city =
                        new Ent.City()
                        {
                            CityId = Guid.NewGuid(),
                            StateId = state.StateId,
                            Name = InfoGenerationHelper.GenerateCityName(existingCityNames),
                        };
                    cities.Add(city);

                    for (int b = 0; b < suburbCount; b++)
                    {
                        int streetNumber = InfoGenerationHelper.GenerateInteger(1, 30);
                        int parkingCapacity = 15 + InfoGenerationHelper.GenerateInteger(5, 20);
                        string postalcode = $"{c:0}{st:0}{(st + InfoGenerationHelper.GenerateInteger(0, 5)):00}{b:00}{streetNumber:00}";

                        var suburb =
                            new Ent.Suburb()
                            {
                                SuburbId = Guid.NewGuid(),
                                CityId = city.CityId,
                                Name = InfoGenerationHelper.GenerateSuburbName(existingSuburbNames),
                            };
                        suburbs.Add(suburb);

                        var address =
                            new Ent.Address()
                            {
                                AddressId = Guid.NewGuid(),
                                SuburbId = suburb.SuburbId,
                                AddressLine1 = string.Format("{0} {1}", streetNumber, InfoGenerationHelper.GenerateStreetName(existingStreetNames)),
                                PostalCode = postalcode
                            };
                        addresses.Add(address);

                        var location =
                            new Ent.Location()
                            {
                                LocationId = Guid.NewGuid(),
                                Name = existingStreetNames.Last(),
                                AddressId = address.AddressId,
                                ParkingCapacity = parkingCapacity
                            };
                        locations.Add(location);

                        for (int p = 0; p < parkingCapacity; p++)
                        {
                            Ent.MotorVehicleModel vehModel = qModels.Dequeue();

                            int Year = 2010 + InfoGenerationHelper.GenerateInteger(0, 11);
                            int Mileage = InfoGenerationHelper.GenerateInteger(0, 200000);

                            Ent.PickupCargoAccessoryType PickupCargoAccessoryType = Ent.PickupCargoAccessoryType.None;
                            if (vehModel.MotorVehicleType == Ent.MotorVehicleType.Pickup)
                            {
                                int r = InfoGenerationHelper.GenerateInteger(1, 100);
                                if (r >= 33 && r <= 66)
                                {
                                    PickupCargoAccessoryType = Ent.PickupCargoAccessoryType.TonneauCover;
                                }
                                else if (r > 66)
                                {
                                    PickupCargoAccessoryType = Ent.PickupCargoAccessoryType.TruckCap;
                                }
                            }

                            Ent.MotorVehicle vehicle = new Ent.MotorVehicle()
                            {
                                Id = Guid.NewGuid(),
                                MotorVehicleModelId = vehModel.MotorVehicleModelId,
                                Year = Year,
                                VIN = InfoGenerationHelper.GenerateVIN(existingVINs),
                                Registration = InfoGenerationHelper.GenerateVehicleRegistration(existingVehicleRegistrations),
                                Mileage = Mileage,
                                HasNavSystem = InfoGenerationHelper.GenerateBoolean(),
                                HasDashCam = InfoGenerationHelper.GenerateBoolean(),
                                HasBlindspotMonitoring = InfoGenerationHelper.GenerateBoolean(),
                                HasAutomaticEmergencyBrake = InfoGenerationHelper.GenerateBoolean(),
                                HasReversingCam = InfoGenerationHelper.GenerateBoolean(),
                                HasForwardParkingSensor = InfoGenerationHelper.GenerateBoolean(),
                                HasRearParkingSensor = InfoGenerationHelper.GenerateBoolean(),
                                PickupCargoAccessoryType = PickupCargoAccessoryType
                            };
                            vehicles.Add(vehicle);
                            location.Fleet.Add(vehicle);

                            qModels.Enqueue(vehModel);
                        }
                    }
                }
            }

            context.AddOrUpdate(
                contextQuery,
                x => x.VehicleMakeId,
                vehicleMakes.ToArray()
            );

            context.AddOrUpdate(
                contextQuery,
                x => x.MotorVehicleModelId,
                vehicleModels.ToArray()
            );

            context.AddOrUpdate(
                contextQuery,
                x => x.CountryId,
                countries.ToArray());

            context.AddOrUpdate(
                contextQuery,
                x => x.StateId,
                states.ToArray());

            context.AddOrUpdate(
                contextQuery,
                x => x.CityId,
                cities.ToArray());

            context.AddOrUpdate(
                contextQuery,
                x => x.SuburbId,
                suburbs.ToArray());

            context.AddOrUpdate(
                contextQuery,
                x => x.AddressId,
                addresses.ToArray());

            context.AddOrUpdate(
                contextQuery,
                x => x.LocationId,
                locations.ToArray());

            context.AddOrUpdate(
                contextQuery,
                x => x.Id,
                vehicles.ToArray());

            context.SaveChanges();
        }
    }
}
