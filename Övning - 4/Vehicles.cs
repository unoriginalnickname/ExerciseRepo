using System;
using System.Collections.Generic;
using System.Text;

namespace GaragePractice
{

    public class Airplane : Vehicle
    {
        public Airplane()
        {

        }
        public Airplane(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public override void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumEngines = value;
        }
        public override string GetUniquePropertyString()
        {
            return "Number of engines: ";
        }
        public override string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumEngines;
        }

        public int NumEngines { get; set; }
    }
    public class Motorcycle : Vehicle
    {
        public Motorcycle()
        {

        }
        public Motorcycle(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public override void SetUniqueProperty(string prop)
        {
            int volume;
            int.TryParse(prop, out volume);
            CylinderVolume = volume;
        }
        public override string GetUniquePropertyString()
        {
            return "Cylinder volume: ";
        }
        public override string GetUniqueProperty()
        {
            return GetUniquePropertyString() + CylinderVolume;
        }
        public int CylinderVolume { get; set; }
    }
    public class Car : Vehicle
    {
        public Car()
        {

        }
        public Car(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public override string GetUniquePropertyString()
        {
            return "Number of seats: ";
        }
        public override string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumberOfSeats;
        }
        public override void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumberOfSeats = value;
        }
        public int NumberOfSeats { get; set; }

    }

    public class Bus : Vehicle
    {
        public Bus() 
        {
        }
        public Bus(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public override void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumberOfSeats = value;
        }
        public override string GetUniquePropertyString()
        {
            return "Number of seats: ";
        }
        public override string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumberOfSeats;
        }
        public int NumberOfSeats { get; set; }
    }
    public class Boat : Vehicle
    {
        public Boat()
        {

        }
        public Boat(string regPlate, string color, string numWheels, string fuel, string other) : base(regPlate, color, numWheels, fuel, other)
        {
        }
        public override void SetUniqueProperty(string prop)
        {
            int value;
            int.TryParse(prop, out value);
            NumberOfSeats = value;
        }
        public override string GetUniqueProperty()
        {
            return GetUniquePropertyString() + NumberOfSeats;
        }
        public override string GetUniquePropertyString()
        {
            return "Number of seats: ";
        }
        public int NumberOfSeats { get; set; }
    }
}
