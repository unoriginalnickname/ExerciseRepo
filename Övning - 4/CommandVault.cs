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
        RootCommand root;
       
        public CommandVault()
        {
            root = new RootCommand("Use the listed commands to operate the program");
            InitializeCommands();
            SetupMenu();
        }

        Command listAllVehiclesCommand = new("listall", "Optional: -vehicletype");
        
        Command findVehicleByRegNumberCommand = new("findreg", "Usage: findreg -regnum ABC-123");
        Command listVehiclesWithProperties = new("listspecific", "Usage: listspecific -wheels 2 -color Green -vehicletype Motorcycle -fuel Gasoline");
        Command unparkVehicleCommand = new("unpark", "Usage: unpark -regnum ABC-123");
        Command parkVehicleCommand = new("park", "Usage: park -regnum ABC-123 -type Motorcycle -color Green -wheels 2 -fuel Element155");
        Command exitCommand = new("exit");

        Option<string> registryNumberOption = new Option<string>("-regnum") { HelpName = "platenumber", Required = true };
       
        Option<string> wheelsOption = new("-wheels") {  HelpName = "Amount", Required = false };
       
        Option<string> colorOption = new("-color") {  HelpName = "Color", Required = false };        //make sure to list all the valid color options
     
        Option<string> vehicleTypeOption = new("-vehicletype") {  HelpName ="Vehicletype", Required = false };    //make sure to list all the vehicle options
     
        Option<string> fuelTypeOption = new("-fuel") { HelpName = "Fuel type", Required = false };    //make sure to list all the vehicle options

        Option<string> parkWheelsOption = new("-wheels") { HelpName = "Amount", Required = true };

        Option<string> parkColorOption = new("-color") { HelpName = "Color", Required = true };        //make sure to list all the valid color options

        Option<string> parkVehicleType = new("-type") { HelpName = "Vehicletype", Required = true };    //make sure to list all the vehicle options

        Option<string> parkFuelType = new("-fuel") { HelpName = "Fuel type", Required = true };    //make sure to list all the vehicle options





        void InitializeCommands()
        {

            //exitcommand
            exitCommand.SetAction(p => run = false);
            //list all

            listAllVehiclesCommand.Add(vehicleTypeOption);
            listAllVehiclesCommand.SetAction(p => ListAll(p.GetValue(vehicleTypeOption)));
            

            //park command and options
            parkVehicleCommand.Add(registryNumberOption);
            parkVehicleCommand.Add(parkVehicleType);
            parkVehicleCommand.Add(parkFuelType);
            parkVehicleCommand.Add(parkColorOption);
            parkVehicleCommand.Add(parkWheelsOption);
            parkVehicleCommand.SetAction(p =>
            {
                string? registryNumber = p.GetValue(registryNumberOption);
                if (registryNumber != null && garage.GetVehicleWithRegNumber(registryNumber) == null)
                {
                    string typeName = p.GetValue(parkVehicleType)!;
                    Vehicle vehicle = (Vehicle)Activator.CreateInstance(Type.GetType("GaragePractice." + typeName))!;
                    vehicle.RegistryNumber = p.GetValue(registryNumberOption) ?? "";
                    vehicle.Fueltype = p.GetValue(parkFuelType) ?? "";
                    vehicle.Color = p.GetValue(parkColorOption) ?? "";
                    vehicle.NumWheels = p.GetValue(parkWheelsOption) ?? "";
                    garage.ParkVehicle(vehicle);
                    View.PrintString("Parked. ");
                }
            });


            //unpark command, options and action
            unparkVehicleCommand.Add(registryNumberOption);
            unparkVehicleCommand.SetAction(p => Unpark(p.GetValue(registryNumberOption)));

            //findreg
            findVehicleByRegNumberCommand.Add(registryNumberOption);

            //we need to tell the command what to do with the data provided by the options
            findVehicleByRegNumberCommand.SetAction(p => Search(regNumber: p.GetValue(registryNumberOption)));
            
            //list vehicles command 
            listVehiclesWithProperties.Add(wheelsOption);
            listVehiclesWithProperties.Add(colorOption);
            listVehiclesWithProperties.Add(vehicleTypeOption);
            listVehiclesWithProperties.Add(fuelTypeOption);
            listVehiclesWithProperties.Add(registryNumberOption);

            listVehiclesWithProperties.SetAction(p => Search(p.GetValue(wheelsOption), p.GetValue(colorOption), p.GetValue(vehicleTypeOption), p.GetValue(fuelTypeOption), p.GetValue(registryNumberOption)));

            //add the commands to the root command
            root.Add(listAllVehiclesCommand);
            root.Add(findVehicleByRegNumberCommand);

            root.Add(listVehiclesWithProperties);
            root.Add(unparkVehicleCommand);
            root.Add(parkVehicleCommand);
            root.Add(exitCommand);

        }
        void ListAll(string? vehicleType = "")
        {
            if(vehicleType != null)
            {
                View.PrintVehicles(SearchForVehiclesToDisplayModel(vehicleType: vehicleType));
            }
            else
            {
                View.PrintVehicles(GetAllVehiclesToDisplayModel());
                View.PrintString($"Free garage slots: {garage.TotalFreeSlots()}/{garage.TotalSlots()}");
            }
           
        }


        void Search(string wheels = "", string color = "", string type = "", string fueltype = "", string regNumber = "")
        {
            View.PrintVehicles(SearchForVehiclesToDisplayModel(wheels, color, type, fueltype, regNumber));
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

            View.PrintString("Garage setup, enter garage size(max 100), or press enter for default size(15)");

            bool parseSuccessful = int.TryParse(Console.ReadLine(), out garageSize);
            if (parseSuccessful)
            {
                garage = new(Math.Min(garageSize, 100));
            }
            else
                garage = new();

            View.PrintString("Autopopulate garage with vehicles? Y/N");
            string input = View.GetInput();

            bool yes = string.Equals(input, "Y", StringComparison.InvariantCultureIgnoreCase);
            if (yes)
            {
                garage.AutoPopulateGarage();
                View.PrintString("Garage is now autopopulated.");
            }
        }
  
        IEnumerable<VehicleDisplayModel> SearchForVehiclesToDisplayModel(string numWheels = "", string color = "", string vehicleType = "", string fuelType = "", string regNumber = "")
        {

            IEnumerable<Vehicle> vehicles = garage.GetAllVehicles();
            if (regNumber != "")
                vehicles = vehicles.Where(x => string.Equals(x.RegistryNumber, regNumber, StringComparison.CurrentCultureIgnoreCase));
            if (numWheels != "")
                vehicles = vehicles.Where(x => x.NumWheels == numWheels);
            if (color != "")
                vehicles = vehicles.Where(x => string.Equals(x.Color, color, StringComparison.CurrentCultureIgnoreCase));
            if (vehicleType != "")
                vehicles = vehicles.Where(x => string.Equals(x.GetType().Name, vehicleType, StringComparison.CurrentCultureIgnoreCase));
            if (fuelType != "")
                vehicles = vehicles.Where(x => x.Fueltype.ToString() == fuelType);
          

            foreach (Vehicle item in vehicles)
            {
                yield return VehicleToDisplayModel(item);
            }
        }
        IEnumerable<VehicleDisplayModel> GetAllVehiclesToDisplayModel()
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
