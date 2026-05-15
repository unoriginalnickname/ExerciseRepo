using Microsoft.VisualBasic.FileIO;

namespace GaragePractice
{
    public struct Filter
    {
        public string? VehicleType {  get; set { if(value != null) field = char.ToUpper(value[0]) + value.Substring(1).ToLower(); } }
        public string? FuelType { get; set; }
        public string? Color { get; set; }
        public string? RegNumber { get; set { if (value != null) field = value.ToUpper(); } }
        public string? NumWheels { get; set; }
        public string? UniqueProperty { get; set; }
        public override string ToString()
        {
            return $"Type {VehicleType}, Reg: {RegNumber}, Color: {Color}, # wheels: {NumWheels}, fueltype: {FuelType}, unique: {UniqueProperty}";
        }
    }
}