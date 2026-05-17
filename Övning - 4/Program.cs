using GaragePractice;
using Övning___4.Commands;

IVehicle vehicle = new Car { RegistryNumber = "TEST-123", Color = "Red", FuelType = "Gas", NumWheels = 4, UniquePropertyValue = "4 doors" };

Console.WriteLine(vehicle == typeof(Car));

CommandVault vault = new CommandVault();