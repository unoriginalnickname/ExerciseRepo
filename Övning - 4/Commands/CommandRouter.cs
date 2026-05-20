using Övning___4.View;
using Övning___4.ViewModel;
using System.CommandLine;

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

    internal void Exit() => run = false;

    public void PrintErrors(IEnumerable<string> error) => View.PrintIEnumerable(error);

    public void ParkVehicle(Filter filter, string? garageName) => garageManager.TryParkVehicle(filter, garageName);
    public void Unpark(string regNumber) => garageManager.Unpark(regNumber);
    public void ParkRandom() => garageManager.ParkRandom();
    public void UnparkRandomVehicle() => garageManager.UnparkRandomVehicle();
    public void ListAll() => garageManager.ListAllVehicles();
    public void Find(Filter filter) => garageManager.Find(filter);
    public void ListApprovedVehicleTypes() => garageManager.ListApprovedVehicleTypes();
    internal void ListSpecificGarage(string? v) => garageManager.ListSpecificGarage(v);
    internal void Demo() => garageManager.Demo();
    internal void ListAllGarages() => garageManager.ListAllGarages();
    internal void CreateGarage(string? garageType, int? garageSize, string? garageName) => garageManager.CreateGarage(garageType, garageSize, garageName);

    internal void ListSpecificGarageContents(string[]? strings) => garageManager.ListSpecificGarage(string.Join(" ", strings));
    internal void CreateOneOfEachGarage() => garageManager.CreateOneOfEachGarage();
    internal void Autopopulate() => garageManager.AutoPopulateGarages();
}