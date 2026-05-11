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

        public override string GetUniqueProperty()
        {
            return "Number of engines: " + NumEngines;
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
        public override string GetUniqueProperty()
        {
            return "Cylinder volume: " + CylinderVolume;
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
        public override string GetUniqueProperty()
        {
            return "Number of seats: " + NumberOfSeats;
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
        public override string GetUniqueProperty()
        {
            return "Number of seats: " + NumberOfSeats;
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
        public override string GetUniqueProperty()
        {
            return "Number of seats: " + NumberOfSeats;
        }
        public int NumberOfSeats { get; set; }
    }
}
