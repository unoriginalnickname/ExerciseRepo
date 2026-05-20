using Övning___4.Misc;
using Övning___4.View;

    public partial class GarageManager
    {
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

}
