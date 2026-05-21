using Övning___4.Misc;

namespace Övning___4.ViewModel
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
        public static string Header()
        {
            return
                $"{"Type",-13}" +
                $"{"RegNumber",-10}" +
                $"{"Color",-8}" +
                $"{"Wheels",-6}" +
                $"{"Fuel",-10}" +
                $"{"Details"}";
        }
        internal IVehicle ToVehicle()
        {
           return VehicleFactory.CreateVehicle(this);
        }
    }
}