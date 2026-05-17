using GaragePractice;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Övning___4
{
    public static class VehicleFactory
    {
        private static Random random = new Random();
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

        public static IVehicle CreateVehicle(Filter filter)
        {
            if (!VehicleCreators.TryGetValue(filter.VehicleType, out var factory))
                throw new ArgumentException($"Vehicle type '{filter.VehicleType}' is not approved.");

            var vehicle = factory();
            vehicle.RegistryNumber = filter.RegistryNumber ?? throw new ArgumentException("RegNumber is required");
            vehicle.FuelType = filter.FuelType ?? throw new ArgumentException("FuelType is required");
            vehicle.Color = filter.Color ?? "Unknown";
            vehicle.NumWheels = filter.NumWheels ?? throw new ArgumentException("NumWheels is required");
            vehicle.UniquePropertyValue = filter.UniquePropertyValue ?? "Unknown";
            return vehicle;
        }
        
        public static IVehicle CreateRandomVehicle(Type type)
        {
            try
            {
                Type vehicleType = type;
                if (vehicleType == typeof(IVehicle))
                {
                    string typeName = RandomHelper.Pick(VehicleTypeRegistry.ApprovedVehicleTypes.Keys.ToList());
                    Type? pickedType = Type.GetType("GaragePractice." + typeName);
                    if (pickedType == null)
                        throw new InvalidOperationException($"Type '{typeName}' could not be found.");
                    vehicleType = pickedType;
                }

                var filter = new Filter
                {
                    VehicleType = vehicleType.Name,
                    RegistryNumber = GenerateRegNumber(),
                    Color = RandomHelper.Pick(new List<string> { "Red", "Blue", "Black", "White", "Green", "Orange", "Magenta", "Chrome" }),
                    FuelType = RandomHelper.Pick(new List<string> { "Petrol", "Diesel", "Electric", "Hybrid" }),
                    NumWheels = RandomHelper.Pick(new List<int> { 2, 4, 6, 8 }),
                    UniquePropertyValue = uniquePropertyValueGenerators[vehicleType]()
                };

                return CreateVehicle(filter);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

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