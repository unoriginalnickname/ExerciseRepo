using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using System.Reflection.Metadata;
using System.Text;

namespace GaragePractice
{
    public class CommandVault
    {
        
        Garage garage;
        const int garageMinSize = 15;
        RootCommand root;

        //Command listCommand = new("list", "Optional: -vehicletype");
        
        Command findVehiclesCommand = new("find", "Usage: find --vehicletype Motorcycle --regnum AbC-123 --color Green --wheels 2 --fuel Gasoline");

        Command unparkVehicleCommand = new("unpark", "Usage: unpark --regnum ABC-123");
        Command parkVehicleCommand = new("park", "Usage: park --regnum ABC-123 --vehicletype Motorcycle --color Green --wheels 2 --fuel Element155");
        Command exitCommand = new("exit");


        Option<string> registryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = false };
        Option<string> wheelsOption = new("--wheels") {  HelpName = "Amount", Required = false };
        Option<string> colorOption = new("--color") {  HelpName = "Color", Required = false };        //make sure to list all the valid color options
        Option<string> vehicleTypeOption = new("--vehicletype") {  HelpName ="Vehicletype", Required = false };    //make sure to list all the vehicle options
        Option<string> fuelTypeOption = new("--fuel") { HelpName = "Fuel type", Required = false };    //make sure to list all the vehicle options


