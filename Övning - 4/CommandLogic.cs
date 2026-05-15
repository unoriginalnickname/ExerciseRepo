using GaragePractice;
using System.CommandLine;
using System.Text;

namespace Övning___4
{
    public class CommandLogic
    {
        public Garage garage;
        RootCommand root;
        public CommandLogic(RootCommand root)
        {
            this.root = root;
            this.garage = new Garage(SetupGarageSize());
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

            IVehicle vehicle = (IVehicle?)Activator.CreateInstance(Type.GetType("GaragePractice." + filter.VehicleType));
            if (vehicle != null)
            {
                vehicle.RegistryNumber = filter.RegNumber!;
                vehicle.Fueltype = filter.FuelType!;
                vehicle.Color = filter.Color!;
                vehicle.NumWheels = filter.NumWheels!;
                vehicle.UniqueProperty = filter.UniqueProperty!;
                garage.ParkVehicle(vehicle);
                View.PrintString("Parked. ");
                return;
            }
        }

        bool DoesRegisterNumberExist(string? regNumber)
        {
            IVehicle[] temp = garage.GetIVehicleArray();

            if (FilterVehicles
                (temp, new Filter() { RegNumber = regNumber }).Length == 0)
            {
                return false;
            }
            return true;
        }

        public void Find(Filter filter)
        {
            IVehicle[] filteredVehicles = FilterVehicles(garage.GetIVehicleArray(), filter);
            View.PrintVehicles(ConvertVehiclesToFilterArray(filteredVehicles));
        }
        bool Matches(IVehicle vehicle, Filter filter)
        {
            if (filter.RegNumber != null)
                if (!string.Equals(vehicle.RegistryNumber, filter.RegNumber, StringComparison.OrdinalIgnoreCase))
                    return false;

            if (filter.NumWheels != null)
                if (!string.Equals(vehicle.NumWheels, filter.NumWheels, StringComparison.OrdinalIgnoreCase))
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
        IVehicle[] FilterVehicles(IVehicle[] vehicleArr, Filter filter)
        {
            IVehicle[] temp = new IVehicle[vehicleArr.Length];
            int count = 0;

            for (int i = 0; i < vehicleArr.Length; i++)
            {
                if (vehicleArr[i] != null && Matches(vehicleArr[i], filter))
                {
                    temp[count++] = vehicleArr[i];
                }
            }
            IVehicle[] result = new IVehicle[count];
            Array.Copy(temp, result, count);
            return result;
        }
        IVehicle[]? RemoveNullFromVehicleArray(IVehicle[] arr)
        {
            int numberOfVehicles = garage.GetNumberOfVehiclesInGarage();
            IVehicle[] result = new IVehicle[numberOfVehicles];
            int count = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null)
                {
                    result[count++] = arr[i];
                }
            }
            return result;
        }
        private Filter[]? ConvertVehiclesToFilterArray(IVehicle[]? arr)
        {
            if (arr != null)
            {
                Filter[] displayModelArray = new Filter[arr.Length];

                int displayModelIndex = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    displayModelArray[displayModelIndex] = ConvertVehicleToDisplay(arr[i]);
                    displayModelIndex++;
                }
                return displayModelArray;
            }
            else return null;
        }
        public void ListAll()
        {
            View.PrintVehicles(ConvertVehiclesToFilterArray(RemoveNullFromVehicleArray(garage.GetIVehicleArray())));
        }

        public void Unpark(string? regNumber)
        {
            if (regNumber != null)
            {
                View.PrintString(garage.UnParkVehicle(regNumber) ? "Car successfully unparked" : "Car was not unparked");
            }
        }

        void SetupAutoPopulate(Garage garage)
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
                UniqueProperty = vehicle.UniqueProperty
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
            string[,] approved = garage.ApprovedVehicleTypes;

            sb.Append("\nApproved vehicletypes: \n\n");
            for (int i = 0; i < approved.GetLength(0); i++)
            {
                if (i != approved.Length - 1)
                    sb.Append($"{approved[i, 0]}, unique: {approved[i, 1]}. \n");
                else
                    sb.Append($"{approved[i, 0]}, unique: {approved[i, 1]}. ");
            }
            View.PrintString(sb.ToString());
        }
    }
}
