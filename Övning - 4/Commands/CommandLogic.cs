using GaragePractice;
using System.CommandLine;

public class CommandLogic
{
    private readonly GarageManager garageManager;
    private readonly RootCommand root;
    private bool run = true;

    public CommandLogic(RootCommand root)
    {
        this.root = root;

        int garageSize = AskGarageSize();
        this.garageManager = new GarageManager(garageSize);

        AskAutoPopulate();
    }

    private int AskGarageSize()
    {
        View.PrintString($"Garage setup, enter garage size(max 100), or press enter for default size 15");
        string input = View.GetInput();
        if (int.TryParse(input, out int size))
            return Math.Max(Math.Min(size, 100), 15); // clamp 15–100
        return 15; // default
    }

    public void Run()
    {
        root.Parse("--help").Invoke();
        while (run)
        {
            var input = View.GetInput().Split();
            root.Parse(input).Invoke();
        }
    }

    internal void Exit() => run = false;

    private void AskAutoPopulate()
    {
        View.PrintString("\nAutopopulate garage with vehicles? Y/N");
        if (string.Equals(View.GetInput(), "Y", StringComparison.InvariantCultureIgnoreCase))
        {
            garageManager.AutoPopulateGarage();
        }
    }

 
    public void ParkVehicle(Filter filter) => garageManager.ParkVehicle(filter);
    public void Unpark(string regNumber) => garageManager.Unpark(regNumber);
    public void ParkRandom() => garageManager.ParkRandom();
    public void UnparkRandomVehicle() => garageManager.UnparkRandomVehicle();
    public void ListAll() => garageManager.ListAll();
    public void Find(Filter filter) => garageManager.Find(filter);
    public void ListApprovedVehicleTypes() => garageManager.ListApprovedVehicleTypes();
}