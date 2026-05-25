using Övning___4.Misc;
using Övning___4.ViewModel;

public partial class GarageManager
    {

    public OperationResult AutoPopulateGarages()
    {
        if (!garages.Any())
            return OperationResult.Fail("No garages to populate.");

        foreach (var garage in garages)
        {
            while (garage.HasFreeSlots)
            {
                var vehicle = VehicleFactory.CreateRandomVehicle(garage.GarageVehicleType);
                garage.ParkVehicle(vehicle);
            }
        }
        return OperationResult.Ok("All garages populated.");
    }

    public OperationResult CreateOneOfEachGarage()
    {
        garages.Add(new Garage<Airplane>(3, "Airplane Garage 1"));
        garages.Add(new Garage<Boat>(3, "Boat Garage 1"));
        garages.Add(new Garage<Bus>(3, "Bus Garage 1"));
        garages.Add(new Garage<Car>(3, "Car Garage 1"));
        garages.Add(new Garage<Motorcycle>(3, "Motorcycle Garage 1"));
        garages.Add(new Garage<Uap>(2, "Classified 1"));
        garages.Add(new Garage<Ufo>(2, "Classified 2"));
        return OperationResult.Ok("One of each garage was created");
    }
    //parkrandom
    public OperationResult ParkRandom()
    {
        if (!garages.Any())
            return OperationResult.Fail("No garages found, need to create a garage.");

        var availableGarages = garages.Where(x => x.HasFreeSlots).ToList();
        if (!availableGarages.Any())
            return OperationResult.Fail("All garages are full.");

        var garage = RandomHelper.Pick(availableGarages);
        var vehicle = VehicleFactory.CreateRandomVehicle(garage.GarageVehicleType);
        garage.ParkVehicle(vehicle);
        return OperationResult.Ok(ParkSuccessMessage(garage, vehicle));
    }

    public OperationResult UnparkRandomVehicle()
    {
        var garagesWithVehicles = garages.Where(x => x.GetVehicles().Any()).ToList();

        if (!garagesWithVehicles.Any())
        {
            return OperationResult.Fail("No vehicles to unpark.");
        }

        var garage = RandomHelper.Pick(garagesWithVehicles);
        var vehicle = RandomHelper.Pick(garage.GetVehicles().ToList());

        garage.Unpark(vehicle.RegistryNumber);
        return OperationResult.Ok($"Unparked {vehicle.GetType().Name} ({vehicle.RegistryNumber}) from {garage.Name}");
    }

}
