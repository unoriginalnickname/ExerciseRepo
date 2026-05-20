using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{
    interface IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string Other { get; set; }
        public abstract void SetUniqueProperty(string property);
        public abstract string GetUniquePropertyString();
        public abstract string GetUniqueProperty();
    }
}
