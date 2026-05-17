using GaragePractice;
using Övning___4;
using System.Text;

public class GarageManager
{
    private readonly Garage<IVehicle> garage;

    public GarageManager(int garageSize = 15) => garage = new Garage<IVehicle>(garageSize);

    private bool IsGarageFull => garage.Count() >= garage.MaxSize;

    public bool IsRegNumberDuplicate(string regNumber) => garage.Any(v => v.RegistryNumber == regNumber);

    public void ParkVehicle(Filter filter)
    {
        if (IsGarageFull) { View.PrintString("Garage full."); return; }
        if (IsRegNumberDuplicate(filter.RegNumber)) { View.PrintString("Duplicate reg number."); return; }
        if (!garage.ApprovedVehicleTypes.ContainsKey(filter.VehicleType)) { View.PrintString("Vehicle not approved."); return; }

        garage.ParkVehicle(VehicleFactory.CreateVehicle(filter) ?? throw new InvalidOperationException("Failed to create vehicle."));
        View.PrintString("Parked.");
    }

    public void ParkRandom()
    {
        if (IsGarageFull) { View.PrintString("Garage is full."); return; }

        try
        {
            var existingRegs = new HashSet<string>(garage.Select(v => v.RegistryNumber));
            var vehicle = VehicleFactory.CreateRandomVehicle(garage, existingRegs);
            garage.ParkVehicle(vehicle);
            View.PrintString("Parked random: " + vehicle.RegistryNumber);
        }
        catch (Exception ex) { View.PrintString("Error: " + ex.Message); }
    }

    public void Unpark(string regNumber)
    {
        var vehicle = garage.FirstOrDefault(v => v.RegistryNumber == regNumber);
        if (vehicle != null) { garage.Unpark(vehicle); View.PrintString("Unparked: " + vehicle.RegistryNumber); }
        else View.PrintString("Vehicle not found.");
    }

    public void UnparkRandomVehicle()
    {
        if (garage.Count() == 0) { View.PrintString("Garage empty."); return; }

        var vehicle = RandomHelper.Pick(garage);
        garage.Unpark(vehicle);
        View.PrintString("Unparked random: " + vehicle.RegistryNumber);
    }

    // Helper method that lists vehicles optionally filtered
    private void ListVehicles(Filter filter)
    {
       var vehicles2 = garage
            .Where(v => Matches(v, filter))
            .Select(ConvertVehicleToFilter)
            .ToList();

        View.PrintVehicles(vehicles2);
    }
    private void ListVehicles()
    {
        var vehicles2 = garage
             .Select(ConvertVehicleToFilter)
             .ToList();

        View.PrintVehicles(vehicles2);
    }

    // Now ListAll becomes
    public void ListAll() => ListVehicles();

    // And Find becomes
    public void Find(Filter filter) => ListVehicles(filter);
    public void AutoPopulateGarage()
    {
        View.PrintString("Autopopulating...");
        for (int i = 0; i < garage.MaxSize - 1; i++) ParkRandom();
    }

    public void ListApprovedVehicleTypes()
    {
        var sb = new StringBuilder("\nApproved vehicle types:\n");
        foreach (var kv in garage.ApprovedVehicleTypes)
            sb.AppendLine($"{kv.Key}, unique: {kv.Value}");
        View.PrintString(sb.ToString());
    }

    // --- helpers ---
    private bool Matches(IVehicle v, Filter f) =>
        (f.RegNumber == null || v.RegistryNumber.Equals(f.RegNumber, StringComparison.OrdinalIgnoreCase)) &&
        (f.NumWheels == 0 || v.NumWheels == f.NumWheels) &&
        (f.Color == null || v.Color.Equals(f.Color, StringComparison.OrdinalIgnoreCase)) &&
        (f.VehicleType == null || v.GetType().Name.Equals(f.VehicleType, StringComparison.OrdinalIgnoreCase)) &&
        (f.FuelType == null || v.Fueltype.ToString().Equals(f.FuelType, StringComparison.OrdinalIgnoreCase));

    private Filter ConvertVehicleToFilter(IVehicle v) => new Filter
    {
        VehicleType = v.GetType().Name,
        RegNumber = v.RegistryNumber,
        Color = v.Color,
        NumWheels = v.NumWheels,
        FuelType = v.Fueltype,
        UniquePropertyValue = v.UniquePropertyValue,
        UniquePropertyString = v.UniquePropertyString
    };
}