using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{
    public class Garage //static is impossible because we must use parameterized constructor
    {
        private Vehicle[] garageSpace;
        private readonly int defaultSize = 15;

        public Garage()
        {
            Console.WriteLine("Garage: creating garage with default size " + defaultSize);
            garageSpace = new Vehicle[defaultSize];
        }

        public Garage(int garageSize)
        {
            garageSpace = new Vehicle[garageSize];
        }

        //fancy syntax
        public void ParkVehicle(Vehicle vehicle)
        {
            if (FindFirstFreeParkingSlot() is int parkingSlot)
                garageSpace[parkingSlot] = vehicle;
        }

        private int? FindFirstFreeParkingSlot()
        {
            for (int i = 0; i < garageSpace.Length; i++)
                if (garageSpace[i] == null)
                    return i;
            return null;
        }

        public bool UnParkVehicle(string? regPlateNumber)
        {
            for (int i = 0; i < garageSpace.Length; i++)
            {
                if (garageSpace[i] != null)
                {
                    if (garageSpace[i].RegistryNumber == regPlateNumber)
                    {
                        garageSpace[i] = null;
                        return true;
                    }
                }
            }
            return false;
        }
  
        public Vehicle[] GetAllVehiclesToArray() // need to convert all IEnumerable to Array
        {
            return (Vehicle[])garageSpace.Clone();
        }

        public void AutoPopulateGarage()
        {
            Console.WriteLine("Garage: Autopopulating...");
            //garageSpace[0] = new Airplane("123", "green", "8", FuelType.ZeroPointModule, "nothing");
            garageSpace[1] = new Boat("123-ABC", "blue", "2", "Gas", "nothing");
            garageSpace[2] = new Car("456-DEF", "yellow", "4", "Diesel", "nothing");
            garageSpace[3] = new Airplane("789-GHI", "black", "4", "Anti-gravity", "nothing");
            garageSpace[4] = new Car("ABC-393", "black", "4", "Gasoline", "nothing");
            garageSpace[5] = new Bus("ABC-999", "black", "4", "Gasoline", "nothing");
            garageSpace[6] = new Motorcycle("ABC-234", "pink", "3", "Gasoline", "nothing");
            garageSpace[7] = new Motorcycle("qqq-728", "pink", "3", "ZeroPointEnergy", "nothing");
            garageSpace[8] = new Motorcycle("ZZZ-562", "pink", "3", "Diesel", "nothing");
            garageSpace[9] = new Motorcycle("ZZZ-123", "black", "3", "Diesel", "nothing");
        }
        //○ Alla svarta fordon med fyra hjul.
        //○ Alla motorcyklar som är rosa och har 3 hjul.
        //○ Alla lastbilar
        //○ Alla röda fordon

        public Vehicle[] GetGarageContents()
        {
            return garageSpace;
        }
    }
}
