using System.Linq;
using Övning___4;
using System.CommandLine;
using static System.Net.WebRequestMethods;
using Övning___4.ViewModel;

namespace Övning___4.Commands
{
    public class CommandVault
    {
        public CommandVault()
        {
            Root = new RootCommand();
            Root.Description = "Enter the command followed by --help to see commands.";
            InitializeCommands();
            logic = new CommandRouter(Root);
            logic.Run();
        }

        RootCommand Root;
        CommandRouter logic;

       

        Command demo = new("demo", "Creates one garage of each type of random size and populates each garage");
        Command oneOfEachGarage = new("oneofeach", "Creates one garage of each type of random size");
        Command autoPopulate = new("autopopulate", "Autopopulates each garage");


        Command createGarage = new("creategarage");

        Command listAllVehicles = new("listallvehicles", "Lists all vehicles");
        Command listAllGarages = new("listallgarages", "Lists all garages");

        Command listSpecificGarage = new("listgaragecontents", "--name");

        Command listGarageTypes = new("listgaragetypes", "Lists all garage types");
        Command findCommand = new("find", "Finds the specified vehicle");
        Command parkRandom = new("parkrandom", "Attempts to park a random vehicle");


        Command unparkRandom = new("unparkrandom", "attempts to unpark a random vehicle");
        Command unparkCommand = new("unpark", "--regnum" +
            "\n example: unpark --regnum 123-ABC");
        Command parkCommand = new("park", "Parks the specified vehicle");

        Command exitCommand = new("exit");


        Option<string[]> createGarageNameOption = new Option<string[]>("--name") { HelpName = "Garage Name", Required = true, Arity = ArgumentArity.OneOrMore, AllowMultipleArgumentsPerToken = true };

        Option<string> createGarageTypeOption = new Option<string>("--type") { HelpName = "Garage Type", Required = true };
        Option<int> createGarageSizeOption = new Option<int>("--size") { HelpName = "Garage Size", Required = true };



        Option<string> registryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = false };
        Option<int?> wheelsOption = new("--wheels") { HelpName = "Amount", Required = false };
        Option<string> colorOption = new("--color") { HelpName = "Color", Required = false };
        Option<string> vehicleTypeOption = new("--type") { HelpName = "Vehicletype", Required = false };
        Option<string> fuelTypeOption = new("--fuel") { HelpName = "Fuel type", Required = false };
        Option<string> uniqueOption = new("--unique") { HelpName = "Unique property", Required = false };


        Option<string[]> parkNameOptionNonRequired = new Option<string[]>("--garagename") { HelpName = "garageID", Required = false, Arity = new ArgumentArity(1, 10) };
        Option<string> parkRegistryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = true };
        Option<int> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };
        Option<string> parkVehicleType = new("--type") { HelpName = "Vehicletype", Required = true };
        Option<string> parkFuelType = new("--fuel") { HelpName = "Fuel type", Required = true };
        Option<string> parkUniqueProperty = new("--unique") { HelpName = "Unique property", Required = true };

        void InitializeCommands()
        {
            oneOfEachGarage.SetAction(p => logic.CreateOneOfEachGarage());
            autoPopulate.SetAction(p => logic.Autopopulate());

            listSpecificGarage.Add(createGarageNameOption);
            listSpecificGarage.SetAction(p =>  logic.ListSpecificGarageContents(p.GetValue(createGarageNameOption)));

            createGarage.Add(createGarageTypeOption);
            createGarage.Add(createGarageSizeOption);
            createGarage.Add(createGarageNameOption);

            createGarage.SetAction(p =>
            {
                logic.CreateGarage(p.GetValue(createGarageTypeOption), p.GetValue(createGarageSizeOption), CreateGarageHelper(p, createGarageNameOption));
            });

            listAllGarages.SetAction(p => logic.ListAllGarages());

            demo.SetAction(p => logic.Demo());


           // listSpecificGarage.SetAction(p => logic.ListSpecificGarage(CreateStringFromArrayHelper(p, garageNameOptionNonRequired)));

            listAllVehicles.SetAction(_ => logic.ListAll());
            listGarageTypes.SetAction(_ => logic.ListApprovedVehicleTypes());

            parkCommand.Add(parkRegistryNumberOption);
            parkCommand.Add(parkVehicleType);
            parkCommand.Add(parkFuelType);
            parkCommand.Add(parkColorOption);
            parkCommand.Add(parkWheelsOption);
            parkCommand.Add(parkUniqueProperty);
            parkCommand.Add(parkNameOptionNonRequired);


            parkCommand.SetAction(p =>
            {
                string? reg = p.GetValue(parkRegistryNumberOption);
                string? type = p.GetValue(parkVehicleType);
                string? fuel = p.GetValue(parkFuelType);
                string? color = p.GetValue(parkColorOption);
                string? unique = p.GetValue(parkUniqueProperty);
                int? wheels = p.GetValue(parkWheelsOption);
                string? garageName = p.GetValue(parkNameOptionNonRequired) is string[] arr ? string.Join(" ", arr) : null;


                if (!FilterFactory.TryCreate(requireAllFields: true, out Filter filter, out List<string> errors, reg, type, fuel, color, unique, wheels))
                {
                    logic.PrintErrors(errors);
                    return;
                }

                logic.ParkVehicle(filter, garageName);
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

                if (!FilterFactory.TryCreate(requireAllFields: false, out Filter filter, out List<string> errors, reg, type, fuel, color, unique, wheels))
                {
                    logic.PrintErrors(errors);
                    return;
                }

                logic.Find(filter);
            });



            unparkCommand.Add(parkRegistryNumberOption);
            unparkCommand.SetAction(p =>
            {
                string? registryNumber = p.GetValue(parkRegistryNumberOption);
                if (registryNumber != null)
                    logic.Unpark(registryNumber);
            });


            parkRandom.SetAction(p => logic.ParkRandom());
            unparkRandom.SetAction(p => logic.UnparkRandomVehicle());

            exitCommand.SetAction(p => logic.Exit());

            //add the commands to the root command

            Root.Add(demo);
            Root.Add(oneOfEachGarage);
            Root.Add(autoPopulate);
            Root.Add(createGarage);
            Root.Add(listAllVehicles);
            Root.Add(listAllGarages);
            Root.Add(listSpecificGarage);
            Root.Add(listGarageTypes);
            Root.Add(parkRandom);
            Root.Add(unparkRandom);
            Root.Add(findCommand);
            Root.Add(unparkCommand);
            Root.Add(parkCommand);
            Root.Add(exitCommand);
        }

        private string? CreateGarageHelper(ParseResult p, Option<string[]> garageNameOption)
        {

            return (p.GetValue(garageNameOption) is string[] arr ? string.Join(" ", arr) : null);
        }
        private string? CreateStringFromArrayHelper(ParseResult p, Option<string[]> garageNameOption)
        {
            string[]? array = p.GetValue(garageNameOption);
            string? mystring = (array is string[] arr ? string.Join(" ", arr) : null);

            return mystring;
        }
    }
}