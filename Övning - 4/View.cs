using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{

    public static class View
    {

        public static void PrintString(string s)
        {
            Console.WriteLine(s);
        }

        public static void PrintVehicles(IEnumerable<VehicleDisplayModel> enumerable)
        {
            if(enumerable.Count() == 0)
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

        internal static string GetInput()
        {
            return Console.ReadLine();
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
