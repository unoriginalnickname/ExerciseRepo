using Microsoft.VisualBasic.FileIO;
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
        const int garageMinSize = 15;
        Garage garage;

        RootCommand root;
        Command listall = new("listall", "lists all vehicles");
        Command findCommand = new("find", "Usage: find --vehicletype Motorcycle --regnum AbC-123 --color Green --wheels 2 --fuel Gasoline");
        Command unparkCommand = new("unpark", "Usage: unpark --regnum ABC-123");
        Command parkCommand = new("park", "Usage: park --regnum ABC-123 --vehicletype Motorcycle --color Green --wheels 2 --fuel Element155");
        Command exitCommand = new("exit");


        Option<string> registryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = false };
        Option<string> wheelsOption = new("--wheels") { HelpName = "Amount", Required = false }; //future implementations
        Option<string> colorOption = new("--color") { HelpName = "Color", Required = false };         //list all the valid color options
        Option<string> vehicleTypeOption = new("--vehicletype") { HelpName = "Vehicletype", Required = false };    //list all the vehicle options
        Option<string> fuelTypeOption = new("--fuel") { HelpName = "Fuel type", Required = false };


        Option<string> parkRegistryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = true };
        Option<string> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };        //make sure to list all the valid color options
        Option<string> parkVehicleType = new("--vehicletype") { HelpName = "Vehicletype", Required = true };    //make sure to list all the vehicle options
        Option<string> parkFuelType = new("--fuel") { HelpName = "Fuel type", Required = true };    //make sure to list all the vehicle options


        public CommandVault()
        {
            root = new RootCommand("Garage");
            InitializeCommands();
            SetupMenu();
        }

        void InitializeCommands()
        {
            parkCommand.Add(parkRegistryNumberOption); parkCommand.Add(parkVehicleType); parkCommand.Add(parkFuelType); parkCommand.Add(parkColorOption); parkCommand.Add(parkWheelsOption);

            findCommand.Add(registryNumberOption); findCommand.Add(vehicleTypeOption); findCommand.Add(wheelsOption); findCommand.Add(colorOption); findCommand.Add(fuelTypeOption);

            unparkCommand.Add(parkRegistryNumberOption);

            unparkCommand.SetAction(p => Unpark(p.GetValue(parkRegistryNumberOption)));


            listall.SetAction(_ => ListAll());
            findCommand.SetAction(p => Find(p.GetValue(wheelsOption), p.GetValue(colorOption), p.GetValue(vehicleTypeOption), p.GetValue(fuelTypeOption), p.GetValue(registryNumberOption)));

            parkCommand.SetAction(p => Park(p.GetValue(parkWheelsOption), p.GetValue(parkColorOption), p.GetValue(parkVehicleType), p.GetValue(parkFuelType), p.GetValue(parkRegistryNumberOption)));

            exitCommand.SetAction(p => run = false);

            //add the commands to the root command
            root.Add(listall);
            root.Add(findCommand);
            root.Add(unparkCommand);
            root.Add(parkCommand);
            root.Add(exitCommand);
        }
        void Park(string? wheels = null, string? color = null, string? vehicleType = null, string? fueltype = null, string? registryNumber = null)
        {
            if (registryNumber != null && FindVehiclesToDisplayModelArrayMethod(regNumber: registryNumber).Length == 0)
            {
                Vehicle? vehicle = (Vehicle?)Activator.CreateInstance(Type.GetType("GaragePractice." + vehicleType)!);
                if (vehicle != null)
                {
                    vehicle.RegistryNumber = registryNumber ?? "";
                    vehicle.Fueltype = fueltype ?? "";
                    vehicle.Color = color ?? "";
                    vehicle.NumWheels = wheels ?? "";

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
        }

        VehicleDisplayModel[] FindVehiclesToDisplayModelArrayMethod(string? numWheels = null, string? color = null, string? vehicleType = null, string? fuelType = null, string? regNumber = null)
        {
            Vehicle[] vehicles = garage.GetAllVehiclesToArray();
            Vehicle[] vehiclesMatching = new Vehicle[vehicles.Length];
            VehicleDisplayModel[] displayModelArray;

            //get whatever is matching
            for (int i = 0; i < vehicles.Length - 1; i++)
            {
                if (regNumber != null && vehicles[i] != null)
                    if (string.Equals(vehicles[i].RegistryNumber, regNumber, StringComparison.CurrentCultureIgnoreCase))
                        vehiclesMatching[i] = vehicles[i];

                if (numWheels != null && vehicles[i] != null)
                    if (string.Equals(vehicles[i].NumWheels, numWheels, StringComparison.CurrentCultureIgnoreCase))
                        vehiclesMatching[i] = vehicles[i];

                if (color != null && vehicles[i] != null)
                    if (string.Equals(vehicles[i].Color, color, StringComparison.OrdinalIgnoreCase))
                        vehiclesMatching[i] = vehicles[i];

                if (vehicleType != null && vehicles[i] != null)
                    if (string.Equals(vehicles[i].GetType().Name, vehicleType, StringComparison.CurrentCultureIgnoreCase))
                        vehiclesMatching[i] = vehicles[i];

                if (fuelType != null && vehicles[i] != null)
                    if (vehicles[i].Fueltype.ToString() == fuelType)
                        vehiclesMatching[i] = vehicles[i];
            }
            return VehicleArrToDisplayArr(vehiclesMatching);
        }

        private VehicleDisplayModel[] VehicleArrToDisplayArr(Vehicle[] vehiclesMatching)
        {
            VehicleDisplayModel[] displayModelArray;
            int numVehiclesFound = 0;
            for (int i = 0; i < vehiclesMatching.Length - 1; i++)
            {
                if (vehiclesMatching[i] != null)
                {
                    numVehiclesFound++;
                }
            }
            //now we know the size we need
            displayModelArray = new VehicleDisplayModel[numVehiclesFound];

            int displayModelIndex = 0;
            for (int i = 0; i < vehiclesMatching.Length - 1; i++)
            {
                if (vehiclesMatching[i] != null)
                {
                    displayModelArray[displayModelIndex] = VehicleToDisplayModel(vehiclesMatching[i]);
                    displayModelIndex++;
                }
            }

            return displayModelArray;
        }

        private void ListAll()
        {
            View.PrintVehicles(VehicleArrToDisplayArr(garage.GetAllVehiclesToArray()));
        }


        void Find(string? wheels = null, string? color = null, string? vehicleType = null, string? fueltype = null, string? regNumber = null)
        {
            if (wheels == null && color == null && vehicleType == null && fueltype == null && regNumber == null)
                root.Parse("find --help").Invoke();
            else
                View.PrintVehicles(FindVehiclesToDisplayModelArrayMethod(wheels, color, vehicleType, fueltype, regNumber));
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