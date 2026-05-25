using Övning___4.View;
using Övning___4.ViewModel;
using System.CommandLine;
using System.Globalization;
using System.Resources;

public class CommandRouter
{
    private readonly GarageManager garageManager;
    private readonly RootCommand root;
    private bool run = true;

    public CommandRouter(RootCommand root)
    {
        this.root = root;

        this.garageManager = new GarageManager();

    }
    public void Run()
    {
        root.Parse("--help").Invoke();
        while (run)
        {
            string input = ReadLine.Read("> ");
            ReadLine.AddHistory(input);
            root.Parse(input.Split()).Invoke();
        }
    }

    private void Handle(OperationResult result) => View.PrintString(result.Message);

    public void ParkRandom() => Handle(garageManager.ParkRandom());
    public void UnparkRandomVehicle() => Handle(garageManager.UnparkRandomVehicle());
    internal void CreateOneOfEachGarage() => Handle(garageManager.CreateOneOfEachGarage());
    internal void Autopopulate() => Handle(garageManager.AutoPopulateGarages());
    

    


    public void ListAll() => Handle(garageManager.ListAllVehicles());
    internal void Exit() => run = false;


    public void ListApprovedVehicleTypes() => Handle(garageManager.ListApprovedVehicleTypes());
 

    internal void ListAllGarages() => Handle(garageManager.ListAllGarages());

    public void Unpark(string regNumber) => Handle(garageManager.Unpark(regNumber)); // has input verification

    public void ParkVehicle(Filter filter, string? garageName) => Handle(garageManager.TryParkVehicle(filter, garageName));

    public void Find(Filter filter) => Handle(garageManager.ListVehicles(filter));

    internal void ListSpecificGarage(string? v) => Handle(garageManager.ListSpecificGarage(v));
    internal void CreateGarage(string? garageType, int? garageSize, string? garageName) => Handle(garageManager.TryCreateGarage(garageType, garageSize, garageName));

    internal void ListSpecificGarageContents(string[]? strings) => Handle(garageManager.ListSpecificGarage(string.Join(" ", strings)));

}