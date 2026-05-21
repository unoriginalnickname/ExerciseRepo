using System.CommandLine;

namespace Övning___4.Commands
{
    public partial class CommandVault
    {
        // Garage options
        Option<string[]> garageNameOption = new("--name", "-n") { HelpName = "Garage name", Required = true, Arity = ArgumentArity.OneOrMore, AllowMultipleArgumentsPerToken = true };
        Option<string> garageTypeOption = new("--type", "-t") { HelpName = "Garage type", Required = true };
        Option<int> garageSizeOption = new("--size") { HelpName = "Garage size", Required = true };

        // Park options (required)
        Option<string> parkRegNumOption = new("--reg", "-r") { HelpName = "Plate number", Required = true };
        Option<int?> parkWheelsOption = new("--wheels", "-w") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color", "-c") { HelpName = "Color", Required = true };
        Option<string> parkTypeOption = new("--type", "-t") { HelpName = "Vehicle type", Required = true };
        Option<string> parkFuelOption = new("--fuel", "-f") { HelpName = "Fuel type", Required = true };
        Option<string> parkUniqueOption = new("--unique", "-u") { HelpName = "Unique", Required = true };
        Option<string[]> parkGarageOption = new("--garage", "-g") { HelpName = "Garage name", Required = false, Arity = new ArgumentArity(0, 10), AllowMultipleArgumentsPerToken = true };

        // Find options (optional)
        Option<string> findRegNumOption = new("--reg", "-r") { HelpName = "Plate number", Required = false };
        Option<int?> findWheelsOption = new("--wheels", "-w") { HelpName = "Amount", Required = false };
        Option<string> findColorOption = new("--color", "-c") { HelpName = "Color", Required = false };
        Option<string> findTypeOption = new("--type", "-t") { HelpName = "Vehicle type", Required = false };
        Option<string> findFuelOption = new("--fuel", "-f") { HelpName = "Fuel type", Required = false };
        Option<string> findUniqueOption = new("--unique", "-u") { HelpName = "Unique", Required = false };

        // Unpark option
        Option<string> unparkRegNumOption = new("--reg", "-r") { HelpName = "Plate number", Required = true };

        // List specific garage option
        Option<string[]> listSpecificGarageNameOption = new("--name", "-n") { HelpName = "Garage name", Required = true, Arity = ArgumentArity.OneOrMore, AllowMultipleArgumentsPerToken = true };

    }
}
