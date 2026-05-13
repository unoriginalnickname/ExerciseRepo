using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{

    public class Airplane : IVehicle
    {
        public Airplane()
        {

        }
        public Airplane(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumEngines = value;
        }
        public string GetUniquePropertyString()
        {
            return "Number of engines: ";
        }
        public string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumEngines;
        }

        public int NumEngines { get; set; }
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string Other { get; set; }
    }
    public class Motorcycle : IVehicle
    {

        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string Other { get; set; }
        public int CylinderVolume { get; set; }
        public Motorcycle()
        {

        }
        public Motorcycle(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public  void SetUniqueProperty(string prop)
        {
            int volume;
            int.TryParse(prop, out volume);
            CylinderVolume = volume;
        }
        public  string GetUniquePropertyString()
        {
            return "Cylinder volume: ";
        }
        public  string GetUniqueProperty()
        {
            return GetUniquePropertyString() + CylinderVolume;
        }
    
    }
    public class Car : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string Other { get; set; }
        public int CylinderVolume { get; set; }
        public Car()
        {

        }
        public Car(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public  string GetUniquePropertyString()
        {
            return "Number of seats: ";
        }
        public  string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumberOfSeats;
        }
        public  void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumberOfSeats = value;
        }
        public int NumberOfSeats { get; set; }

    }

    public class Bus : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string Other { get; set; }
        public int NumberOfSeats { get; set; }
        public Bus() 
        {
        }
        public Bus(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public  void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumberOfSeats = value;
        }
        public  string GetUniquePropertyString()
        {
            return "Number of seats: ";
        }
        public  string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumberOfSeats;
        }
  
    }
    public class Boat : IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string Other { get; set; }
        public Boat()
        {

        }
        public Boat(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumberOfSeats = value;
        }
        public string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumberOfSeats;
        }
        public string GetUniquePropertyString()
        {
            return "Number of seats: ";
        }
        public int NumberOfSeats { get; set; }
    }
}
