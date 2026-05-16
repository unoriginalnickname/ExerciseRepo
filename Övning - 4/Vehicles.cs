using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{

    public class Airplane : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get { return "Number of engines: ";  } set; }
    }
    public class Motorcycle : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get { return "Cylinder volume: "; } set; }
    }
    public class Car : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get { return "Number of seats: "; } set; }
    }

    public class Bus : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get; set; }
    }
    public class Boat : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get { return "Number of seats: ";  } set; }
    }
    public class Ufo : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get { return "Anti gravity engine type: "; } set; }
    }
    public class Uap : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get { return "Anti gravity engine type: "; } set; }
    }
}
