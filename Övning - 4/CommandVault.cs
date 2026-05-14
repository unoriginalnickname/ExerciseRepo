using Övning___4;
using System.CommandLine;

namespace GaragePractice
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
        Command unparkCommand = new("unpark", "--regnum" +
            "\n example: unpark --regnum 123-ABC");
        Command parkCommand = new("park", "--regnum --vehicletype --color --wheels --fuel --unique (ALL REQUIRED)" +
            "\n example: park --regnum 123-ABC --vehicletype car --color Red --wheels 2 --fuel Gas");
        Command exitCommand = new("exit");


        Option<string> registryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = false };
        Option<string> wheelsOption = new("--wheels") { HelpName = "Amount", Required = false }; //future implementations: list all the valid color, vehicle options
        Option<string> colorOption = new("--color") { HelpName = "Color", Required = false };
        Option<string> vehicleTypeOption = new("--vehicletype") { HelpName = "Vehicletype", Required = false };
        Option<string> fuelTypeOption = new("--fuel") { HelpName = "Fuel type", Required = false };

        Option<string> parkRegistryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = true };
        Option<string> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };
        Option<string> parkVehicleType = new("--vehicletype") { HelpName = "Vehicletype", Required = true };
        Option<string> parkFuelType = new("--fuel") { HelpName = "Fuel type", Required = true };
        Option<string> parkUniqueProperty = new("--unique") { HelpName = "Unique property", Required = true };

        void InitializeCommands()
        {
            listall.SetAction(_ => logic.ListAll());
            listApprovedVehicleTypes.SetAction(_ => logic.ListApprovedVehicleTypes());

            parkCommand.Add(parkRegistryNumberOption); parkCommand.Add(parkVehicleType); parkCommand.Add(parkFuelType); parkCommand.Add(parkColorOption); parkCommand.Add(parkWheelsOption); parkCommand.Add(parkUniqueProperty);
            parkCommand.SetAction(p => logic.Park(new Filter()
            {
                NumWheels = p.GetValue(parkWheelsOption),
                Color = p.GetValue(parkColorOption),
                FuelType = p.GetValue(parkFuelType),
                RegNumber = p.GetValue(parkRegistryNumberOption),
                VehicleType = p.GetValue(parkVehicleType),
                UniqueProperty = p.GetValue(parkUniqueProperty)
            }));

            findCommand.Add(registryNumberOption); findCommand.Add(vehicleTypeOption); findCommand.Add(wheelsOption); findCommand.Add(colorOption); findCommand.Add(fuelTypeOption);
            findCommand.SetAction(p => logic.Find(new Filter()
            {
                NumWheels = p.GetValue(wheelsOption),
                Color = p.GetValue(colorOption),
                VehicleType = p.GetValue(vehicleTypeOption),
                FuelType = p.GetValue(fuelTypeOption),
                RegNumber = p.GetValue(registryNumberOption)
            }));

            unparkCommand.Add(parkRegistryNumberOption);
            unparkCommand.SetAction(p => logic.Unpark(p.GetValue(parkRegistryNumberOption)));

            exitCommand.SetAction(p => logic.Exit());

            //add the commands to the root command
            Root.Add(listall);
            Root.Add(listApprovedVehicleTypes);
            Root.Add(findCommand);
            Root.Add(unparkCommand);
            Root.Add(parkCommand);
            Root.Add(exitCommand);
        }
    }
}