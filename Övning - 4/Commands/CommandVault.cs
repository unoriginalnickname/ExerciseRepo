using System.CommandLine;
using Övning___4.ViewModel;

namespace Övning___4.Commands
{
    public class CommandVault
    {
        public CommandVault()
        {
            Root = new("Enter the command followed by --help to see commands.");
            logic = new CommandRouter(Root);
            InitializeCommands();
            logic.Run();
        }

        RootCommand Root;
        CommandRouter logic;


        Command demo = new("demo", "[Setup] Creates one garage of each type and populates each garage");
        Command oneOfEachGarage = new("oneofeach", "[Setup] Creates one garage of each type");
        Command autoPopulate = new("autopopulate", "[Setup] Autopopulates each garage");
        Command createGarage = new("creategarage", "[Garage] Creates a new garage of the specified type");
        Command listAllVehicles = new("listallvehicles", "[Vehicle] Lists all vehicles");
        Command listAllGarages = new("listallgarages", "[Garage] Lists all garages");
        Command listSpecificGarage = new("listgaragecontents", "[Garage] Lists all vehicles in the specified garage");
        Command listGarageTypes = new("listgaragetypes", "[Garage] Lists all approved garage types");
        Command findCommand = new("find", "[Vehicle] Finds vehicles matching the specified criteria");
        Command parkRandom = new("parkrandom", "[Vehicle] Attempts to park a random vehicle");
        Command unparkRandom = new("unparkrandom", "[Vehicle] Attempts to unpark a random vehicle");
        Command unparkCommand = new("unpark", "[Vehicle] Unparks a vehicle by registration number");
        Command parkCommand = new("park", "[Vehicle] Parks the specified vehicle");
        Command exitCommand = new("exit", "[App] Exits the application");


        // Garage options
        Option<string[]> garageNameOption = new("--garagename") { HelpName = "Garage name", Required = true, Arity = ArgumentArity.OneOrMore, AllowMultipleArgumentsPerToken = true };
        Option<string> garageTypeOption = new("--type") { HelpName = "Garage type", Required = true };
        Option<int> garageSizeOption = new("--size") { HelpName = "Garage size", Required = true };

        // Park options (required)
        Option<string> parkRegNumOption = new("--regnum") { HelpName = "Plate number", Required = true };
        Option<int?> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };
        Option<string> parkTypeOption = new("--type") { HelpName = "Vehicle type", Required = true };
        Option<string> parkFuelOption = new("--fuel") { HelpName = "Fuel type", Required = true };
        Option<string> parkUniqueOption = new("--unique") { HelpName = "Unique", Required = true };
        Option<string[]> parkGarageOption = new("--garagename") { HelpName = "Garage name", Required = false, Arity = new ArgumentArity(0, 10), AllowMultipleArgumentsPerToken = true };

        // Find options (optional)
        Option<string> findRegNumOption = new("--regnum") { HelpName = "Plate number", Required = false };
        Option<int?> findWheelsOption = new("--wheels") { HelpName = "Amount", Required = false };
        Option<string> findColorOption = new("--color") { HelpName = "Color", Required = false };
        Option<string> findTypeOption = new("--type") { HelpName = "Vehicle type", Required = false };
        Option<string> findFuelOption = new("--fuel") { HelpName = "Fuel type", Required = false };
        Option<string> findUniqueOption = new("--unique") { HelpName = "Unique", Required = false };

        // Unpark option
        Option<string> unparkRegNumOption = new("--regnum") { HelpName = "Plate number", Required = true };

        // List specific garage option
        Option<string[]> listGarageNameOption = new("--garagename") { HelpName = "Garage name", Required = true, Arity = ArgumentArity.OneOrMore, AllowMultipleArgumentsPerToken = true };
        Option<string[]> listSpecificGarageNameOption = new("--garagename") { HelpName = "Garage name", Required = true, Arity = ArgumentArity.OneOrMore, AllowMultipleArgumentsPerToken = true };



