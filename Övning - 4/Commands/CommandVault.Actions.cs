using System.CommandLine;

namespace Övning___4.Commands
{

    public partial class CommandVault
    {
        private void OnPark(ParseResult p)
        {
            var filter = GetVehicleFilter(p, parkRegNumOption, parkTypeOption, parkFuelOption, parkColorOption, parkUniqueOption, parkWheelsOption);
            if (filter == null) return;
            logic.ParkVehicle(filter, JoinArgs(p, parkGarageOption));
        }

        private void OnFind(ParseResult p)
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

            var filter = GetVehicleFilter(p, findRegNumOption, findTypeOption, findFuelOption, findColorOption, findUniqueOption, findWheelsOption);
            if (filter == null) return;
            logic.Find(filter);
        }

        private void OnCreateGarage(ParseResult p)
        {
            string? name = JoinArgs(p, garageNameOption);
            logic.CreateGarage(p.GetValue(garageTypeOption), p.GetValue(garageSizeOption), name);
        }
    }
}

