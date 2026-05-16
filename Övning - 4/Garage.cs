using Övning___4;
using System.Collections;

namespace GaragePractice
{
    public class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        private List<T> vehicleGarage;
        private int maxSize;
        private bool allowMultipleTypes;

        Dictionary<string, string> approvedVehicleTypes = new Dictionary<string, string>()
 {
    { "Airplane", "Wing span (m)" },
    { "Boat", "Hull length (m)" },
    { "Bus", "Number of stops" },
    { "Car", "Number of doors" },
    { "Motorcycle", "Engine size (cc)" },
    { "Ufo", "Abduction capacity" },
    { "Uap", "Classified" },
 };

        public Dictionary<string, string> ApprovedVehicleTypes { get { return approvedVehicleTypes; } }

        private readonly int defaultSize = 15;

        public Garage()
        {
            Console.WriteLine("Garage: creating garage with default size " + defaultSize);
            maxSize = defaultSize;
            vehicleGarage = new List<T>();
        }

        public Garage(int garageSize, bool allowMultipleTypes = true)
        {
            maxSize = garageSize;
            vehicleGarage = new List<T>();
            this.allowMultipleTypes = allowMultipleTypes;
        }

        public void ParkVehicle(T item)
        {
            if (vehicleGarage.Count < maxSize)
            {
                vehicleGarage.Add(item);
            }
            else
                Console.WriteLine("The garage is full. Cannot park vehicle with registry number " + item.RegistryNumber + ". ");
        }

        public bool UnParkVehicle(string? regPlateNumber)
        {
            if (vehicleGarage.Where(v => v.RegistryNumber == regPlateNumber).FirstOrDefault() is T vehicleToRemove)
            {
                vehicleGarage.Remove(vehicleToRemove);
                Console.WriteLine("Garage: Unparking successful. Vehicle with registry number " + regPlateNumber + " removed from garage. ");
                return true;
            }
            else
            {
                Console.WriteLine("Garage: Unparking failed. No registry number provided. ");
                return false;
            }
        }

        public IList<T> GetGarageToIList()
        {
            return vehicleGarage;
        }

        public int GetNumberOfVehiclesInGarage()
        {
            return vehicleGarage.Count;
        }

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

  
        public void AutoPopulateGarage()
        {
            Console.WriteLine("Garage: Autopopulating...");

            List<string> colors = new() { "Red", "Blue", "Black", "White", "Green", "Orange", "Magenta", "Chrome" };
            List<string> gasTypes = new() { "Petrol", "Diesel", "Electric", "Hybrid" };
            List<int> wheelCounts = new() { 2, 4, 6, 8 };

            var allowedTypes = typeof(T) == typeof(IVehicle)
                ? approvedVehicleTypes.Keys.Select(k => Type.GetType("GaragePractice." + k)!).ToList()
                : new List<Type> { typeof(T) };

            int freeSlots = 1;
            for (int i = 0; i < maxSize - freeSlots; i++)
            {
                Type vehicleType = allowedTypes[i % allowedTypes.Count];
                T vehicle = (T)Activator.CreateInstance(vehicleType)!;

                string reg;
                do
                {
                    reg = GenerateRegNumber();
                } while (vehicleGarage.Any(v => v.RegistryNumber == reg));

                vehicle.RegistryNumber = reg;
                vehicle.Color = RandomHelper.Pick(colors);
                vehicle.Fueltype = RandomHelper.Pick(gasTypes);
                vehicle.NumWheels = RandomHelper.Pick(wheelCounts);
                vehicle.UniquePropertyString = approvedVehicleTypes[vehicleType.Name];
                vehicle.UniquePropertyValue = uniquePropertyValueGenerators[vehicleType]();

                vehicleGarage.Add(vehicle);
            }
        }


        Dictionary<Type, Func<string>> uniquePropertyValueGenerators = new()
{
    { typeof(Airplane),   () => $"{Random.Shared.Next(10, 80)} m" },
    { typeof(Boat),       () => $"{Random.Shared.Next(5, 200)} m" },
    { typeof(Bus),        () => $"{Random.Shared.Next(10, 200)} stops" },
    { typeof(Car),        () => $"{Random.Shared.Next(2, 6)} doors" },
    { typeof(Motorcycle), () => $"{Random.Shared.Next(50, 1500)} cc" },
    { typeof(Ufo),        () => $"{Random.Shared.Next(1, 1000)} abductees" },
    { typeof(Uap),        () => "Classified" }
};

        internal bool VehicleIsApprovedType(string vehicleType)
        {
        return approvedVehicleTypes.ContainsKey(vehicleType);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return vehicleGarage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
