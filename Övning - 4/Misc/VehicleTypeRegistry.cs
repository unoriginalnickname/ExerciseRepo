namespace Övning___4.Misc
{
    public static class VehicleTypeRegistry
    {
        static readonly Dictionary<string, string> 
            approvedVehicleTypes = new(){
                                         { "Airplane", "Wing span (m)" },
                                         { "Boat", "Hull length (m)" },
                                         { "Bus", "Number of stops" },
                                         { "Car", "Number of doors" },
                                         { "Motorcycle", "Engine size (cc)" },
                                         { "Ufo", "Abduction capacity" },
                                         { "Uap", "Classified" } };

        public static readonly List<Type> 
            GarageTypeList = new() {
                                    typeof(Airplane),
                                    typeof(Boat),
                                    typeof(Bus),
                                    typeof(Car),
                                    typeof(Motorcycle),
                                    typeof(Uap),
                                    typeof(Ufo) };

        public static readonly Dictionary<string, Type> 
            GarageTypeDictionary = new() {
                                         { "Airplane", typeof(Airplane) },
                                         { "Boat", typeof(Boat) },
                                         { "Bus", typeof(Bus) },
                                         { "Car", typeof(Car) },
                                         { "Motorcycle", typeof(Motorcycle) },
                                         { "Ufo", typeof(Uap) },
                                         { "Uap", typeof(Ufo) }
        };


        public static Dictionary<string, string> ApprovedVehicleTypes { get { return approvedVehicleTypes; } }

    }
}
