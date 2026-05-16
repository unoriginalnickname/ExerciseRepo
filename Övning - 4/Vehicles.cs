using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{

    public class Airplane : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
    public class Motorcycle : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
    public class Car : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }

    public class Bus : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
    public class Boat : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
    public class Ufo : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
    public class Uap : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
}
