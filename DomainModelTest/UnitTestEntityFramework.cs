using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using DomainModel;
using Ent = DomainModel.Entities;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;

namespace DomainModelTest
{
    [TestClass]
    public class UnitTestEntityFramework
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [TestInitialize]
        public void TestInitialize()
        {
        }

        protected void AssertDatabaseHasData(VehicleRentalContext context)
        {
            Assert.IsTrue(context.MotorVehicles.Count() > 0);
            Assert.IsTrue(context.MotorVehicleModels.Count() > 0);
            Assert.IsTrue(context.VehicleMakes.Count() > 0);

            Assert.IsTrue(context.Locations.Count() > 0);
            Assert.IsTrue(context.Addresses.Count() > 0);
            Assert.IsTrue(context.Suburbs.Count() > 0);
            Assert.IsTrue(context.Cities.Count() > 0);
            Assert.IsTrue(context.States.Count() > 0);
            Assert.IsTrue(context.Countries.Count() > 0);
        }

        protected void AssertDatabaseIsEmpty(VehicleRentalContext context)
        {
            Assert.IsTrue(context.MotorVehicles.Count() == 0);
            Assert.IsTrue(context.MotorVehicleModels.Count() == 0);
            Assert.IsTrue(context.VehicleMakes.Count() == 0);

            Assert.IsTrue(context.Locations.Count() == 0);
            Assert.IsTrue(context.Addresses.Count() == 0);
            Assert.IsTrue(context.Suburbs.Count() == 0);
            Assert.IsTrue(context.Cities.Count() == 0);
            Assert.IsTrue(context.States.Count() == 0);
            Assert.IsTrue(context.Countries.Count() == 0);
        }

        [TestMethod]
        [TestCategory("EF6 -  Integrated Ops")]
        public void EmptyDB()
        {
            var context = new VehicleRentalContext();
            context.Database.Log = msg => log.Debug(msg);

            AssertDatabaseHasData(context);
            
            VehicleRentalContextHelper.EmptyDatabase(context);

            AssertDatabaseIsEmpty(context);
        }

        [TestMethod]
        [TestCategory("EF6 -  Integrated Ops")]
        public void PopulateDB()
        {
            var context = new VehicleRentalContext();
            context.Database.Log = msg => log.Debug(msg);

            var queryContext = new VehicleRentalContext();
            queryContext.Database.Log = msg => log.Debug(msg);

            AssertDatabaseIsEmpty(context);

            VehicleRentalContextHelper.PopulateDatabase(context, queryContext);

            AssertDatabaseHasData(context);

            int fleetCount = 0;
            Dictionary<Guid, int> dictModelCount = new Dictionary<Guid, int>();
            foreach (var location in context.Locations.ToList())
            {
                fleetCount += location.Fleet.Count();

                var gByModel_Fleet =
                    location.Fleet.GroupBy(x => x.MotorVehicleModel).ToList();

                foreach (var fleetForModel in gByModel_Fleet)
                {
                    if (dictModelCount.ContainsKey(fleetForModel.Key.MotorVehicleModelId))
                    {
                        int existingCount =
                            dictModelCount[fleetForModel.Key.MotorVehicleModelId] + fleetForModel.Count();
                        dictModelCount[fleetForModel.Key.MotorVehicleModelId] = existingCount;
                    }
                    else
                    {
                        dictModelCount[fleetForModel.Key.MotorVehicleModelId] = fleetForModel.Count();
                    }
                }
            }

            var gByModel_Vehicles =
                context.MotorVehicles.GroupBy(x => x.MotorVehicleModel).ToList();

            foreach (var vehiclesForModel in gByModel_Vehicles)
            {
                int existingCount = dictModelCount.ContainsKey(vehiclesForModel.Key.MotorVehicleModelId) ? dictModelCount[vehiclesForModel.Key.MotorVehicleModelId] : int.MinValue;
                Assert.AreNotEqual(existingCount, int.MinValue);
                Assert.AreEqual(existingCount, vehiclesForModel.Count());
            }
        }

