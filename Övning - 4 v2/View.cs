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

        public static void PrintVehicles(VehicleDisplayModel[] displayModelArray)
        {
            if(displayModelArray.Length == 0)
            {
                Console.WriteLine("No vehicles found. ");
            }
            Console.WriteLine("Amount found: " + displayModelArray.Length);
            //could format strings into columns

            for (int i = 0; i < displayModelArray.Length; i++)
            {
                Console.WriteLine(displayModelArray[i].ToString());
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
