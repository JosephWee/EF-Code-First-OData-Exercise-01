using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModelTest
{
    public class InfoGenerationHelper
    {
        private static Random rand = new Random();

        private static Queue<Tuple<string, bool>> regionNames = new Queue<Tuple<string, bool>>();

        private static List<Tuple<string, bool>> regionTypes = new List<Tuple<string, bool>>() { new Tuple<string, bool>("Coasts", true), new Tuple<string, bool>("Peaks", false), new Tuple<string, bool>("Ridges", false), new Tuple<string, bool>("Hills", false), new Tuple<string, bool>("Plains", false) };

        private static List<string> cityNames_Part1 = new List<string>() { "Winter", "Haven", "Storm", "Frost", "Wind" };
        private static List<string> cityNames_Part2 = new List<string>() { "City", "Hold", "Deep" };

        private static List<string> suburbNames = new List<string>() { "Blue", "Green", "Maroon", "Amber", "Minners", "Merchants", "Traders", "Market", "Castle", "Monument", "Capitol" };
        private static List<string> suburbTypes = new List<string>() { "District", "Circle", "Gardens", "Square", "Walk", "Gate" };

        private static List<string> streetNames = new List<string>() { "Main", "Station", "Tailor", "Bond", "Orchard", "Orange", "Oak", "Acacia", "Copper", "Helm", "Watt", "Wallace", "Toll", "Blacksmith", "Kings", "Queens", "River", "Fence", "Lakeview", "Cart", "Wagon", "Mountain View" };
        private static List<string> streetTypes = new List<string>() { "Street", "Road", "Rise", "Heights", "Grove" };

        static InfoGenerationHelper()
        {
            List<Tuple<string, bool>> listRegionNames = new List<Tuple<string, bool>>() { new Tuple<string, bool>("North", true), new Tuple<string, bool>("South", true), new Tuple<string, bool>("East", true), new Tuple<string, bool>("West", true), new Tuple<string, bool>("Central", false) };
            regionNames = GenericListHelper.ToQueue(listRegionNames, true);
        }

        public static string GenerateStateName(ICollection<string> existingStateNames = null)
        {
            if (existingStateNames == null)
                existingStateNames = new List<string>();

            string stateName = null;

            do
            {
                var regionName = regionNames.Dequeue();

                if (regionName.Item2)
                {
                    int index = rand.Next(0, regionTypes.Count - 1);
                    stateName =
                        string.Format(
                            "{0} {1}",
                            regionName.Item1,
                            regionTypes.ElementAt(index).Item1);
                }
                else
                {
                    int index = rand.Next(0, regionTypes.Count(x => x.Item2 == false) - 1);
                    stateName =
                        string.Format(
                            "{0} {1}",
                            regionName.Item1,
                            regionTypes.Where(x => x.Item2 == false).ElementAt(index).Item1);
                }

                regionNames.Enqueue(regionName);
            }
            while (existingStateNames.Contains(stateName));

            if (!string.IsNullOrEmpty(stateName))
                existingStateNames.Add(stateName);

            return stateName;
        }

        public static string GenerateCityName(ICollection<string> existingCityNames = null)
        {
            if (existingCityNames == null)
                existingCityNames = new List<string>();

            string cityName = null;
            do
            {
                cityName =
                    string.Format(
                        "{0} {1}",
                        cityNames_Part1[rand.Next(0, cityNames_Part1.Count - 1)],
                        cityNames_Part2[rand.Next(0, cityNames_Part2.Count - 1)]
                    );
            } while (existingCityNames.Contains(cityName));

            if (!string.IsNullOrEmpty(cityName))
                existingCityNames.Add(cityName);

            return cityName;
        }

        public static string GenerateSuburbName(ICollection<string> existingSuburbNames = null)
        {
            if (existingSuburbNames == null)
                existingSuburbNames = new List<string>();

            string suburbName = null;
            do
            {
                suburbName =
                    string.Format(
                        "{0} {1}",
                        suburbNames[rand.Next(0, suburbNames.Count - 1)],
                        suburbTypes[rand.Next(0, suburbTypes.Count - 1)]
                    );
            } while (existingSuburbNames.Contains(suburbName));

            if (!string.IsNullOrEmpty(suburbName))
                existingSuburbNames.Add(suburbName);

            return suburbName;
        }

        public static string GenerateStreetName(ICollection<string> existingStreetNames = null)
        {
            if (existingStreetNames == null)
                existingStreetNames = new List<string>();

            string streetName = null;
            do
            {
                streetName =
                    string.Format(
                        "{0} {1}",
                        streetNames[rand.Next(0, streetNames.Count - 1)],
                        streetTypes[rand.Next(0, streetTypes.Count - 1)]
                    );
            } while (existingStreetNames.Contains(streetName));

            if (!string.IsNullOrEmpty(streetName))
                existingStreetNames.Add(streetName);

            return streetName;
        }

        public static string GenerateVIN(ICollection<string> existingVINs = null)
        {
            if (existingVINs == null)
                existingVINs = new List<string>();

            string VIN = null;
            do
            {
                StringBuilder builderVIN = new StringBuilder();

                for (int vin = 0; vin < 17; vin++)
                {
                    if (vin == 0 || vin == 3 || vin == 9)
                    {
                        builderVIN.Append(Convert.ToChar(rand.Next(65, 87)));
                    }
                    else
                    {
                        builderVIN.Append(rand.Next(0, 9));
                    }
                }

                VIN = builderVIN.ToString();

            } while (existingVINs.Contains(VIN));

            if (!string.IsNullOrEmpty(VIN))
                existingVINs.Add(VIN);

            return VIN;
        }

        public static string GenerateVehicleRegistration(ICollection<string> existingVehicleRegistrations = null)
        {
            if (existingVehicleRegistrations == null)
                existingVehicleRegistrations = new List<string>();

            string RegistrationNumber = null;

            do
            {
                RegistrationNumber =
                    string.Format(
                        "S{0}{1}{2:0000}{3}",
                        Convert.ToChar(rand.Next(65, 87)),
                        Convert.ToChar(rand.Next(65, 87)),
                        rand.Next(0, 1000),
                        Convert.ToChar(rand.Next(65, 87))
                    );
            } while (existingVehicleRegistrations.Contains(RegistrationNumber));

            if (!string.IsNullOrEmpty(RegistrationNumber))
                existingVehicleRegistrations.Add(RegistrationNumber);

            return RegistrationNumber;
        }

        public static int GenerateInteger(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static List<int> GenerateIntegerList(int min, int max, int count)
        {
            List<int> results = new List<int>();
            
            for (int i = 0; i < count; i++)
            {
                results.Add(
                    rand.Next(min, max)
                );
            }

            return results;
        }

        public static bool GenerateBoolean()
        {
            return (rand.Next(1, 100) >= 50);
        }
    }
}