        [TestMethod]
        [TestCategory("EF6 - Connected Scenario")]
        [TestCategory("EF6 - Update")]
        public void ChangeVehicleMake()
        {
            var context = new VehicleRentalContext();

            Assert.IsTrue(context.VehicleMakes.Count() >= 2);
            Assert.IsTrue(context.MotorVehicleModels.Count() > 0);

            var vehicleMakes = context.VehicleMakes.ToList();
            var vehicleModel = context.MotorVehicleModels.First();
            var vehicleMake = vehicleModel.VehicleMake;
            var otherVehicleMake = vehicleMakes.First(m => m.VehicleMakeId != vehicleMake.VehicleMakeId);

            vehicleModel.VehicleMakeId = otherVehicleMake.VehicleMakeId;

            context.SaveChanges();

            var vehicleModelAfterSave = context.MotorVehicleModels.First(m => m.MotorVehicleModelId == vehicleModel.MotorVehicleModelId);

            Assert.IsNotNull(vehicleModelAfterSave);
            Assert.AreEqual(vehicleModel.MotorVehicleModelId, vehicleModelAfterSave.MotorVehicleModelId);
            Assert.AreNotEqual(vehicleMake.VehicleMakeId, vehicleModelAfterSave.VehicleMakeId);
            Assert.AreEqual(otherVehicleMake.VehicleMakeId, vehicleModelAfterSave.VehicleMakeId);
        }

        [TestMethod]
        [TestCategory("EF6 - Connected Scenario")]
        [TestCategory("EF6 - Update")]
        public void RemoveVehicleFromFleet()
        {
            var context = new VehicleRentalContext();

            Assert.IsTrue(context.Locations.Count() > 0);
            Assert.IsTrue(context.MotorVehicles.Count(mv => mv.LocationId != null) > 0);

            var location = context.Locations.First(L => L.Fleet.Count() > 0);
            var fleetVehicle = location.Fleet.First();
            Guid? oldLocationId = fleetVehicle.LocationId;
            location.Fleet.Remove(fleetVehicle);
            
            context.SaveChanges();

            var vehicleAfterSave = context.MotorVehicles.First(mv => mv.Id == fleetVehicle.Id);

            Assert.IsNotNull(vehicleAfterSave);
            Assert.IsTrue(oldLocationId.HasValue);
            Assert.IsTrue(oldLocationId != Guid.Empty);
            Assert.IsNull(vehicleAfterSave.LocationId);
            Assert.IsNull(vehicleAfterSave.Location);
        }

        [TestMethod]
        [TestCategory("EF6 - Connected Scenario")]
        [TestCategory("EF6 - Update")]
        [TestCategory("EF6 - Delete")]
        [TestCategory("EF6 - Cascade Set Null")]
        public void DeleteLocationWithFleet()
        {
            var context = new VehicleRentalContext();

            Assert.IsTrue(context.Locations.Count(L => L.Fleet.Count() > 0) > 0);
            
            var locationWithFleet = context.Locations.First(L => L.Fleet.Count() > 0);
            var fleetIds = locationWithFleet.Fleet.Select(v => v.Id).ToList();
            Assert.IsTrue(fleetIds.Count > 0);

            context.Locations.Remove(locationWithFleet);

            context.SaveChanges();

            var locationAfterSave = context.Locations.FirstOrDefault(L => L.LocationId == locationWithFleet.LocationId);
            var formerFleetVehicles = context.MotorVehicles.Where(mv => fleetIds.Contains(mv.Id)).ToList();

            Assert.IsNull(locationAfterSave);
            Assert.AreEqual(fleetIds.Count, formerFleetVehicles.Count);
            Assert.AreEqual(fleetIds.Count, formerFleetVehicles.Count(mv => mv.LocationId == null));
        }

