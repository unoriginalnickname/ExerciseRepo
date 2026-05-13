using System.CommandLine;

namespace GaragePractice
{
    public class CommandVault
    {
        const int garageMinSize = 15;
        Garage garage;

        RootCommand root;
        Command listall = new("listall", "Lists all vehicles");
        Command findCommand = new("find", "--vehicletype --regnum --color --wheels --fuel" +
            "\nexample: find --fuel Gas --vehicletype Car");
        Command unparkCommand = new("unpark", "--regnum" +
            "\n example: unpark --regnum 123-ABC");
        Command parkCommand = new("park", "--regnum --vehicletype --color --wheels --fuel (ALL REQUIRED)" +
            "\n example: park --regnum 123-ABC --vehicletype car --color Red --wheels 2 --fuel Gas");
        Command exitCommand = new("exit");


        Option<string> registryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = false };
        Option<string> wheelsOption = new("--wheels") { HelpName = "Amount", Required = false }; //future implementations: list all the valid color, vehicle options
        Option<string> colorOption = new("--color") { HelpName = "Color", Required = false };       
        Option<string> vehicleTypeOption = new("--vehicletype") { HelpName = "Vehicletype", Required = false }; 
        Option<string> fuelTypeOption = new("--fuel") { HelpName = "Fuel type", Required = false };


        Option<string> parkRegistryNumberOption = new Option<string>("--regnum") { HelpName = "platenumber", Required = true };
        Option<string> parkWheelsOption = new("--wheels") { HelpName = "Amount", Required = true };
        Option<string> parkColorOption = new("--color") { HelpName = "Color", Required = true };       
        Option<string> parkVehicleType = new("--vehicletype") { HelpName = "Vehicletype", Required = true };  
        Option<string> parkFuelType = new("--fuel") { HelpName = "Fuel type", Required = true };

  
    

        public CommandVault()
        {
            root = new RootCommand("Usage examples:" +
                "\nfind --vehicletype Motorcycle --color pink" +
                "\nunpark --regnum ABC-123");
            InitializeCommands();
            SetupMenu();
           
        }

        void InitializeCommands()
        {
            parkCommand.Add(parkRegistryNumberOption); parkCommand.Add(parkVehicleType); parkCommand.Add(parkFuelType); parkCommand.Add(parkColorOption); parkCommand.Add(parkWheelsOption);

            findCommand.Add(registryNumberOption); findCommand.Add(vehicleTypeOption); findCommand.Add(wheelsOption); findCommand.Add(colorOption); findCommand.Add(fuelTypeOption);

            unparkCommand.Add(parkRegistryNumberOption);

            unparkCommand.SetAction(p => Unpark(p.GetValue(parkRegistryNumberOption)));


            listall.SetAction(_ => ListAll());
            findCommand.SetAction(p => Find(p.GetValue(wheelsOption), p.GetValue(colorOption), p.GetValue(vehicleTypeOption), p.GetValue(fuelTypeOption), p.GetValue(registryNumberOption)));

            parkCommand.SetAction(p => Park(p.GetValue(parkWheelsOption), p.GetValue(parkColorOption), p.GetValue(parkVehicleType), p.GetValue(parkFuelType), p.GetValue(parkRegistryNumberOption)));

            exitCommand.SetAction(p => run = false);

            //add the commands to the root command
            root.Add(listall);
            root.Add(findCommand);
            root.Add(unparkCommand);
            root.Add(parkCommand);
            root.Add(exitCommand);
        }
        void Park(string? wheels = null, string? color = null, string? vehicleType = null, string? fueltype = null, string? registryNumber = null)
        {
            if (registryNumber != null && FindVehiclesToDisplayModelArrayMethod(regNumber: registryNumber).Length == 0)
            {
                Vehicle? vehicle = (Vehicle?)Activator.CreateInstance(Type.GetType("GaragePractice." + vehicleType)!);
                if (vehicle != null)
                {
                    vehicle.RegistryNumber = registryNumber ?? "";
                    vehicle.Fueltype = fueltype ?? "";
                    vehicle.Color = color ?? "";
                    vehicle.NumWheels = wheels ?? "";

                    View.PrintString($"Vehicle has unique property, provide value for: {vehicle.GetUniquePropertyString()}");

                    vehicle.SetUniqueProperty(View.GetInput());
                    garage.ParkVehicle(vehicle);
                    View.PrintString("Parked. ");
                    return;
                }
                View.PrintString("Can't park here.");
            }
            else
            {
                View.PrintString("Can't park here.");
            }
        }

