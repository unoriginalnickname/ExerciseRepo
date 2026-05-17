using GaragePractice;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Övning___4
{
    public static class VehicleFactory
    {
        public static IVehicle CreateVehicle(FilterX filter)
        {
            if (!VehicleCreators.TryGetValue(filter.VehicleType, out var factory))
                throw new ArgumentException($"Vehicle type '{filter.VehicleType}' is not approved.");

            var vehicle = factory();
            vehicle.RegistryNumber = filter.RegNumber ?? throw new ArgumentException("RegNumber is required");
            vehicle.Fueltype = filter.FuelType ?? throw new ArgumentException("FuelType is required");
            vehicle.Color = filter.Color ?? "Unknown";
            vehicle.NumWheels = filter.NumWheels ?? throw new ArgumentException("NumWheels is required");
            vehicle.UniquePropertyValue = filter.UniquePropertyValue ?? "Unknown";

            return vehicle;
        }

        private static readonly Dictionary<string, Func<IVehicle>> VehicleCreators = new()
{
    { "Car", () => new Car() },
    { "Bus", () => new Bus() },
    { "Motorcycle", () => new Motorcycle() },
    { "Airplane", () => new Airplane() },
    { "Boat", () => new Boat() },
    { "Ufo", () => new Ufo() },
    { "Uap", () => new Uap() }
}; 
        
        public static IVehicle CreateRandomVehicle<T>(Garage<T> garage, HashSet<string> existingRegs) where T : IVehicle
        {
            IVehicle vehicle;
            do
            {
                vehicle = CreateRandomVehicle(garage); // existing logic
            } while (existingRegs.Contains(vehicle.RegistryNumber));

            return vehicle;
        }

        public static IVehicle CreateRandomVehicle<T>(Garage<T> garage) where T : IVehicle
        {
            try
            {
                Type vehicleType = typeof(T);

                // If garage is generic over IVehicle, pick a random approved type
                if (vehicleType == typeof(IVehicle))
                {
                    // Pick a random approved vehicle type from the garage dictionary
                    string typeName = RandomHelper.Pick(garage.ApprovedVehicleTypes.Keys.ToList());
                    Type? pickedType = Type.GetType("GaragePractice." + typeName);

                    if (pickedType == null)
                        throw new InvalidOperationException($"Type '{typeName}' could not be found.");

                    vehicleType = pickedType;
                }


                // Build filter with random properties
                var filter = new FilterX
                {
                    VehicleType = vehicleType.Name,
                    RegNumber = GenerateRegNumber(),
                    Color = RandomHelper.Pick(new List<string> { "Red", "Blue", "Black", "White", "Green", "Orange", "Magenta", "Chrome" }),
                    FuelType = RandomHelper.Pick(new List<string> { "Petrol", "Diesel", "Electric", "Hybrid" }),
                    NumWheels = RandomHelper.Pick(new List<int> { 2, 4, 6, 8 }),
                    UniquePropertyValue = uniquePropertyValueGenerators[vehicleType]()
                };

                return CreateVehicle(filter);
            }
            catch (Exception ex) 
            {
                // Log or handle gracefully
                throw;
            }

        }


        private static Dictionary<Type, Func<string>> uniquePropertyValueGenerators = new()
{
    { typeof(Airplane),   () => $"{Random.Shared.Next(10, 80)} m" },
    { typeof(Boat),       () => $"{Random.Shared.Next(5, 200)} m" },
    { typeof(Bus),        () => $"{Random.Shared.Next(10, 200)} stops" },
    { typeof(Car),        () => $"{Random.Shared.Next(2, 6)} doors" },
    { typeof(Motorcycle), () => $"{Random.Shared.Next(50, 1500)} cc" },
    { typeof(Ufo),        () => $"{Random.Shared.Next(1, 1000)} abductees" },
    { typeof(Uap),        () => "Classified" }
};
        private static Random random = new Random();
        public static string GenerateRegNumber()
        {

            string letters = new string(
                Enumerable.Range(0, 3)
                    .Select(_ => (char)random.Next('A', 'Z' + 1))
                    .ToArray());

            string numbers = random.Next(100, 999).ToString();

            return letters + numbers;
        }
    }
}