        void InitializeCommands()
        {

            createGarage.Add(garageNameOption);
            createGarage.Add(garageTypeOption);
            createGarage.Add(garageSizeOption);

            parkCommand.Add(parkRegNumOption);
            parkCommand.Add(parkWheelsOption);
            parkCommand.Add(parkColorOption);
            parkCommand.Add(parkTypeOption);
            parkCommand.Add(parkFuelOption);
            parkCommand.Add(parkUniqueOption);
            parkCommand.Add(parkGarageOption);

            findCommand.Add(findRegNumOption);
            findCommand.Add(findWheelsOption);
            findCommand.Add(findColorOption);
            findCommand.Add(findTypeOption);
            findCommand.Add(findFuelOption);
            findCommand.Add(findUniqueOption);

            listSpecificGarage.Add(listSpecificGarageNameOption);

            unparkCommand.Add(unparkRegNumOption);

            demo.SetAction(_ => logic.Demo());
            oneOfEachGarage.SetAction(_ => logic.CreateOneOfEachGarage());
            autoPopulate.SetAction(_ => logic.Autopopulate());
            listAllGarages.SetAction(_ => logic.ListAllGarages());
            parkRandom.SetAction(_ => logic.ParkRandom());
            unparkRandom.SetAction(_ => logic.UnparkRandomVehicle());
            exitCommand.SetAction(_ => logic.Exit());
            listAllVehicles.SetAction(_ => logic.ListAll());
            listGarageTypes.SetAction(_ => logic.ListApprovedVehicleTypes());

            unparkCommand.SetAction(p => logic.Unpark(p.GetValue(unparkRegNumOption)!));
            listSpecificGarage.SetAction(p => logic.ListSpecificGarageContents(p.GetValue(listSpecificGarageNameOption)));
  
            createGarage.SetAction(p =>
            {
                string? name = JoinArgs(p, garageNameOption);
                logic.CreateGarage(p.GetValue(garageTypeOption), p.GetValue(garageSizeOption), name);
            });

            parkCommand.SetAction(p =>
            {
                var filter = GetVehicleFilter(p, requireAll: true, parkRegNumOption, parkTypeOption, parkFuelOption, parkColorOption, parkUniqueOption, parkWheelsOption);
                if (filter == null) return;
                logic.ParkVehicle(filter, JoinArgs(p, parkGarageOption));
            });

            findCommand.SetAction(p =>
            {
                var filter = GetVehicleFilter(p, requireAll: false, findRegNumOption, findTypeOption, findFuelOption, findColorOption, findUniqueOption, findWheelsOption);
                if (filter == null) return;
                logic.Find(filter);
            });
            findCommand.SetAction(p =>
            {
                var reg = p.GetValue(findRegNumOption);
                var type = p.GetValue(findTypeOption);
                var fuel = p.GetValue(findFuelOption);
                var color = p.GetValue(findColorOption);
                var unique = p.GetValue(findUniqueOption);
                var wheels = p.GetValue(findWheelsOption);

                if (reg == null && type == null && fuel == null && color == null && unique == null && wheels == null)
                {
                    findCommand.Parse("--help").Invoke();
                    return;
                }

                var filter = GetVehicleFilter(p, requireAll: false, findRegNumOption, findTypeOption, findFuelOption, findColorOption, findUniqueOption, findWheelsOption);
                if (filter == null) return;
                logic.Find(filter);
            });


            Root.Add(demo);
            Root.Add(oneOfEachGarage);
            Root.Add(autoPopulate);

            Root.Add(createGarage);
            Root.Add(listAllGarages);
            Root.Add(listSpecificGarage);
            Root.Add(listGarageTypes);

            Root.Add(listAllVehicles);
            Root.Add(parkCommand);
            Root.Add(parkRandom);
            Root.Add(unparkCommand);
            Root.Add(unparkRandom);
            Root.Add(findCommand);

            Root.Add(exitCommand);
        }
        private string? JoinArgs(ParseResult p, Option<string[]> option) =>
    p.GetValue(option) is string[] arr ? string.Join(" ", arr) : null;


        private Filter? GetVehicleFilter(ParseResult p, bool requireAll,
    Option<string> regNum, Option<string> type, Option<string> fuel,
    Option<string> color, Option<string> unique, Option<int?> wheels)
        {
            if (!FilterFactory.TryCreate(requireAll, out Filter filter, out List<string> errors,
                p.GetValue(regNum), p.GetValue(type), p.GetValue(fuel),
                p.GetValue(color), p.GetValue(unique), p.GetValue(wheels)))
            {
                logic.PrintErrors(errors);
                return null;
            }
            return filter;
        }
    }
}