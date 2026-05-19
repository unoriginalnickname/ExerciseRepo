using Övning___4.Misc;
using Övning___4.View;
using Övning___4.ViewModel;
using System.Text;

public class GarageManager
{


    private List<IGarage> garages = new();

    public void MakeNewGarage<T>(int size, string name) where T : IVehicle
    {
        garages.Add(new Garage<T>(size, name));
    }
    public readonly List<Type> AllGarageTypes = new() { 
        typeof(Airplane), 
        typeof(Boat), 
        typeof(Bus), 
        typeof(Car), 
        typeof(Motorcycle),
        typeof(Uap), 
        typeof(Ufo) };

    public bool IsRegNumberDuplicate(string regNumber) =>
    garages.Any(g => g.GetVehicles().Any(v => v.RegistryNumber == regNumber));

    //this comes from input, input is made into a filter, we might have a garagename

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

    //parkrandom
    public void ParkRandom()
    {
        if (!garages.Any())
        {
            View.PrintString("No garages found, need to create a garage.");
            return;
        }

        var availableGarages = garages.Where(x => x.HasFreeSlots).ToList();
        if (!availableGarages.Any())
        {
            View.PrintString("All garages are full.");
            return;
        }

        var garage = RandomHelper.Pick(availableGarages);
        var vehicle = VehicleFactory.CreateRandomVehicle(garage.TypeOfGarage);
        garage.ParkVehicle(vehicle);
        ParkSuccessMessage(garage, vehicle);
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

    public void UnparkRandomVehicle()
    {
        var garagesWithVehicles = garages.Where(x => x.GetVehicles().Any()).ToList();

        if (!garagesWithVehicles.Any())
        {
            View.PrintString("No vehicles to unpark.");
            return;
        }

        var garage = RandomHelper.Pick(garagesWithVehicles);
        var vehicle = RandomHelper.Pick(garage.GetVehicles().ToList());

        garage.Unpark(vehicle.RegistryNumber);
        View.PrintString($"Unparked {vehicle.GetType().Name} ({vehicle.RegistryNumber}) from {garage.GarageName}");
    }

    // Helper method that lists vehicles optionally filtered
    private void ListVehicles(Filter f)
    {
        List<(List<Filter>, string)> tupleList = new();
     
    
        foreach (IGarage garage in garages)
        {
            var filterList = garage
               .Where(v => Matches(v, f)).Select(FilterFactory.ConvertVehicleToFilter).ToList();
                if(filterList.Count > 0)
            tupleList.Add((filterList, garage.GarageName));
        }
        foreach (var tuple in tupleList)
        {
            View.PrintString($"\nFound: {tuple.Item1.Count()} of {f.VehicleType} in: {tuple.Item2.ToString()}: \n");
            foreach (var filter in tuple.Item1)
            {
                View.PrintString(filter.ToString());
            }
        }
    }
    public void ListAllVehicles()
    {
        if (!garages.Any())
        {
            View.PrintString("No garages exist");
            return;
        }

        // if (!garages.Where(x => x.Value.GetVehicles().Count() > 0).Any()) //old
        if (!garages.Any(x => x.GetVehicles().Any()))
        {
            View.PrintString("Garages are empty");
            return;
        }

        View.PrintString(Filter.Header());
        foreach (var garage in garages)
        {
            foreach (var item in garage.GetVehicles())
            {
                Filter filter = FilterFactory.ConvertVehicleToFilter(item);
                View.PrintString(filter.ToString());
            }
        }
    }



    public void Find(Filter filter) => ListVehicles(filter);

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


    internal bool ListSpecificGarage(string? garageName)
    {
        if (garageName is null)
            return false;

        var garage = garages.FirstOrDefault(x => x.GarageName == garageName);
        if (garage != null)
        {
            var viewConvertedVehicles = garage
                    .Select(item => FilterFactory.ConvertVehicleToFilter((IVehicle)item))
                    .ToList();

            View.PrintGarage(viewConvertedVehicles, garage.ToString());
            return true;
        }
        View.PrintString("Garage wasn't found");
        return false;
    }

    internal void Demo()
    {
        CreateOneOfEachGarage();
        AutoPopulateGarages();
    }
    public void AutoPopulateGarages()
    {
        if (!garages.Any())
        {
            View.PrintString("No garages to populate.");
            return;
        }

        View.PrintString("Autopopulating...");
        foreach (var garage in garages)
        {
            while (garage.HasFreeSlots)
            {
                var vehicle = VehicleFactory.CreateRandomVehicle(garage.TypeOfGarage);
                garage.ParkVehicle(vehicle);
            }
        }
        View.PrintString("All garages populated.");
    }

    public void CreateOneOfEachGarage()
    {
        garages.Add(new Garage<Airplane>(3, "Airplane Garage 1"));
        garages.Add(new Garage<Boat>(3, "Boat Garage 1"));
        garages.Add(new Garage<Bus>(3, "Bus Garage 1"));
        garages.Add(new Garage<Car>(3, "Car Garage 1"));
        garages.Add(new Garage<Motorcycle>(3, "Motorcycle Garage 1"));
        garages.Add(new Garage<Uap>(2, "Classified 1"));
        garages.Add(new Garage<Ufo>(2, "Classified 2"));
        View.PrintString("One of each garage was created");
    }
    string? NormalizeWord(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        return char.ToUpperInvariant(s![0]) + s.Substring(1).ToLowerInvariant();
    }
    internal void CreateGarage(string? garageTypeName, int? garageSize, string? garageName)
    {
        string? correctedTypeName = NormalizeWord(garageTypeName);

        if (correctedTypeName == null)
        {
            View.PrintString("Garage type name cannot be empty.");
            return;
        }

        var type = Type.GetType(correctedTypeName)
            ?? throw new ArgumentException($"Unknown vehicle type '{correctedTypeName}'");

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

    internal IEnumerable<string> GetGarageStrings() => garages.Select(g => g.ToString());
    internal void ListAllGarages()
    {
        View.PrintString(Garage<Car>.Header());
        View.PrintIEnumerable(GetGarageStrings());
    }
}