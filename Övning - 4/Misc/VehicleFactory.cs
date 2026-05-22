using Övning___4.ViewModel;

namespace Övning___4.Misc
{
    public static class VehicleFactory
    {

        private static Dictionary<Type, string> uniquePropertyStrings = new()
{
    { typeof(Airplane),   "Wingspan" },
    { typeof(Boat),       "Length" },
    { typeof(Bus),        "Stops" },
    { typeof(Car),        "Doors" },
    { typeof(Motorcycle), "Engine" },
    { typeof(Ufo),        "Abductees" },
    { typeof(Uap),        "Details" }
};

        private static Dictionary<Type, Func<string>> uniquePropertyValue = new()
{
    { typeof(Airplane),   () => $"{Random.Shared.Next(10, 80)} m" },
    { typeof(Boat),       () => $"{Random.Shared.Next(5, 200)} m" },
    { typeof(Bus),        () => $"{Random.Shared.Next(10, 200)}" },
    { typeof(Car),        () => $"{Random.Shared.Next(2, 6)}" },
    { typeof(Motorcycle), () => $"{Random.Shared.Next(50, 1500)} cc" },
    { typeof(Ufo),        () => $"{Random.Shared.Next(1, 1000)}" },
    { typeof(Uap),        () => "Classified" }
};

        private static readonly List<string> Colors = new() { "Red", "Blue", "Black", "White", "Green", "Orange", "Magenta", "Chrome" };
        private static readonly List<string> FuelTypes = new() { "Petrol", "Diesel", "Electric", "Hybrid" };
        private static readonly List<int> WheelOptions = new() { 2, 4, 6, 8 };

        public static IVehicle CreateVehicle(Filter filter)
        {
            var vehicleType = Type.GetType(filter.VehicleType)
                ?? throw new ArgumentException($"Unknown vehicle type '{filter.VehicleType}'");

            var vehicle = (IVehicle)(Activator.CreateInstance(vehicleType)
                ?? throw new ArgumentException($"Could not create vehicle of type '{filter.VehicleType}'"));

            vehicle.RegistryNumber = filter.RegistryNumber ?? throw new ArgumentException("RegNumber is required");
            vehicle.FuelType = filter.FuelType ?? throw new ArgumentException("FuelType is required");
            vehicle.NumWheels = filter.NumWheels ?? throw new ArgumentException("NumWheels is required");
            vehicle.Color = filter.Color ?? "Unknown";
            vehicle.UniquePropertyValue = filter.UniquePropertyValue ?? uniquePropertyValue[vehicleType]();
            vehicle.UniquePropertyString = filter.UniquePropertyString ?? uniquePropertyStrings[vehicleType];
            return vehicle;
        }


        public static IVehicle CreateRandomVehicle(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!uniquePropertyStrings.ContainsKey(type))
                throw new ArgumentException($"'{type.Name}' is not a registered vehicle type.", nameof(type));
   
            var filter = new Filter
            {
                VehicleType = type.Name,
                RegistryNumber = GenerateRegNumber(),
                Color = RandomHelper.Pick(Colors),
                FuelType = RandomHelper.Pick(FuelTypes),
                NumWheels = RandomHelper.Pick(WheelOptions),
                UniquePropertyValue = uniquePropertyValue[type](),
                UniquePropertyString = uniquePropertyStrings[type]
            };

            return CreateVehicle(filter);
        }

        public static string GenerateRegNumber()
        {
            string letters = new string(
                Enumerable.Range(0, 3)
                    .Select(_ => (char)Random.Shared.Next('A', 'Z' + 1))
                    .ToArray());

            string numbers = Random.Shared.Next(100, 999).ToString();
            return letters + numbers;
        }
    }
}