        Option<string> parkRegistryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = true };
        Option<string> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };        //make sure to list all the valid color options
        Option<string> parkVehicleType = new("--vehicletype") { HelpName = "Vehicletype", Required = true };    //make sure to list all the vehicle options
        Option<string> parkFuelType = new("--fuel") { HelpName = "Fuel type", Required = true };    //make sure to list all the vehicle options
        //Option<string[]> uniqueTrait = new("-unique") { HelpName = "unique trait", Required = true, Arity = new ArgumentArity(1, 10)};    //make sure to list all the vehicle options
        public CommandVault()
        {
            root = new RootCommand("Use the listed commands to operate the program");
            InitializeCommands();
            SetupMenu();
        }

        void InitializeCommands()
        {
            //exitcommand
            exitCommand.SetAction(p => run = false);

            //park options
            parkVehicleCommand.Add(parkRegistryNumberOption);
            parkVehicleCommand.Add(parkVehicleType);
            parkVehicleCommand.Add(parkFuelType);
            parkVehicleCommand.Add(parkColorOption);
            parkVehicleCommand.Add(parkWheelsOption);
            //parkVehicleCommand.Add(uniqueTrait);

            //unpark options
            unparkVehicleCommand.Add(parkRegistryNumberOption);

            //find vehicles command 
            findVehiclesCommand.Add(registryNumberOption);
            findVehiclesCommand.Add(vehicleTypeOption);
            findVehiclesCommand.Add(wheelsOption);
            findVehiclesCommand.Add(colorOption);
            findVehiclesCommand.Add(fuelTypeOption);


            unparkVehicleCommand.SetAction(p => Unpark(p.GetValue(parkRegistryNumberOption)));
            
            findVehiclesCommand.SetAction(p => Search(p.GetValue(wheelsOption), p.GetValue(colorOption), p.GetValue(vehicleTypeOption), p.GetValue(fuelTypeOption), p.GetValue(registryNumberOption)));
            
            parkVehicleCommand.SetAction(p =>
            {
                string registryNumber = p.GetValue(parkRegistryNumberOption);
                if (registryNumber != null && garage.GetVehicleWithRegNumber(registryNumber) == null)
                {
                    string typeName = p.GetValue(parkVehicleType)!;
                    Vehicle? vehicle = (Vehicle?)Activator.CreateInstance(Type.GetType("GaragePractice." + typeName)!);
                    if(vehicle != null)
                    {
                        vehicle.RegistryNumber = p.GetValue(parkRegistryNumberOption) ?? "";
                        vehicle.Fueltype = p.GetValue(parkFuelType) ?? "";
                        vehicle.Color = p.GetValue(parkColorOption) ?? "";
                        vehicle.NumWheels = p.GetValue(parkWheelsOption) ?? "";

                        View.PrintString($"Vehicle has unique property, provide value for: {vehicle.GetUniquePropertyString()}");

                        vehicle.SetUniqueProperty(View.GetInput());
                        garage.ParkVehicle(vehicle);
                        View.PrintString("Parked. ");
                        return;
                    }
                    View.PrintString("Can't park here.");
                }
                else
                {
                    View.PrintString("Can't park here.");
                }
            });

            //add the commands to the root command
            root.Add(findVehiclesCommand);
            root.Add(unparkVehicleCommand);
            root.Add(parkVehicleCommand);
            root.Add(exitCommand);

        }
        IEnumerable<VehicleDisplayModel> SearchForVehiclesToDisplayModel(string? numWheels = null, string? color = null, string? vehicleType = null, string? fuelType = null, string? regNumber = null)
        {

            IEnumerable<Vehicle> vehicles = garage.GetAllVehicles();
            if (regNumber != null)
                vehicles = vehicles.Where(x => string.Equals(x.RegistryNumber, regNumber, StringComparison.CurrentCultureIgnoreCase));
            if (numWheels != null)
                vehicles = vehicles.Where(x => x.NumWheels == numWheels);
            if (color != null)
                vehicles = vehicles.Where(x => string.Equals(x.Color, color, StringComparison.OrdinalIgnoreCase));
            if (vehicleType != null)
                vehicles = vehicles.Where(x => string.Equals(x.GetType().Name, vehicleType, StringComparison.CurrentCultureIgnoreCase));
            if (fuelType != null)
                vehicles = vehicles.Where(x => x.Fueltype.ToString() == fuelType);

            foreach (Vehicle item in vehicles)
            {
                yield return VehicleToDisplayModel(item);
            }
        }

        void Search(string? wheels = null, string? color = null, string? vehicleType = null, string? fueltype = null, string? regNumber = null)
        {
            View.PrintVehicles(SearchForVehiclesToDisplayModel(wheels, color, vehicleType, fueltype, regNumber));
        }

        private void Unpark(string? regNumber)
        {
            if (regNumber != null)
            {
                View.PrintString(garage.UnParkVehicle(regNumber) ? "Car successfully unparked" : "Car was not unparked");
            }
        }

        bool run = true;
        public void Run()
        {
            root.Parse("--help").Invoke();
            while (run)
            {
                var input = View.GetInput().Split();
                root.Parse(input).Invoke();
            }
        }

        void SetupMenu()
        {
            int garageSize;

            View.PrintString($"Garage setup, enter garage size(max 100), or press enter for default size({garageMinSize})");

            bool parseSuccessful = int.TryParse(View.GetInput(), out garageSize);
            if (parseSuccessful)
            {
                garage = new(Math.Max(Math.Min(garageSize, 100), garageMinSize));
            }
            else
                garage = new();

            View.PrintString("\nAutopopulate garage with vehicles? Y/N");
            string input = View.GetInput();

            bool yes = string.Equals(input, "Y", StringComparison.InvariantCultureIgnoreCase);
            if (yes)
            {
                garage.AutoPopulateGarage();
                View.PrintString("\nGarage is now autopopulated.\n");
            }
        }
  

        IEnumerable<VehicleDisplayModel> AllVehiclesToDisplayModel()
        {
            foreach (Vehicle v in garage.GetAllVehicles())
            {
                yield return VehicleToDisplayModel(v);

            }
        }

        private VehicleDisplayModel VehicleToDisplayModel(Vehicle vehicle)
        {
            VehicleDisplayModel model = new VehicleDisplayModel
            {
                VehicleType = vehicle.GetType().Name,
                RegPlateNumber = vehicle.RegistryNumber,
                Color = vehicle.Color,
                NumWheels = vehicle.NumWheels,
                Fueltype = vehicle.Fueltype.ToString(),
                UniqueProperties = vehicle.GetUniqueProperty()
            };
            return model;
        }
    }
}