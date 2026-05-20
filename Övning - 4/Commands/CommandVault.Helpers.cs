using Övning___4.ViewModel;
using Övning___4.View;
using System.CommandLine;


namespace Övning___4.Commands
{
    public partial class CommandVault
    {
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
                View.View.PrintString(string.Join("\n", errors));
                return null;
            }
            return filter;
        }
    }
}
