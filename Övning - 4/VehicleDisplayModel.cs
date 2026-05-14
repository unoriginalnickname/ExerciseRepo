namespace GaragePractice
{
    public class VehicleDisplayModel
    {
        public string VehicleType { get; set; }
        public string RegPlateNumber { get; set; }
        public string Color { get; set; }
        public string NumWheels { get; set; }
        public string Fueltype { get; set; }
        public string UniqueProperties { get; set; }

        public override string ToString()
        {
            return $"Type {VehicleType}, Reg: {RegPlateNumber}, Color: {Color}, # wheels: {NumWheels}, fueltype: {Fueltype}, unique: {UniqueProperties}";
        }
    }
}
