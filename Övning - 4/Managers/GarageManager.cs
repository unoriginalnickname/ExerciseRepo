using Övning___4.Misc;
using Övning___4.View;
using Övning___4.ViewModel;

public partial class GarageManager
{
    private List<IGarage> garages = new();
    internal void CreateGarage(string? garageTypeName, int? garageSize, string? garageName)
    {
        if (garageTypeName == null) { View.PrintString("Garage type name cannot be empty."); return; }

        string? correctedTypeName = NormalizeWord(garageTypeName);

        var type = Type.GetType(correctedTypeName);
        if (type == null)
        {
            View.PrintString($"Unknown vehicle type '{correctedTypeName}'. Use 'listgaragetypes' to see approved types.");
            return;
        }

        if (!typeof(IVehicle).IsAssignableFrom(type))
        {
            View.PrintString($"'{correctedTypeName}' is not a valid vehicle type.");
            return;
        }

        var actualGarageType = typeof(Garage<>).MakeGenericType(type);
        var garage = (IGarage)Activator.CreateInstance(actualGarageType, garageSize ?? 55, garageName ?? "Default Garage");

        garages.Add(garage);
        View.PrintString("Garage was added. " + garage.ToString());
    }

    //parking
    public void TryParkVehicle(Filter filter, string? garageName)
    {
        var vehicleType = Type.GetType(filter.VehicleType);
        if (vehicleType == null)
        {
            View.PrintString($"Unknown vehicle type '{filter.VehicleType}'. Use 'listgaragetypes' to see approved types.");
            return;
        }

        var garage = garages.FirstOrDefault(x =>
            x.TypeOfGarage == vehicleType
            && x.HasFreeSlots
            && (garageName == null || x.GarageName == garageName));

        if (garage != null)
        {
            var vehicle = VehicleFactory.CreateVehicle(filter);
            garage.ParkVehicle(vehicle);
            ParkSuccessMessage(garage, vehicle);
        }
        else
            View.PrintString("Could not find an available garage.");
    }

    private static void ParkSuccessMessage(IGarage garage, IVehicle vehicle)
    {
        View.PrintString($"Parked Vehicle in garage: {garage.GarageName}");
        View.PrintString(Filter.Header());
        View.PrintString($"{FilterFactory.ConvertVehicleToFilter(vehicle)} ");
    }

    public void Unpark(string regNumber)
    {
        var garage = garages.FirstOrDefault(x => x.GetVehicles().Any(v => v.RegistryNumber == regNumber));

        if (garage == null)
        {
            View.PrintString($"No vehicle with reg number '{regNumber}' found.");
            return;
        }

        garage.Unpark(regNumber);
        View.PrintString($"Unparked: {regNumber} from {garage.GarageName}");
    }
}