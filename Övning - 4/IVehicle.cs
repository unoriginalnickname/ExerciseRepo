namespace GaragePractice
{
    public interface IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public int NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniquePropertyValue { get; set; }
        public string UniquePropertyString { get; set; }
    }
}