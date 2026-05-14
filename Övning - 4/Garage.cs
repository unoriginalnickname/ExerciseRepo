using Microsoft.VisualBasic.FileIO;
using static System.Net.WebRequestMethods;

namespace GaragePractice
{
    public class Garage
    {

        private readonly string[,] vehicleTypes = new string[,]
        {
    { "Airplane",    "Wing span (m)"         },
    { "Boat",        "Hull length (m)"        },
    { "Bus",         "Number of stops"        },
    { "Car",         "Number of doors"        },
    { "Motorcycle",  "Engine size (cc)"       },
    { "Ufo",         "Abduction capacity"     },
    { "Uap",         "Classified"             },
        };

        public string[,] ApprovedVehicleTypes { get { return vehicleTypes; } }

        private IVehicle[] vehicleGarageArray;
        private readonly int defaultSize = 15;

        public Garage()
        {
            Console.WriteLine("Garage: creating garage with default size " + defaultSize);
            vehicleGarageArray = new IVehicle[defaultSize];
        }

        public Garage(int garageSize)
        {
            vehicleGarageArray = new IVehicle[garageSize];
        }

        public void ParkVehicle(IVehicle vehicle)
        {
            if (FindFirstFreeParkingSlot() is int parkingSlot) //fancy syntax
                vehicleGarageArray[parkingSlot] = vehicle;
        }

        private int? FindFirstFreeParkingSlot()
        {
            for (int i = 0; i < vehicleGarageArray.Length; i++)
                if (vehicleGarageArray[i] == null)
                    return i;
            return null;
        }

        public bool UnParkVehicle(string? regPlateNumber)
        {
            for (int i = 0; i < vehicleGarageArray.Length; i++)
            {
                if (vehicleGarageArray[i] != null)
                    if (vehicleGarageArray[i].RegistryNumber == regPlateNumber) ;
                {
                    vehicleGarageArray[i] = null;
                    return true;
                }
            }
            return false;
        }

        public IVehicle[] GetVehicleArray()
        {
            return (IVehicle[])vehicleGarageArray.Clone();
        }

        public int GetNumberOfVehiclesInGarage()
        {
            return NumberOfVehiclesInArray(this.vehicleGarageArray);
        }
        private int NumberOfVehiclesInArray(IVehicle[] arr)
        {
            int numberOfVehicles = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null) numberOfVehicles++;
            }
            return numberOfVehicles;
        }

        public void AutoPopulateGarage()
        {
            Console.WriteLine("Garage: Autopopulating...");
            //garageSpace[0] = new Airplane("123", "green", "8", FuelType.ZeroPointModule, "nothing");

            for (int i = 0; i < ApprovedVehicleTypes.GetLength(0); i++)
            {
                string vehicleType = ApprovedVehicleTypes[i, 0];

                IVehicle vehicle = (IVehicle?)Activator.CreateInstance(Type.GetType("GaragePractice." + vehicleType));

                vehicle.RegistryNumber = "123-ABC";
                vehicle.Color = "Blue";
                vehicle.Fueltype = "Gas";
                vehicle.NumWheels = "2";
                vehicle.UniqueProperty = "test";
                vehicleGarageArray[i] = vehicle;
            }
        }

        public IVehicle[] GetGarageContents()
        {
            return vehicleGarageArray;
        }

        internal bool VehicleIsApprovedType(string vehicleType)
        {
            int length = ApprovedVehicleTypes.Length;

            for (int i = 0; i < length; i++)
            {
                if (ApprovedVehicleTypes[i, 0] == vehicleType)
                {
                    Console.WriteLine("Vehicletype is approved. ");
                    return true;
                }
            }
            Console.WriteLine("Vehicletype failed approval. ");
            return false;
        }
    }
}
