using GaragePractice;
using Övning___4;
using System.Diagnostics.Metrics;
using System.Text;

public class GarageManager
{


    private Dictionary<Type, IGarage> garages = new();

    Garage<IVehicle> backupGarage = new Garage<IVehicle>();


    public GarageManager(int garageSize = 15)
    {
        garages.Add(typeof(Car), new Garage<Car>(garageSize));
        garages.Add(typeof(Airplane), new Garage<Airplane>(garageSize));
        garages.Add(typeof(Boat), new Garage<Boat>(garageSize));
        garages.Add(typeof(Bus), new Garage<Bus>(garageSize));
        garages.Add(typeof(Motorcycle), new Garage<Motorcycle>(garageSize));
        garages.Add(typeof(Ufo), new Garage<Ufo>(garageSize));
        garages.Add(typeof(Uap), new Garage<Uap>(garageSize));
    }

    private bool IsGarageFull => backupGarage.Count() >= backupGarage.MaxSize;

    public bool IsRegNumberDuplicate(string regNumber) => backupGarage.Any(v => v.RegistryNumber == regNumber);

    public void ParkVehicle(Filter filter)
    {
        var vehicle = VehicleFactory.CreateVehicle(filter);

        if (garages.TryGetValue(Type.GetType(filter.VehicleType), out var garage))
        {
            garage.ParkVehicle(vehicle);
        }
        else
        {
            backupGarage.ParkVehicle(vehicle);
        }
    }

    public void ParkRandom()
    {
        foreach (var garage in garages)
        {
          garage.Value.ParkVehicle(VehicleFactory.CreateRandomVehicle(garage.Key));
        }
    }


    public void Unpark(string regNumber)
    {
        backupGarage.Unpark(regNumber);
        View.PrintString("Unparked: ");
    }

    public void UnparkRandomVehicle()
    {
        if (backupGarage.Count() == 0) { View.PrintString("Garage empty."); return; }
        var vehicle = RandomHelper.Pick(backupGarage);
        backupGarage.Unpark(vehicle.RegistryNumber);
        View.PrintString("Unparked random: " + vehicle.RegistryNumber);
    }

    // Helper method that lists vehicles optionally filtered
    private void ListVehicles(Filter filter)
    {
        var vehicles2 = backupGarage
            .Where(v => Matches(v, filter))
            .Select(ConvertVehicleToFilter)
            .ToList();
        View.PrintVehicles(vehicles2);
    }
    private void ListVehicles()
    {
        foreach (var garage in garages)
        {
            foreach (var item in garage.Value.GetVehicles())
            {
                Filter filter = ConvertVehicleToFilter(item);
                View.PrintString(filter.ToString());
            }
        }
    }

    public void ListAll() => ListVehicles();

    public void Find(Filter filter) => ListVehicles(filter);
    public void AutoPopulateGarage()
    {
        View.PrintString("Autopopulating...");
        for (int i = 0; i < backupGarage.MaxSize - 1; i++) ParkRandom();
    }

    public void ListApprovedVehicleTypes()
    {
        var sb = new StringBuilder("\nApproved vehicle types:\n");
        foreach (var kv in VehicleTypeRegistry.ApprovedVehicleTypes)
            sb.AppendLine($"{kv.Key}, unique: {kv.Value}");
        View.PrintString(sb.ToString());
    }

    private bool Matches(IVehicle v, Filter f) =>
        (f.RegistryNumber == null || v.RegistryNumber.Equals(f.RegistryNumber, StringComparison.OrdinalIgnoreCase)) &&
        (f.NumWheels == null || v.NumWheels == f.NumWheels) &&
        (f.Color == null || v.Color.Equals(f.Color, StringComparison.OrdinalIgnoreCase)) &&
        (f.VehicleType == null || v.GetType().Name.Equals(f.VehicleType, StringComparison.OrdinalIgnoreCase)) &&
        (f.FuelType == null || v.FuelType.ToString().Equals(f.FuelType, StringComparison.OrdinalIgnoreCase));

    private Filter ConvertVehicleToFilter(IVehicle v) => new Filter
    {
        VehicleType = v.GetType().Name,
        RegistryNumber = v.RegistryNumber,
        Color = v.Color,
        NumWheels = v.NumWheels,
        FuelType = v.FuelType,
        UniquePropertyValue = v.UniquePropertyValue,
        UniquePropertyString = v.UniquePropertyString
    };
}