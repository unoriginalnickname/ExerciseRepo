using Microsoft.VisualBasic.FileIO;

namespace GaragePractice
{
    public struct FilterX
    {
        public bool IsValidForPark()
        {
            return
                !string.IsNullOrWhiteSpace(RegNumber) &&
                RegNumber.All(char.IsLetterOrDigit) &&
                !string.IsNullOrWhiteSpace(VehicleType) &&
                VehicleType.All(char.IsLetter) &&
                NumWheels is >= 0 and <= 18;
        }
        public string? VehicleType { get; set; }
        public string? FuelType { get; set; }
        public string? Color { get; set; }
        public string? RegNumber { get; set; }
        public int? NumWheels { get; set; }
        public string? UniquePropertyString { get; set; }
        public string? UniquePropertyValue { get; set; }
        public override string ToString()
        {
            return
                $"{VehicleType,-13}" +
                $"{RegNumber,-10}" +
                $"{Color,-8}" +
                $"{NumWheels,-6}" +
                $"{FuelType,-10}" +
                $"{UniquePropertyString}: {UniquePropertyValue}";
        }
    }
}