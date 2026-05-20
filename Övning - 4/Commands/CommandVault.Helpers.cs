using Övning___4.ViewModel;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

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
                logic.PrintErrors(errors);
                return null;
            }
            return filter;
        }
    }
}
