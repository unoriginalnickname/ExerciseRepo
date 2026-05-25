using Övning___4.Misc;
using Övning___4.ViewModel;
using System.Xml.Linq;

public partial class GarageManager
{
    private List<IGarage> garages = new();
    private static readonly Dictionary<string, Type> VehicleTypes; // need to add this to avoid reflection.

    OperationResult ValidateGarageCreation(string? garageName, string? vehicleTypeString, int? size)
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
 
        var result = ValidateGarageCreation(name, vehicleTypeString, size);
        if (!result.Success)
            return result;
       
        Type? vehicleType;
        result = TryResolveVehicleType(vehicleTypeString!, out vehicleType);
        if (!result.Success)
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
        var result = RegIsAvailable(filter.RegistryNumber);
        if (!result.Success)
            return result;
        
        Type? vehicleType;
        result = TryResolveVehicleType(filter.VehicleType, out vehicleType);
        if (!result.Success)
            return result;

        IGarage garage;
        result = FindAvailableGarage(vehicleType, name, out garage);
        if (!result.Success)
            return result;

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

    private OperationResult FindAvailableGarage(Type? vehicleType, string? name, out IGarage garage)
    {
        garage = garages.FirstOrDefault(x =>
     x.GarageVehicleType == vehicleType
     && x.HasFreeSlots
     && (string.IsNullOrWhiteSpace(name) || x.Name == name));

        if (garage == null)
            return OperationResult.Fail("Could not find an available garage.");

        return OperationResult.Ok("An available garage was found. ");
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