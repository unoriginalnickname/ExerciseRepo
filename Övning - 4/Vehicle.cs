using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{
    public abstract class Vehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }

        public string Fueltype { get; set; }
        public string Other { get; set; }

        public Vehicle(string regPlate = "", string color = "", string numWheels = "", string fuel = "", string other = "")
        {
            RegistryNumber = regPlate;
            Color = color;
            NumWheels = numWheels;
            Fueltype = fuel;
            Other = other;
        }
        public abstract void SetUniqueProperty(string property);
        public abstract string GetUniquePropertyString();

        public abstract string GetUniqueProperty();

        //public static bool operator ==(Vehicle a, string b)
        //{
        //    return a.Equals(b);
        //}

        //public static bool operator !=(Vehicle a, string b)
        //{
        //    return !a.Equals(b);
        //}
    }
}
