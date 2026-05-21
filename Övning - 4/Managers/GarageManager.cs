using Övning___4.Misc;
using Övning___4.ViewModel;

public partial class GarageManager
{
    private List<IGarage> garages = new();
  public OperationResult TryCreateGarage(string? garageVehicleType, int? garageSize, string? garageName)
{
    if (garageVehicleType == null) return OperationResult.Fail("Garage type name cannot be empty.");
    if (string.IsNullOrWhiteSpace(garageName)) return OperationResult.Fail("Garage name cannot be empty.");
    if (garageName.Length > 20) return OperationResult.Fail("Garage name too long, max 20 characters.");
    if (garages.Any(x => x.GarageName == garageName)) return OperationResult.Fail("Garage name already exists.");

    string? correctedTypeName = NormalizeWord(garageVehicleType);

    var type = Type.GetType(correctedTypeName);
    if (type == null)
        return OperationResult.Fail($"Unknown vehicle type '{correctedTypeName}'. Use 'listgaragetypes' to see approved types.");

    if (!typeof(IVehicle).IsAssignableFrom(type))
        return OperationResult.Fail($"'{correctedTypeName}' is not a valid vehicle type.");

    var actualGarageType = typeof(Garage<>).MakeGenericType(type);
    var garage = (IGarage)Activator.CreateInstance(actualGarageType, garageSize ?? 55, garageName);

    garages.Add(garage);
    return OperationResult.Ok("Garage was added. " + garage.ToString());
}

    //parking
    public OperationResult TryParkVehicle(Filter filter, string? garageName)
    {
        if (RegNumberExistsAnywhere(filter.RegistryNumber))
            return OperationResult.Fail("A vehicle with that registration number is already parked.");

        var vehicleType = Type.GetType(filter.VehicleType);
        if (vehicleType == null)
            return OperationResult.Fail($"Unknown vehicle type '{filter.VehicleType}'.");

        var garage = garages.FirstOrDefault(x =>
            x.TypeOfGarage == vehicleType
            && x.HasFreeSlots
            && (string.IsNullOrWhiteSpace(garageName) || x.GarageName == garageName));

        if (garage == null)
            return OperationResult.Fail("Could not find an available garage.");

        try
        {
            var vehicle = VehicleFactory.CreateVehicle(filter);
            garage.ParkVehicle(vehicle);
            return OperationResult.Ok($"Parked {vehicle.GetType().Name} in {garage.GarageName}");
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult.Fail(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    private static string ParkSuccessMessage(IGarage garage, IVehicle vehicle)
    {
        return $"Parked Vehicle in garage: {garage.GarageName}\n{Filter.Header()}\n{FilterFactory.ConvertVehicleToFilter(vehicle)}";
    }

    public OperationResult Unpark(string regNumber)
    {
        if (string.IsNullOrWhiteSpace(regNumber))
            return OperationResult.Fail("Registration number cannot be empty.");

        regNumber = regNumber.Trim().ToUpperInvariant();
        var garage = FindGarageContaining(regNumber);

        if (garage == null)
            return OperationResult.Fail($"No vehicle with reg number '{regNumber}' found.");

        try
        {
            garage.Unpark(regNumber);
            return OperationResult.Ok($"Unparked: {regNumber} from {garage.GarageName}");
        }
        catch (InvalidOperationException ex)
        {
            // Shouldn't happen since we checked above — but safe to catch
            return OperationResult.Fail(ex.Message);
        }
    }
}