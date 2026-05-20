using Övning___4.Misc;
using Övning___4.View;
using Övning___4.ViewModel;
using System.Text;

    public partial class GarageManager
    {
    internal void ListAllGarages()
    {
        View.PrintString(IGarage.Header());
        View.PrintIEnumerable(GetGarageStrings());
    }
    public void Find(Filter filter) => ListVehicles(filter);
    public void ListApprovedVehicleTypes()
    {
        var sb = new StringBuilder("\nApproved vehicle types:\n");
        foreach (var kv in VehicleTypeRegistry.ApprovedVehicleTypes)
            sb.AppendLine($"{kv.Key}, unique: {kv.Value}");
        View.PrintString(sb.ToString());
    }

    private void ListVehicles(Filter f)
        {
            var results = garages
                .Select(g => (
                    Matches: g.Where(v => Matches(v, f)).Select(FilterFactory.ConvertVehicleToFilter).ToList(),
                    g.GarageName))
                .Where(x => x.Matches.Count > 0)
                .ToList();

            if (!results.Any())
            {
                View.PrintString($"No vehicles found matching criteria.");
                return;
            }

            foreach (var (matches, garageName) in results)
            {
                View.PrintString($"\nFound: {matches.Count} in: {garageName}:\n");
                matches.ForEach(f => View.PrintString(f.ToString()));
            }
        }
        public void ListAllVehicles()
        {
            if (!garages.Any())
            {
                View.PrintString("No garages exist");
                return;
            }

            var vehicles = GetAllVehicles();
            if (!vehicles.Any())
            {
                View.PrintString("Garages are empty");
                return;
            }

            View.PrintString(Filter.Header());
            foreach (var vehicle in vehicles)
                View.PrintString(FilterFactory.ConvertVehicleToFilter(vehicle).ToString());
        }
    internal bool ListSpecificGarage(string? garageName)
    {
        if (garageName is null)
            return false;

        var garage = garages.FirstOrDefault(x => x.GarageName == garageName);
        if (garage != null)
        {
            var viewConvertedVehicles = garage
                    .Select(FilterFactory.ConvertVehicleToFilter)
                    .ToList();

            View.PrintGarage(viewConvertedVehicles, garage.ToString());
            return true;
        }
        View.PrintString("Garage wasn't found");
        return false;
    }
}

