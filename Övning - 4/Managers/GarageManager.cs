using Övning___4.Misc;
using Övning___4.ViewModel;

public partial class GarageManager
{
    private List<IGarage> garages = new();

    OperationResult ValidateGarageCreation(string garageName, string vehicleTypeString, int? size)
    {
        if (string.IsNullOrWhiteSpace(garageName))
            return OperationResult.Fail("Garage name cannot be empty.");
        if (string.IsNullOrWhiteSpace(vehicleTypeString))
            return OperationResult.Fail("Garage vehicle type cannot be empty.");
        if ((size == null))
            return OperationResult.Fail("Garage garage size cannot be empty.");
        if ((size == 0))
            return OperationResult.Fail("Garage garage size cannot be zero.");
        if (garageName.Length > 20)
            return OperationResult.Fail("Garage name too long, max 20 characters.");
        if (garages.Any(x => x.Name == garageName))
            return OperationResult.Fail("Garage name already exists.");

        return OperationResult.Ok("Garage inputs are ok. ");
    }

    OperationResult TryResolveVehicleType(string typeString, out Type? type)
    {
        type = Type.GetType(NormalizeWord(typeString));
        if (type == null)
            return OperationResult.Fail($"Unknown vehicle type. " +
                $"Use 'listgaragetypes' to see approved types.");

        if (!typeof(IVehicle).IsAssignableFrom(type))
            return OperationResult.Fail($"Given type is not a valid vehicle type.");
        return OperationResult.Ok("Type is ok.");
    }

    IGarage MakeGarageInstance(Type type, int? size, string name)
    {
        Type garageType = typeof(Garage<>).MakeGenericType(type!);
        return (IGarage)Activator.CreateInstance(garageType, size, name);
    }

    public OperationResult TryCreateGarage(string? vehicleTypeString,
        int? size, string? name)
    {
        if (ValidateGarageCreation(name!, vehicleTypeString!, size) is
            { Success: false } inputResult)
            return inputResult;

        Type? vehicleType;

        if (TryResolveVehicleType(vehicleTypeString!, out vehicleType) is { Success: false } result)
            return result;
        try
        {
            garages.Add(MakeGarageInstance(vehicleType!, size, name!));
            return OperationResult.Ok($"Garage {name}was added. ");
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    //parking
    public OperationResult TryParkVehicle(Filter filter, string? name)
    {
        if (RegIsAvailable(filter.RegistryNumber) is { Success: false } regAvailabilityResult)
            return regAvailabilityResult;
        
        Type? vehicleType;
        if (TryResolveVehicleType(filter.VehicleType, out vehicleType) is { Success: false } typeCheckResult)
            return typeCheckResult;

        var garage = garages.FirstOrDefault(x =>
            x.GarageVehicleType == vehicleType
            && x.HasFreeSlots
            && (string.IsNullOrWhiteSpace(name) || x.Name == name));

        if (garage == null)
            return OperationResult.Fail("Could not find an available garage.");

        try
        {
            var vehicle = VehicleFactory.CreateVehicle(filter);
            garage.ParkVehicle(vehicle);
            return OperationResult.Ok($"Parked {vehicle.GetType().Name} in {garage.Name}");
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
        return $"Parked Vehicle in garage: {garage.Name}\n{Filter.Header()}\n{FilterFactory.ConvertVehicleToFilter(vehicle)}";
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
            return OperationResult.Ok($"Unparked: {regNumber} from {garage.Name}");
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
   
}