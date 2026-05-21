namespace Övning___4.Misc
{
    static class VehicleTypeRegistry
    {
       static readonly Dictionary<string, string> approvedVehicleTypes = new()
 {
    { "Airplane", "Wing span (m)" },
    { "Boat", "Hull length (m)" },
    { "Bus", "Number of stops" },
    { "Car", "Number of doors" },
    { "Motorcycle", "Engine size (cc)" },
    { "Ufo", "Abduction capacity" },
    { "Uap", "Classified" },
 };

        public static readonly List<Type> AllGarageTypes = new() {
        typeof(Airplane),
        typeof(Boat),
        typeof(Bus),
        typeof(Car),
        typeof(Motorcycle),
        typeof(Uap),
        typeof(Ufo) };

        public static Dictionary<string, string> ApprovedVehicleTypes { get { return approvedVehicleTypes; } }

    }
}
