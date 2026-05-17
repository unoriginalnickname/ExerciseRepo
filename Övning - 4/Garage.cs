using Övning___4;
using System.Collections;

namespace GaragePractice
{
    public class Garage<T> : IGarage, IEnumerable<T> where T : IVehicle

    {
        private List<T> internalGarage;
        private readonly int defaultSize = 15;
        private int maxSize;


        public int MaxSize { get { return maxSize; } internal set { maxSize = value; } }

        public Garage()
        {
            Console.WriteLine("Garage: creating garage with default size " + defaultSize);
            maxSize = defaultSize;
            internalGarage = new List<T>();
        }

        public Garage(int garageSize)
        {
            maxSize = garageSize;
            internalGarage = new List<T>();

        }
        public IEnumerator<T> GetEnumerator()
        {
            return internalGarage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ParkVehicle(IVehicle vehicle)
        {
           internalGarage.Add((T)vehicle);
        }

        public void Unpark(string registryNumber)
        {
            internalGarage.RemoveAll(v => v.RegistryNumber == registryNumber);
        }

        public IEnumerable<IVehicle> GetVehicles()
        {
            return internalGarage.Cast<IVehicle>();
        }
    }
}
