using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{

    public static class View
    {

        //Från gränssnittet skall det gå att:
        //● Navigera till samtlig funktionalitet från garage via gränssnittet
        //● Skapa ett garage med en användar-specificerad storlek
        //● Det skall gå att stänga av applikationen från gränssnittet

        public static void PrintString(string s)
        {
            Console.WriteLine(s);
        }
        public static void PrintVehicles(IEnumerable<VehicleDisplayModel> enumerable)
        {
            if(enumerable.Count() < 1)
            {
                Console.WriteLine("Garage appears empty. ");
            }
            Console.WriteLine("Amount found: " + enumerable.Count());
            //could format strings into columns
            foreach (VehicleDisplayModel item in enumerable)
            {
                Console.WriteLine(item.ToString());
            }
        }
        internal static void PrintVehicle(VehicleDisplayModel vehicleDisplayModel)
        {
            throw new NotImplementedException();
        }

        internal static string GetInput()
        {
            return Console.ReadLine();
        }

        static void LaunchMenu()
        {
            Console.WriteLine("Launch Menu GarageSimulator");
            Console.WriteLine("Enter garage size or just press enter for default size (15 slots)");

        }

        static void MainMenu()
        {
            Console.Clear();
            //   Console.WriteLine($"Garage Stats - Size: {garage.TotalSlots}, Free slots: {garage.TotalFreeSlots})") ;
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("\nMake new garage");
        }
        public static void MakeGarage(int garageSize)
        {

        }
        public static void FindVehicleWithRegNumber(string vehicle)
        {

        }

        public static void ListSpecificVehicleType()
        {

        }
        public static void Terminate()
        {

        }

        internal static void Clear()
        {
            Console.Clear();
        }

        internal static void PrintStats()
        {
            throw new NotImplementedException();
        }
    }
}
