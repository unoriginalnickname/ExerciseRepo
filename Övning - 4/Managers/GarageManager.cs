using Övning___4.Misc;
using Övning___4.ViewModel;

public partial class GarageManager
{
    private List<IGarage> garages = new();

    OperationResult CheckGarageInputs(string garageName, string garageTypeString, int? garageSize)
    {
        if (string.IsNullOrWhiteSpace(garageName))
            return OperationResult.Fail("Garage name cannot be empty.");
        if (string.IsNullOrWhiteSpace(garageTypeString))
            return OperationResult.Fail("Garage vehicle type cannot be empty.");
        if ((garageSize == null))
            return OperationResult.Fail("Garage garage size cannot be empty.");
        if ((garageSize == 0))
            return OperationResult.Fail("Garage garage size cannot be zero.");
        if (garageName.Length > 20)
            return OperationResult.Fail("Garage name too long, max 20 characters.");
        if (garages.Any(x => x.GarageName == garageName))
            return OperationResult.Fail("Garage name already exists.");

        return OperationResult.Ok("Garage inputs are ok. ");
    }

    OperationResult CheckTypeValidity(Type? type)
    {
        if (type == null)
            return OperationResult.Fail($"Unknown vehicle type. " +
                $"Use 'listgaragetypes' to see approved types.");

        if (!typeof(IVehicle).IsAssignableFrom(type))
            return OperationResult.Fail($"Given type is not a valid vehicle type.");
        return OperationResult.Ok("Type is ok.");
    }

    IGarage MakeGarageInstance(Type type, int size, string name)
    {
        Type garageType = typeof(Garage<>).MakeGenericType(type!);
        return (IGarage)Activator.CreateInstance(garageType, size, name);
    }

    public OperationResult TryCreateGarage(string? garageTypeString,
        int garageSize, string? garageName)
    {
        if (CheckGarageInputs(garageName, garageTypeString, garageSize) is
            { Success: false } inputResult)
            return inputResult;

        Type? garageType = Type.GetType(NormalizeWord(garageTypeString));

        if (CheckTypeValidity(garageType) is { Success: false } result)
            return result;

        garages.Add(MakeGarageInstance(garageType, garageSize, garageName));
        return OperationResult.Ok($"Garage {garageName}was added. ");
    }

    //parking
    public OperationResult TryParkVehicle(Filter filter, string? garageName)
    {
        if (RegNumberExistsAnywhere(filter.RegistryNumber) is { Success: true } regCheckResult)
            return regCheckResult;
        
        var vehicleType = Type.GetType(filter.VehicleType);
        if (CheckTypeValidity(vehicleType) is { Success: true } typeCheckResult)
            return typeCheckResult;

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
            return OperationResult.Fail(ex.Message);
        }
    }
}