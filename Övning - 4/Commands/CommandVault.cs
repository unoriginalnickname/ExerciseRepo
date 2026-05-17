using GaragePractice;
using Övning___4;
using System.CommandLine;
using static System.Net.WebRequestMethods;

namespace Övning___4.Commands
{
    public class CommandVault
    {
        public CommandVault()
        {
            Root = new RootCommand();
            InitializeCommands();
            logic = new CommandLogic(Root);
            logic.Run();
        }

        RootCommand Root;
        CommandLogic logic;

        Command listall = new("listall", "Lists all parked vehicles in the garage.");
        Command listApprovedVehicleTypes = new("listapproved", "Lists all approved vehicle types for parking in the garage.");
        Command findCommand = new("find", "--vehicletype --regnum --color --wheels --fuel" +
            "\nexample: find --fuel Gas --vehicletype Car");
        Command parkRandom = new("parkrandom", "attempts to park a random vehicle");
        Command unparkRandom = new("unparkrandom", "attempts to unpark a random vehicle");
        Command unparkCommand = new("unpark", "--regnum" +
            "\n example: unpark --regnum 123-ABC");
        Command parkCommand = new("park", "--regnum --vehicletype --color --wheels --fuel --unique (ALL REQUIRED)" +
            "\n example: park --regnum 123-ABC --vehicletype car --color Red --wheels 2 --fuel Gas");
        
        Command exitCommand = new("exit");


        Option<string> registryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = false };
        Option<int?> wheelsOption = new("--wheels") { HelpName = "Amount", Required = false };
        Option<string> colorOption = new("--color") { HelpName = "Color", Required = false };
        Option<string> vehicleTypeOption = new("--vehicletype") { HelpName = "Vehicletype", Required = false };
        Option<string> fuelTypeOption = new("--fuel") { HelpName = "Fuel type", Required = false };
        Option<string> uniqueOption = new("--unique") { HelpName = "Unique property", Required = false };


        Option<string> parkRegistryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = true };
        Option<int> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };
        Option<string> parkVehicleType = new("--vehicletype") { HelpName = "Vehicletype", Required = true };
        Option<string> parkFuelType = new("--fuel") { HelpName = "Fuel type", Required = true };
        Option<string> parkUniqueProperty = new("--unique") { HelpName = "Unique property", Required = true };

        void InitializeCommands()
        {
            listall.SetAction(_ => logic.ListAll());
            listApprovedVehicleTypes.SetAction(_ => logic.ListApprovedVehicleTypes());

            parkCommand.Add(parkRegistryNumberOption); parkCommand.Add(parkVehicleType); parkCommand.Add(parkFuelType); parkCommand.Add(parkColorOption); parkCommand.Add(parkWheelsOption); parkCommand.Add(parkUniqueProperty);
            parkCommand.SetAction(p =>
            {
                string? reg = p.GetValue(parkRegistryNumberOption);
                string? type = p.GetValue(parkVehicleType);
                string? fuel = p.GetValue(parkFuelType);
                string? color = p.GetValue(parkColorOption);
                string? unique = p.GetValue(parkUniqueProperty);
                int? wheels = p.GetValue(parkWheelsOption);
           

                var filter = new FilterX
                {
                    NumWheels = wheels,
                    Color = color,
                    FuelType = fuel,
                    RegNumber = reg,
                    VehicleType = type,
                    UniquePropertyValue = unique
                };
                filter.Color = Normalize(filter.Color);
                filter.VehicleType = Normalize(filter.VehicleType);
                filter.RegNumber = NormalizeReg(filter.RegNumber);
                filter.FuelType = Normalize(filter.FuelType);

                if (!FilterValidator.ValidateForPark(filter))
                {
                    Console.WriteLine("Invalid vehicle data for parking.");
                    return;
                }

                logic.ParkVehicle(filter);
            });

            findCommand.Add(registryNumberOption); findCommand.Add(vehicleTypeOption); findCommand.Add(wheelsOption); findCommand.Add(colorOption); findCommand.Add(fuelTypeOption); findCommand.Add(uniqueOption);

            findCommand.SetAction(p =>
            {
                string? reg = p.GetValue(registryNumberOption);
                string? type = p.GetValue(vehicleTypeOption);
                string? fuel = p.GetValue(fuelTypeOption);
                string? color = p.GetValue(colorOption);
                string? unique = p.GetValue(uniqueOption);
                int? wheels = p.GetValue(wheelsOption);

                var filter = new FilterX
                {
                    NumWheels = wheels,
                    Color = color,
                    FuelType = fuel,
                    RegNumber = reg,
                    VehicleType = type,
                    UniquePropertyValue = unique
                };

                filter.Color = Normalize(filter.Color);
                filter.VehicleType = Normalize(filter.VehicleType);
                filter.RegNumber = NormalizeReg(filter.RegNumber);
                filter.FuelType = Normalize(filter.FuelType);

                if (!FilterValidator.ValidateForFind(filter))
                {
                    Console.WriteLine("Invalid vehicle data for search.");
                    return;
                }

                logic.Find(filter);
            });



            unparkCommand.Add(parkRegistryNumberOption);
            unparkCommand.SetAction(p => logic.Unpark(p.GetValue(parkRegistryNumberOption)));

            parkRandom.SetAction(p => logic.ParkRandom());
            unparkRandom.SetAction(p => logic.UnparkRandomVehicle());

            exitCommand.SetAction(p => logic.Exit());

            //add the commands to the root command
            Root.Add(listall);
            Root.Add(listApprovedVehicleTypes);
            Root.Add(parkRandom);
            Root.Add(unparkRandom);
            Root.Add(findCommand);
            Root.Add(unparkCommand);
            Root.Add(parkCommand);
            Root.Add(exitCommand);
        }

        private string? Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }
        private string? NormalizeReg(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.All(char.IsLetterOrDigit) ? value.ToUpper() : null;
        }
    }
}