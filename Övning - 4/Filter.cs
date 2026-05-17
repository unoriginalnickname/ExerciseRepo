using Microsoft.VisualBasic.FileIO;

namespace GaragePractice
{
    public class Filter
    {
        public string? VehicleType { get; set; }
        public string? FuelType { get; set; }
        public string? Color { get; set; }
        public string? RegistryNumber { get; set; }
        public int? NumWheels { get; set; }
        public string? UniquePropertyString { get; set; }
        public string? UniquePropertyValue { get; set; }
        public override string ToString()
        {
            return
                $"{VehicleType,-13}" +
                $"{RegistryNumber,-10}" +
                $"{Color,-8}" +
                $"{NumWheels,-6}" +
                $"{FuelType,-10}" +
                $"{UniquePropertyString}: {UniquePropertyValue}";
        }
    }
}