        VehicleDisplayModel[] FindVehiclesToDisplayModelArrayMethod(string? numWheels = null, string? color = null, string? vehicleType = null, string? fuelType = null, string? regNumber = null)
        {
            Vehicle[] vehicles = garage.GetAllVehiclesToArray();
            Vehicle[] realMatch = new Vehicle[vehicles.Length];
            Vehicle[] tempMatch = new Vehicle[vehicles.Length];

            //get whatever is matching
            for (int i = 0; i < vehicles.Length - 1; i++)
            {
                if (vehicles[i] != null)
                {
                    if (regNumber != null)
                        if (string.Equals(vehicles[i].RegistryNumber, regNumber, StringComparison.CurrentCultureIgnoreCase))
                            tempMatch[i] = vehicles[i];

                    if (numWheels != null)
                        if (string.Equals(vehicles[i].NumWheels, numWheels, StringComparison.CurrentCultureIgnoreCase))
                            tempMatch[i] = vehicles[i];

                    if (color != null)
                        if (string.Equals(vehicles[i].Color, color, StringComparison.OrdinalIgnoreCase))
                            tempMatch[i] = vehicles[i];

                    if (vehicleType != null)
                        if (string.Equals(vehicles[i].GetType().Name, vehicleType, StringComparison.CurrentCultureIgnoreCase))
                            tempMatch[i] = vehicles[i];

                    if (fuelType != null)
                        if (vehicles[i].Fueltype.ToString() == fuelType)
                            tempMatch[i] = vehicles[i];
                }
            }
            //remove what is not matching
            for (int i = 0; i < vehicles.Length - 1; i++)
            {
                if (vehicles[i] != null)
                {
                    if (regNumber != null)
                        if (!string.Equals(vehicles[i].RegistryNumber, regNumber, StringComparison.CurrentCultureIgnoreCase))
                            realMatch = RemoveNonMatchingFromArray(tempMatch, i);

                    if (numWheels != null)
                        if (!string.Equals(vehicles[i].NumWheels, numWheels, StringComparison.CurrentCultureIgnoreCase))
                            realMatch = RemoveNonMatchingFromArray(tempMatch, i);

                    if (color != null)
                        if (!string.Equals(vehicles[i].Color, color, StringComparison.OrdinalIgnoreCase))
                            realMatch = RemoveNonMatchingFromArray(tempMatch, i);

                    if (vehicleType != null)
                        if (!string.Equals(vehicles[i].GetType().Name, vehicleType, StringComparison.CurrentCultureIgnoreCase))
                            realMatch = RemoveNonMatchingFromArray(tempMatch, i);

                    if (fuelType != null)
                        if (vehicles[i].Fueltype.ToString() != fuelType)
                            realMatch = RemoveNonMatchingFromArray(tempMatch, i);
                }
            }
            return VehicleArrToDisplayArr(realMatch);
        }

        private Vehicle[] RemoveNonMatchingFromArray(Vehicle[] tempMatch, int indexToRemove)
        {
            if (tempMatch.Length < 2)
                return null;

            //make new array
            Vehicle[] finalArray = new Vehicle[tempMatch.Length - 1];

            //copy contents 
            int finalIndex = 0;
            for (int i = 0; i < tempMatch.Length; i++)
                if (i != indexToRemove) //except the one to remove
                    finalArray[finalIndex++] = tempMatch[i];

            return finalArray;
        }
        private VehicleDisplayModel[] VehicleArrToDisplayArr(Vehicle[] vehiclesMatching)
        {
            VehicleDisplayModel[] displayModelArray;
            int numVehiclesFound = 0;
            for (int i = 0; i < vehiclesMatching.Length - 1; i++)
            {
                if (vehiclesMatching[i] != null)
                {
                    numVehiclesFound++;
                }
            }
            //now we know the size we need
            displayModelArray = new VehicleDisplayModel[numVehiclesFound];

            int displayModelIndex = 0;
            for (int i = 0; i < vehiclesMatching.Length - 1; i++)
            {
                if (vehiclesMatching[i] != null)
                {
                    displayModelArray[displayModelIndex] = VehicleToDisplayModel(vehiclesMatching[i]);
                    displayModelIndex++;
                }
            }
            return displayModelArray;
        }

        private void ListAll()
        {
            View.PrintVehicles(VehicleArrToDisplayArr(garage.GetAllVehiclesToArray()));
        }


        void Find(string? wheels = null, string? color = null, string? vehicleType = null, string? fueltype = null, string? regNumber = null)
        {
            if (wheels == null && color == null && vehicleType == null && fueltype == null && regNumber == null)
                root.Parse("find --help").Invoke();
            else
                View.PrintVehicles(FindVehiclesToDisplayModelArrayMethod(wheels, color, vehicleType, fueltype, regNumber));
        }

        private void Unpark(string? regNumber)
        {
            if (regNumber != null)
            {
                View.PrintString(garage.UnParkVehicle(regNumber) ? "Car successfully unparked" : "Car was not unparked");
            }
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

        void SetupMenu()
        {
            int garageSize;

            View.PrintString($"Garage setup, enter garage size(max 100), or press enter for default size({garageMinSize})");

            bool parseSuccessful = int.TryParse(View.GetInput(), out garageSize);
            if (parseSuccessful)
            {
                garage = new(Math.Max(Math.Min(garageSize, 100), garageMinSize));
            }
            else
                garage = new();

            View.PrintString("\nAutopopulate garage with vehicles? Y/N");
            string input = View.GetInput();

            bool yes = string.Equals(input, "Y", StringComparison.InvariantCultureIgnoreCase);
            if (yes)
            {
                garage.AutoPopulateGarage();
                View.PrintString("\nGarage is now autopopulated.\n");
            }
        }

        private VehicleDisplayModel VehicleToDisplayModel(Vehicle vehicle)
        {
            VehicleDisplayModel model = new VehicleDisplayModel
            {
                VehicleType = vehicle.GetType().Name,
                RegPlateNumber = vehicle.RegistryNumber,
                Color = vehicle.Color,
                NumWheels = vehicle.NumWheels,
                Fueltype = vehicle.Fueltype.ToString(),
                UniqueProperties = vehicle.GetUniqueProperty()
            };
            return model;
        }
    }
}