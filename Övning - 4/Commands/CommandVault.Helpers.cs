using Övning___4.ViewModel;
using System.CommandLine;


namespace Övning___4.Commands
{
    public partial class CommandVault
    {
        private string? JoinArgs(ParseResult p, Option<string[]> option) =>
p.GetValue(option) is string[] arr ? string.Join(" ", arr) : null;


        private VehicleFilter? GetVehicleFilter(ParseResult p,
    Option<string> regNum, Option<string> type, Option<string> fuel,
    Option<string> color, Option<string> unique, Option<int?> wheels)
        {
            if (!FilterFactory.TryCreateFilter(out VehicleFilter filter,
                p.GetValue(regNum), p.GetValue(type), p.GetValue(fuel),
                p.GetValue(color), p.GetValue(unique), p.GetValue(wheels)).Success)
            {
                return null;
            }
            return filter;
        }
    }
}