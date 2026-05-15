using Microsoft.VisualBasic.FileIO;
using static System.Net.WebRequestMethods;
using System.Linq;
using System.Security.Cryptography;

namespace GaragePractice
{
    public class Garage
    {
        
      
        Dictionary<string, string> approvedVehicleTypes = new Dictionary<string, string>()
 {
    { "Airplane",    "Wing span (m)"         },
    { "Boat",        "Hull length (m)"        },
    { "Bus",         "Number of stops"        },
    { "Car",         "Number of doors"        },
    { "Motorcycle",  "Engine size (cc)"       },
    { "Ufo",         "Abduction capacity"     },
    { "Uap",         "Classified"             },
 };

        //    private readonly string[,] vehicleTypes = new string[,]
        //    {
        //{ "Airplane",    "Wing span (m)"         },
        //{ "Boat",        "Hull length (m)"        },
        //{ "Bus",         "Number of stops"        },
        //{ "Car",         "Number of doors"        },
        //{ "Motorcycle",  "Engine size (cc)"       },
        //{ "Ufo",         "Abduction capacity"     },
        //{ "Uap",         "Classified"             },
        //    };

        public Dictionary<string, string> ApprovedVehicleTypes { get { return approvedVehicleTypes; } }

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

        public IVehicle[] GetIVehicleArray()
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

            var keys = ApprovedVehicleTypes.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                string vehicleType = keys[i];

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
            int length = ApprovedVehicleTypes.Keys.Count;
            var keys = ApprovedVehicleTypes.Keys.ToList();

            for (int i = 0; i < length; i++)
            {
                if (keys[i] == vehicleType)
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
