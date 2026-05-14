namespace GaragePractice
{
    public interface IVehicle
    {
        public string RegistryNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperty { get; set; }
        public string UniquePropertyString { get; set; }
    }
}