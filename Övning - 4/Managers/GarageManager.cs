using Övning___4.Misc;
using Övning___4.ViewModel;

public partial class GarageManager
{
    private List<IGarage> garages = new();
    internal OperationResult CreateGarage(string? garageTypeName, int? garageSize, string? garageName)
    {
        if (garageTypeName == null) { return OperationResult.Fail("Garage type name cannot be empty.");  }
        if (garageName.Length > 20)
            return OperationResult.Fail("Garage name too long, max 20 characters. ");

        string? correctedTypeName = NormalizeWord(garageTypeName);
        if (garages.Any(x => x.GarageName == correctedTypeName)) { return OperationResult.Fail("Garage name already exists"); }

        var type = Type.GetType(correctedTypeName);
        if (type == null)
            return OperationResult.Fail($"Unknown vehicle type '{correctedTypeName}'. Use 'listgaragetypes' to see approved types.");
        

        if (!typeof(IVehicle).IsAssignableFrom(type))
            return OperationResult.Fail($"'{correctedTypeName}' is not a valid vehicle type.");

        var actualGarageType = typeof(Garage<>).MakeGenericType(type);
        var garage = (IGarage)Activator.CreateInstance(actualGarageType, garageSize ?? 55, garageName ?? "Default Garage");

        garages.Add(garage);
        return OperationResult.Ok("Garage was added. " + garage.ToString());
    }

    //parking
    public OperationResult TryParkVehicle(Filter filter, string? garageName)
    {
        if (garages.Any((x => x.ContainsVehicleRegNumber(filter.RegistryNumber.ToUpperInvariant()))))
            return OperationResult.Fail("Garage already contains vehicle registration number");

        var vehicleType = Type.GetType(filter.VehicleType);
        if (vehicleType == null)
            return OperationResult.Fail($"Unknown vehicle type '{filter.VehicleType}'.");

        var garage = garages.FirstOrDefault(x =>
            x.TypeOfGarage == vehicleType
            && x.HasFreeSlots
            && (string.IsNullOrWhiteSpace(garageName) || x.GarageName == garageName));

        if (garage == null)
            return OperationResult.Fail("Could not find an available garage.");

        var vehicle = VehicleFactory.CreateVehicle(filter);
        garage.ParkVehicle(vehicle);
        return OperationResult.Ok($"Parked {vehicle.GetType().Name} in {garage.GarageName}");
    }

    private static string ParkSuccessMessage(IGarage garage, IVehicle vehicle)
    {
        return $"Parked Vehicle in garage: {garage.GarageName}\n{Filter.Header()}\n{FilterFactory.ConvertVehicleToFilter(vehicle)}";
    }

    public OperationResult Unpark(string regNumber)
    {
        if (string.IsNullOrWhiteSpace(regNumber))
        {
            return OperationResult.Fail("Registration number cannot be empty.");
        }

        regNumber = regNumber.Trim().ToUpperInvariant(); // normalize
        var garage = FindGarageContaining(regNumber);

        if (garage == null)
            return OperationResult.Fail($"No vehicle with reg number '{regNumber}' found.");

        garage.Unpark(regNumber);
        return OperationResult.Ok($"Unparked: {regNumber} from {garage.GarageName}");
    }
}