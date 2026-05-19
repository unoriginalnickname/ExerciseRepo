using Övning___4.Misc;
using Övning___4.View;
using Övning___4.ViewModel;
using System.Text;

public class GarageManager
{
    public GarageManager()
    {

    }

    private List<IGarage> garages = new();

    Garage<IVehicle> backupGarage = new Garage<IVehicle>(400, "Backup garage 1");

    public void MakeNewGarage<T>(int size, string name) where T : IVehicle
    {
        garages.Add(new Garage<T>(size, name));
    }
    public readonly List<Type> AllGarageTypes = new() { 
        typeof(Ufo), 
        typeof(Airplane), 
        typeof(Boat), 
        typeof(Bus), 
        typeof(Car), 
        typeof(Motorcycle),
        typeof(Uap), 
        typeof(Ufo) };

    public bool IsRegNumberDuplicate(string regNumber) => backupGarage.Any(v => v.RegistryNumber == regNumber);

    //this comes from input, input is made into a filter, we might have a garagename
    
    //parking
    public void TryParkVehicle(Filter filter, string? garageName)
    {
        var garage = garages.Where(x => 
        x.TypeOfGarage == Type.GetType(filter.VehicleType) 
        && x.HasFreeSlots
         && (garageName == null || x.GarageName == garageName)).FirstOrDefault();

        if(garage != null)
        {
            var vehicle = VehicleFactory.CreateVehicle(filter);
            garage.ParkVehicle(vehicle);
            ParkSuccessMessage(garage, vehicle);
        }
        else
            Console.WriteLine("Could not find an available garage. ");

    }

    private IGarage? FindMatchingGarageWithAvailableSlots(Type type)
    {
        throw new NotImplementedException();
    }

    //parkrandom
    public void ParkRandom()
    {
        if (!garages.Any())
            View.PrintString("No garages found, need to create a garage"); //decision here is either to force making a garage or making a default garage.
        //Type vehicleType = RandomHelper.Pick(AllGarageTypes);

        IGarage garage = garages.Where(x => x.HasFreeSlots).FirstOrDefault();
        if(garage != null)
        {
            Type garageType = garage.TypeOfGarage;
            if (garageType != null)
            {

                IVehicle vehicle = VehicleFactory.CreateRandomVehicle(garageType);

                garage.ParkVehicle(vehicle);
                ParkSuccessMessage(garage, vehicle);

                return;
            }
        }

        View.PrintString("Could not park random. ");
    }

    private static void ParkSuccessMessage(IGarage garage, IVehicle vehicle)
    {
        View.PrintString($"Parked Vehicle in garage: {garage.GarageName}");
        View.PrintString(Filter.Header());
        View.PrintString($"{FilterFactory.ConvertVehicleToFilter(vehicle)} ");
    }

    public void Unpark(string regNumber)
    {
        backupGarage.Unpark(regNumber);
        View.PrintString("Unparked: ");
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
               .Where(v => Matches(v, f)).Select(ConvertVehicleToFilter).ToList();
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
    private void ListVehicles()
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
                Filter filter = ConvertVehicleToFilter(item);
                View.PrintString(filter.ToString());
            }
        }
    }

    public void ListAll() => ListVehicles();

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

    internal bool ListSpecificGarage(string? garageName)
    {
        if (garageName is null)
            return false;

        var garage = garages.Where(x => x.GarageName == garageName).Select(x => x).FirstOrDefault();
        if(garage != null)
        {
            List<Filter> viewConvertedVehicles = new List<Filter>();

            foreach (var item in garage)
                viewConvertedVehicles.Add(FilterFactory.ConvertVehicleToFilter((IVehicle)item));

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
    }
    string? NormalizeWord(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        return char.ToUpperInvariant(s![0]) + s.Substring(1).ToLowerInvariant();
    }
    internal void CreateGarage(string? garageTypeName, int? garageSize, string? garageName)
    {

        string? correctedTypeName = NormalizeWord(garageTypeName);

        // fix toupper and fix capitalization here
        var type = Type.GetType(correctedTypeName); // e.g. "MyNamespace.Car"

        var actualGarageType = typeof(Garage<>).MakeGenericType(type); //crashes here

        var garage = (IGarage)Activator.CreateInstance(actualGarageType, garageSize ?? 55, garageName ?? "default name");

        garages.Add(garage);
        View.PrintString("Garage was added. " + garage.ToString());
    }

    internal IEnumerable<string> GetGarageStrings()
    {
        List<string> garageStrings = new List<string>();

        foreach (var item in garages)
        {
            garageStrings.Add(item.ToString());
        }

        return garageStrings;
    }
 
    internal void ListAllGarages()
    {
        //car is just to get access to header inside garage.
        View.PrintString(Garage<Car>.Header());
        View.PrintIEnumerable(GetGarageStrings());
    }

    //internal IEnumerable<string> GetAllGarageNames()
    //{
    //    List<string> garageNames = new List<string>();

    //    foreach (var garage in garages)
    //    {


    //    }
    //}
}