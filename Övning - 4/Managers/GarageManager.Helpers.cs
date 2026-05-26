using Övning___4.ViewModel;


public partial class GarageManager
{
    private OperationResult RegIsAvailable(string? regNumber)
    {
        if (regNumber is null)
            return OperationResult.Ok("Reg number is null ");
        if (garages.Any(g => g.ContainsVehicleRegNumber(regNumber)))
            return OperationResult.Fail("A vehicle with that registration number is already parked.");
        return OperationResult.Ok("Reg number isn't a duplicate. ");
    }


    private bool Matches(IVehicle v, VehicleFilter f) =>
        (f.RegistryNumber == null || v.RegistryNumber.Equals(f.RegistryNumber, StringComparison.OrdinalIgnoreCase)) &&
        (f.NumWheels == null || v.NumWheels == f.NumWheels) &&
        (f.Color == null || v.Color.Equals(f.Color, StringComparison.OrdinalIgnoreCase)) &&
        (f.VehicleType == null || v.GetType().Name.Equals(f.VehicleType, StringComparison.OrdinalIgnoreCase)) &&
        (f.FuelType == null || v.FuelType.ToString().Equals(f.FuelType, StringComparison.OrdinalIgnoreCase));

    private OperationResult TryGetAllVehicles(out IEnumerable<IVehicle> vehicles)
    {
        vehicles = garages.SelectMany(g => g.GetVehicles());
        if (!vehicles.Any())
            return OperationResult.Fail("Could not find any vehicles.");
        return OperationResult.Ok("Returning vehicles.");
    }


    OperationResult TryNormalizeWord(string? input, out string output)
    {
        output = null;
        if (string.IsNullOrWhiteSpace(input))
            return OperationResult.Fail("String is null or whitespace");

        output = char.ToUpperInvariant(input![0]) + input.Substring(1).ToLowerInvariant();
        return OperationResult.Ok("String could be normalized");
    }
    private OperationResult TryFindGarageContaining(string regNumber, out IGarage? garage)
    {
        garage = garages.FirstOrDefault(g => g.ContainsVehicleRegNumber(regNumber));
        if (garage == default)
            return OperationResult.Fail("Could not find any garage containing " + regNumber);
        return OperationResult.Ok("Found garage containing " + regNumber);
    }


    internal IEnumerable<string> GetGarageStrings() => garages.Select(g => g.ToString());
}