        [TestMethod]
        [TestCategory("EF6 -  Integrated Ops")]
        [TestCategory("EF6 - Delete")]
        [TestCategory("EF6 - Cascade Set Null")]
        [TestCategory("EF6 - Cascade Delete")]
        public void DeleteCountryThenMake()
        {
            var context = new VehicleRentalContext();

            Assert.IsTrue(context.MotorVehicles.Count() > 0);
            Assert.IsTrue(context.MotorVehicleModels.Count() > 0);
            Assert.IsTrue(context.VehicleMakes.Count() > 0);

            Assert.IsTrue(context.Locations.Count() > 0);
            Assert.IsTrue(context.Addresses.Count() > 0);
            Assert.IsTrue(context.Suburbs.Count() > 0);
            Assert.IsTrue(context.Cities.Count() > 0);
            Assert.IsTrue(context.States.Count() > 0);
            Assert.IsTrue(context.Countries.Count() > 0);

            context.Countries.RemoveRange(
                context.Countries.ToList()
            );

            context.SaveChanges();

            Assert.IsTrue(context.MotorVehicles.Count() > 0);
            Assert.IsTrue(context.MotorVehicleModels.Count() > 0);
            Assert.IsTrue(context.VehicleMakes.Count() > 0);

            Assert.IsTrue(context.Locations.Count() == 0);
            Assert.IsTrue(context.Addresses.Count() == 0);
            Assert.IsTrue(context.Suburbs.Count() == 0);
            Assert.IsTrue(context.Cities.Count() == 0);
            Assert.IsTrue(context.States.Count() == 0);
            Assert.IsTrue(context.Countries.Count() == 0);

            context.VehicleMakes.RemoveRange(
                context.VehicleMakes.ToList()
            );

            context.SaveChanges();

            Assert.IsTrue(context.MotorVehicles.Count() == 0);
            Assert.IsTrue(context.MotorVehicleModels.Count() == 0);
            Assert.IsTrue(context.VehicleMakes.Count() == 0);

            Assert.IsTrue(context.Locations.Count() == 0);
            Assert.IsTrue(context.Addresses.Count() == 0);
            Assert.IsTrue(context.Suburbs.Count() == 0);
            Assert.IsTrue(context.Cities.Count() == 0);
            Assert.IsTrue(context.States.Count() == 0);
            Assert.IsTrue(context.Countries.Count() == 0);
        }

        [TestMethod]
        [TestCategory("EF6 - Connected Scenario")]
        [TestCategory("EF6 - Add")]
        public void AddLocation()
        {
            var context = new VehicleRentalContext();

            AssertDatabaseHasData(context);

            var existingAddrs =
                context.Addresses.ToList();

            var existingLocations =
                context.Locations.ToList();

            List<Tuple<int, string, Ent.Address>> existingAddrParts =
                new List<Tuple<int, string, Ent.Address>>();

            foreach (var addr in existingAddrs)
            {
                int streetNumber = int.MinValue;
                string streetName = string.Empty;

                Regex regex = new Regex(@"^(\d+)\s+(\D+)$", RegexOptions.IgnoreCase);
                Match m = regex.Match(addr.AddressLine1.Trim());
                if (m.Success && m.Groups.Count > 2)
                {
                    streetNumber = int.Parse(m.Groups[1].Value);
                    streetName = m.Groups[2].Value;
                }

                Tuple<int, string, Ent.Address> tup =
                    new Tuple<int, string, Ent.Address>(
                        streetNumber,
                        streetName,
                        addr);

                existingAddrParts.Add(tup);
            }

            var existingAddr = existingAddrParts.First();

            int newStreetNumber = int.MinValue;
            do
            {
                newStreetNumber = InfoGenerationHelper.GenerateInteger(1, 30);
            } while (newStreetNumber == existingAddr.Item1);

            string partialPostalCode =
                existingAddr.Item3.PostalCode
                .Substring(
                    0,
                    existingAddr.Item3.PostalCode.Length - 2
                );

            Ent.Address newAddress =
                new Ent.Address()
                {
                    AddressId = Guid.NewGuid(),
                    AddressLine1 =
                        newStreetNumber + " " + existingAddr.Item2,
                    SuburbId = existingAddr.Item3.SuburbId,
                    PostalCode =
                        string.Format(
                            "{0}{1:00}",
                            partialPostalCode,
                            newStreetNumber)
                };

            context.Addresses.Add(newAddress);

            Ent.Location newLocation = new Ent.Location()
            {
                LocationId = Guid.NewGuid(),
                AddressId = newAddress.AddressId,
                ParkingCapacity = 15 + InfoGenerationHelper.GenerateInteger(5, 20)
            };

            context.Locations.Add(newLocation);

            context.SaveChanges();

            var addedAddr =
                context.Addresses.FirstOrDefault(addr => addr.AddressId == newAddress.AddressId);

            var addedLocation =
                context.Locations.FirstOrDefault(loc => loc.LocationId == newLocation.LocationId);

            Assert.IsNotNull(addedAddr);
            Assert.IsNotNull(addedLocation);
            Assert.AreEqual(addedAddr.AddressId, addedLocation.AddressId);
        }

