using Övning___4;
using System.Collections;

namespace GaragePractice
{
    public class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        private List<T> vehicleGarage;
        private readonly int defaultSize = 15;
        private int maxSize;

        readonly Dictionary<string, string> approvedVehicleTypes = new()
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

        public int MaxSize { get { return maxSize; } internal set { maxSize = value; } }
        public Type GetGarageVehicleType()
        {
            return typeof(T);
        }
        public Garage()
        {
            Console.WriteLine("Garage: creating garage with default size " + defaultSize);
            maxSize = defaultSize;
            vehicleGarage = new List<T>();
        }

        public Garage(int garageSize)
        {
            maxSize = garageSize;
            vehicleGarage = new List<T>();

        }

        public void ParkVehicle(T item)
        {
            vehicleGarage.Add(item);
        }

        public void Unpark(T vehicle)
        {
            vehicleGarage.Remove(vehicle);
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
