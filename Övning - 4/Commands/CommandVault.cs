using System.CommandLine;

namespace Övning___4.Commands
{
    public partial class CommandVault
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

            demo.SetAction(_ => { logic.CreateOneOfEachGarage(); logic.Autopopulate(); } )  ;
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
     
            createGarage.SetAction(OnCreateGarage);
    
            parkCommand.SetAction(OnPark);

            findCommand.SetAction(OnFind);

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
    }
}