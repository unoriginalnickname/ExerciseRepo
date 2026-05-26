using Övning___4.Misc;
using Övning___4.ViewModel;
using System.Text;

public partial class GarageManager
{
    public OperationResult ListAllGarages()
    {
        IEnumerable<string> garages = GetGarageStrings();
        if (!garages.Any())
            return OperationResult.Fail("No garages exist.");

        return OperationResult.Ok(IGarage.Header() + "\n"+ string.Join("\n", GetGarageStrings()));
    }
    public OperationResult ListApprovedVehicleTypes()
    {
        var sb = new StringBuilder("\nApproved vehicle types:\n");
        foreach (var kv in VehicleTypeRegistry.ApprovedVehicleTypes)
            sb.AppendLine($"{kv.Key}, unique: {kv.Value}");
       return OperationResult.Ok (sb.ToString());
    }

    public OperationResult ListVehicles(VehicleFilter f)
    {
        var garageResults = garages
            .Select(g => (
                Matches: g.Where(v => Matches(v, f)).Select(FilterFactory.ConvertVehicleToFilter).ToList(),
                g.Name))
            .Where(x => x.Matches.Count > 0)
            .ToList();

        if (!garageResults.Any())
            return OperationResult.Fail($"No vehicles found matching criteria.");

        StringBuilder sb = new StringBuilder();
        foreach (var (matches, garageName) in garageResults)
        {
            sb.Append($"\nFound:{matches.Count} in \"{garageName}\":\n");
            matches.ForEach(f => sb.Append(f.ToString() + "\n"));
        }

        return OperationResult.Ok(sb.ToString());
    }

    public OperationResult ListAllVehicles()
    {
        if (!garages.Any())
            return OperationResult.Fail("No garages exist");

        IEnumerable<IVehicle> vehicles;
        var result = TryGetAllVehicles(out vehicles);
        if (!result.Success)
            return result;

        var lines = vehicles.Select(v => FilterFactory.ConvertVehicleToFilter(v).ToString());
        return OperationResult.Ok(VehicleFilter.Header() + "\n" + string.Join("\n", lines));
    }

    public OperationResult TryListSpecificGarage(string? garageName)
    {
        if (garageName is null)
            return OperationResult.Fail("No garagename provided");

        var garage = garages.FirstOrDefault(x => string.Equals(
            x.Name, garageName, StringComparison.CurrentCultureIgnoreCase));
        if (garage != null)
        {
            var viewConvertedVehicles = garage
                    .Select(FilterFactory.ConvertVehicleToFilter)
                    .ToList();
       
           return OperationResult.Ok(string.Join("\n", viewConvertedVehicles), garage.ToString());
        }
        return OperationResult.Fail("Garage wasn't found");
    }
}