using GaragePractice;
using System.CommandLine;
using System.Text;

namespace Övning___4
{
    public class CommandLogic
    {
        public Garage<IVehicle> garage;
        RootCommand root;
        public CommandLogic(RootCommand root)
        {
            this.root = root;
            this.garage = new Garage<IVehicle>(SetupGarageSize(), true);
            SetupAutoPopulate(garage);
        }

        bool run = true;
        public void Run()
        {
            root.Parse("--help").Invoke();
            while (run)
            {
                var input = View.GetInput().Split();
                root.Parse(input).Invoke();
            }
        }

        public void Park(Filter filter)
        {
            if (DoesRegisterNumberExist(filter.RegNumber))
            {
                View.PrintString("This register number is already parked in the Garage. Calling security.");
                return;
            }
            if (!garage.VehicleIsApprovedType(filter.VehicleType))
            {
                View.PrintString("Vehicle is not of an approved type. See listapproved for approved vehicle types.");
                return;
            }

            IVehicle vehicle = (IVehicle)Activator.CreateInstance(Type.GetType("GaragePractice." + filter.VehicleType)!)!;
            if (vehicle != null)
            {
                vehicle.RegistryNumber = filter.RegNumber!;
                vehicle.Fueltype = filter.FuelType!;
                vehicle.Color = filter.Color!;
                vehicle.NumWheels = filter.NumWheels ?? 0;
                vehicle.UniquePropertyValue = filter.UniquePropertyValue!;
                garage.ParkVehicle(vehicle);
                View.PrintString("Parked. ");
            }
        }




        bool DoesRegisterNumberExist(string? regNumber)
        {

            if (!string.IsNullOrEmpty(regNumber))
            {
               return garage.Any(v => string.Equals(v.RegistryNumber, regNumber, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        public void Find(Filter filter)
        {
            List<IVehicle> vehicles = garage.Where(v => v != null && Matches(v, filter)).ToList();

            View.PrintVehicles(ConvertVehiclesToFilterArray(vehicles));
        }
        bool Matches(IVehicle vehicle, Filter filter)
        {
            if (filter.RegNumber != null)
                if (!string.Equals(vehicle.RegistryNumber, filter.RegNumber, StringComparison.OrdinalIgnoreCase))
                    return false;

            if (filter.NumWheels != null)
                if (vehicle.NumWheels == filter.NumWheels)
                    return false;

            if (filter.Color != null)
                if (!string.Equals(vehicle.Color, filter.Color, StringComparison.OrdinalIgnoreCase))
                    return false;

            if (filter.VehicleType != null)
                if (!string.Equals(vehicle.GetType().Name, filter.VehicleType, StringComparison.OrdinalIgnoreCase))
                    return false;

            if (filter.FuelType != null)
                if (!string.Equals(vehicle.Fueltype.ToString(), filter.FuelType, StringComparison.OrdinalIgnoreCase))
                    return false;

            return true;
        }


        private List<Filter> ConvertVehiclesToFilterArray(List<IVehicle> arr)
        {
            List<Filter> displayModelArray = new List<Filter>();
            foreach (var vehicle in arr)
            {
                displayModelArray.Add(ConvertVehicleToDisplay(vehicle));
            }

            return displayModelArray;
        }
        public void ListAll()
        {
            List<Filter> filters = new List<Filter>();

            List<IVehicle> vehicles = garage.ToList();

            filters = ConvertVehiclesToFilterArray(vehicles);

            View.PrintVehicles(filters);
        }

        public void Unpark(string? regNumber)
        {
            if (regNumber != null)
            {
                View.PrintString(garage.UnParkVehicle(regNumber) ? "Car successfully unparked" : "Car was not unparked");
            }
        }

        void SetupAutoPopulate(Garage<IVehicle> garage)
        {
            View.PrintString("\nAutopopulate garage with vehicles? Y/N");
            string input = View.GetInput();

            bool yes = string.Equals(input, "Y", StringComparison.InvariantCultureIgnoreCase);
            if (yes)
            {
                garage.AutoPopulateGarage();
                View.PrintString("\nGarage is now autopopulated.\n");
            }
        }
        int SetupGarageSize()
        {
            int garageSize;
            View.PrintString($"Garage setup, enter garage size(max 100), or press enter for default size 15");
            bool parseSuccessful = int.TryParse(View.GetInput(), out garageSize);
            if (parseSuccessful)
                return (Math.Max(Math.Min(garageSize, 100), 15));
            else return 15;
        }

        private Filter ConvertVehicleToDisplay(IVehicle vehicle)
        {
            Filter filter = new Filter
            {
                VehicleType = vehicle.GetType().Name,
                RegNumber = vehicle.RegistryNumber,
                Color = vehicle.Color,
                NumWheels = vehicle.NumWheels,
                FuelType = vehicle.Fueltype,
                UniquePropertyValue = vehicle.UniquePropertyValue,
                UniquePropertyString = vehicle.UniquePropertyString
            };
            return filter;
        }

        internal void Exit()
        {
            run = false;
        }

        internal void ListApprovedVehicleTypes()
        {
            StringBuilder sb = new StringBuilder();
            var approved = garage.ApprovedVehicleTypes.Keys.ToList();
            var unique = garage.ApprovedVehicleTypes.Values.ToList();

            sb.Append("\nApproved vehicletypes: \n\n");
            for (int i = 0; i < approved.Count; i++)
            {
                if (i != approved.Count - 1)
                    sb.Append($"{approved[i]}, unique: {unique[i]}. \n");
                else
                    sb.Append($"{approved[i]}, unique: {unique[i]}. ");
            }
            View.PrintString(sb.ToString());
        }
    }
}
