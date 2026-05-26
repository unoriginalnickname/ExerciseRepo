using Övning___4.Misc;
using Övning___4.ViewModel;

public partial class GarageManager
{
    private List<IGarage> garages = new();
    OperationResult ValidateGarageCreationInput(string? garageName, string? vehicleTypeString, int? size)
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

    private OperationResult TryGetVehicleType(string typeString, out Type? type)
    {
        type = null;
        string? normalizedString;
        var result = TryNormalizeWord(typeString, out normalizedString);
        if (!result.Success)
            return result;

        type = normalizedString switch
        {
            "Airplane" => typeof(Airplane),
            "Boat" => typeof(Boat),
            "Bus" => typeof(Bus),
            "Car" => typeof(Car),
            "Motorcycle" => typeof(Motorcycle),
            "Ufo" => typeof(Ufo),
            "Uap" => typeof(Uap),

            _ => null
        };
        return type is null ? OperationResult.Fail("Unknown vehicle type. Use 'listgaragetypes' to see approved types.")
            : OperationResult.Ok("Found vehicletype: " + type.Name);
    }

    OperationResult TryMakeGarageInstance(string vehicleType, int size, string name, out IGarage? garage)
    {
        garage = null;

        garage = vehicleType switch
        {
            "Airplane" => new Garage<Airplane>(size, name),
            "Boat" => new Garage<Boat>(size, name),
            "Bus" => new Garage<Bus>(size, name),
            "Car" => new Garage<Car>(size, name),
            "Motorcycle" => new Garage<Motorcycle>(size, name),
            "Ufo" => new Garage<Uap>(size, name),
            "Uap" => new Garage<Uap>(size, name),

            _ => null
        };
        return garage is null ? OperationResult.Fail("Garage is null")
            : OperationResult.Ok("Garage is ok. ");
    }

    public OperationResult TryAddNewGarage(string? vehicleTypeString,
        int? size, string? name)
    {

        var result = ValidateGarageCreationInput(name, vehicleTypeString, size);
        if (!result.Success)
            return result;

        string normalizedVehicleType;
        result = TryNormalizeWord(vehicleTypeString, out normalizedVehicleType);
        if (!result.Success)
            return result;

        IGarage? garage;
        result = TryMakeGarageInstance(normalizedVehicleType!, (int)size!, name!, out garage);
        if (!result.Success)
            return result;

        try
        {
            garages.Add(garage!);
            return OperationResult.Ok($"Garage {name} was added. ");
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    //parking
    public OperationResult TryParkVehicle(VehicleFilter filter, string? garageName)
    {
        var result = RegIsAvailable(filter.RegistryNumber);
        if (!result.Success)
            return result;

        Type? vehicleType;
        result = TryGetVehicleType(filter.VehicleType!, out vehicleType);
        if (!result.Success)
            return result;

        if(garageName != null)
        result = TryNormalizeWord(garageName, out garageName);
        if (!result.Success)
            return result;

        IGarage? garage;
        result = TryFindAvailableGarage(vehicleType, garageName, out garage);
        if (!result.Success)
            return result;

        try
        {
            IVehicle? vehicle;
            result = VehicleFactory.TryCreateVehicle(filter, out vehicle);
            if (!result.Success)
                return result;

            garage!.ParkVehicle(vehicle!);
            return OperationResult.Ok($"Parked {vehicle!.GetType().Name} in {garage.Name}");
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

    private OperationResult TryFindAvailableGarage(Type? vehicleType, string? name, out IGarage? garage)
    {
        garage = null;

        garage = garages.FirstOrDefault(x =>
     x.GarageVehicleType == vehicleType
     && x.HasFreeSlots
     && (string.IsNullOrWhiteSpace(name) || string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)));

        if (garage == null)
            return OperationResult.Fail("Could not find an available garage.");

        return OperationResult.Ok("An available garage was found. ");
    }

    public OperationResult TryUnpark(string regNumber)
    {
        if (string.IsNullOrWhiteSpace(regNumber))
            return OperationResult.Fail("Registration number cannot be empty.");

        regNumber = regNumber.Trim().ToUpperInvariant();

        IGarage? garage;
        var result = TryFindGarageContaining(regNumber, out garage);
        if (!result.Success)
            return result;

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