        /// <summary>
        /// For more information on EF6 Concurrency see:
        /// https://www.entityframeworktutorial.net/code-first/TimeStamp-dataannotations-attribute-in-code-first.aspx
        /// https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
        /// https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.timestampattribute?view=net-5.0
        /// https://docs.microsoft.com/en-us/ef/ef6/saving/concurrency
        /// </summary>
        [TestMethod]
        [TestCategory("EF6 - Connected Scenario")]
        [TestCategory("EF6 - Concurrency Conflict")]
        public void ConcurrencyConflictDBWins()
        {
            var context1 = new VehicleRentalContext();
            var context2 = new VehicleRentalContext();

            Assert.IsTrue(context1.MotorVehicles.Any());

            var vehicle1 = context1.MotorVehicles.First();
            var vehicle2 = context2.MotorVehicles.Find(vehicle1.Id);

            int vehicle1SaveAttempts = 0;
            bool changeMileageSuccess = false;
            while (!changeMileageSuccess)
            {
                try
                {
                    vehicle1SaveAttempts++;
                    vehicle1.Mileage = vehicle1.Mileage + 1;
                    context1.SaveChanges();
                    changeMileageSuccess = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                }
            }

            Assert.AreNotEqual(vehicle1.TimeStamp, vehicle2.TimeStamp);

            int vehicle2SaveAttempts = 0;
            bool changeFrontParkingSensorSuccess = false;
            while (!changeFrontParkingSensorSuccess)
            {
                try
                {
                    vehicle2SaveAttempts++;
                    vehicle2.HasForwardParkingSensor = !vehicle2.HasForwardParkingSensor;
                    context2.SaveChanges();
                    changeFrontParkingSensorSuccess = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                }
            }

            Assert.IsTrue(changeMileageSuccess);
            Assert.IsTrue(changeFrontParkingSensorSuccess);
            Assert.AreEqual(1, vehicle1SaveAttempts);
            Assert.AreEqual(2, vehicle2SaveAttempts);
        }

        /// <summary>
        /// For more information on EF6 Concurrency see:
        /// https://www.entityframeworktutorial.net/code-first/TimeStamp-dataannotations-attribute-in-code-first.aspx
        /// https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
        /// https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.timestampattribute?view=net-5.0
        /// https://docs.microsoft.com/en-us/ef/ef6/saving/concurrency
        /// </summary>
        [TestMethod]
        [TestCategory("EF6 - Connected Scenario")]
        [TestCategory("EF6 - Concurrency Conflict")]
        public void ConcurrencyConflictClientWins()
        {
            var context1 = new VehicleRentalContext();
            var context2 = new VehicleRentalContext();

            Assert.IsTrue(context1.MotorVehicles.Any());

            var vehicle1 = context1.MotorVehicles.First();
            var vehicle2 = context2.MotorVehicles.Find(vehicle1.Id);

            int vehicle1SaveAttempts = 0;
            bool changeMileageSuccess = false;
            while (!changeMileageSuccess)
            {
                try
                {
                    vehicle1SaveAttempts++;
                    vehicle1.Mileage = vehicle1.Mileage + 1;
                    context1.SaveChanges();
                    changeMileageSuccess = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            }

            Assert.AreNotEqual(vehicle1.TimeStamp, vehicle2.TimeStamp);

            int vehicle2SaveAttempts = 0;
            bool changeFrontParkingSensorSuccess = false;
            while (!changeFrontParkingSensorSuccess)
            {
                try
                {
                    vehicle2SaveAttempts++;
                    vehicle2.HasForwardParkingSensor = !vehicle2.HasForwardParkingSensor;
                    context2.SaveChanges();
                    changeFrontParkingSensorSuccess = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            }

            Assert.IsTrue(changeMileageSuccess);
            Assert.IsTrue(changeFrontParkingSensorSuccess);
            Assert.AreEqual(1, vehicle1SaveAttempts);
            Assert.AreEqual(2, vehicle2SaveAttempts);
        }
    }
}
