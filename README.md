# Garage Manager

A console-based garage management application written in C# (.NET 10). It lets you create typed garages, park and unpark vehicles, and search your fleet — all from an interactive command-line interface.

## Features

- **Typed garages** — each garage holds one specific vehicle type (Car, Bus, Motorcycle, Airplane, Boat, UFO, UAP)
- **Park & unpark** vehicles by registration number
- **Search / filter** across all garages by type, colour, fuel, wheels, registration number, or a vehicle-specific property
- **Random helpers** — quickly populate garages or park/unpark random vehicles for testing
- **Command history** — navigate previous commands with the arrow keys (powered by ReadLine)
- **Extensible vehicle registry** — new vehicle types can be added in `VehicleTypeRegistry.cs`

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Getting Started

```bash
git clone https://github.com/unoriginalnickname/ExerciseRepo.git
cd "ExerciseRepo/Övning - 4"
dotnet run
```

On startup the help menu is printed automatically. Type any command followed by `--help` to see its options.

## Commands

### Setup

| Command | Description |
|---|---|
| `demo` | Creates one garage of each type and fills them with random vehicles |
| `oneofeach` | Creates one empty garage of each vehicle type |
| `autopopulate` | Fills existing garages with random vehicles |

### Garages

| Command | Options | Description |
|---|---|---|
| `creategarage` | `--type`, `--name`, `--size` | Creates a new garage for the given vehicle type |
| `listallgarages` | — | Lists all garages with occupancy |
| `listgaragecontents` | `--name` | Lists all vehicles in a specific garage |
| `listgaragetypes` | — | Shows all supported vehicle types |

### Vehicles

| Command | Options | Description |
|---|---|---|
| `park` | `--reg`, `--type`, `--color`, `--fuel`, `--wheels`, `--unique`, `--garage` | Parks a vehicle |
| `parkrandom` | — | Parks a randomly generated vehicle |
| `unpark` | `--reg` | Unparks a vehicle by registration number |
| `unparkrandom` | — | Unparks a random vehicle |
| `listallvehicles` | — | Lists every vehicle across all garages |
| `find` | `--reg`, `--type`, `--color`, `--fuel`, `--wheels`, `--unique` | Filters vehicles by one or more criteria |

### App

| Command | Description |
|---|---|
| `exit` | Exits the application |

## Vehicle Types

Each vehicle type has a shared set of properties (registration number, colour, fuel type, number of wheels) plus one type-specific property:

| Type | Unique Property |
|---|---|
| Airplane | Wing span (m) |
| Boat | Hull length (m) |
| Bus | Number of stops |
| Car | Number of doors |
| Motorcycle | Engine size (cc) |
| UFO | Abduction capacity |
| UAP | Classified |

## Project Structure

```
Övning - 4/
├── Commands/
│   ├── CommandVault.cs           # Command definitions & initialization
│   ├── CommandVault.Actions.cs   # Action handlers (park, find, creategarage)
│   ├── CommandVault.Helpers.cs   # Shared option/filter helpers
│   ├── CommandVault.Options.cs   # CLI option definitions
│   └── CommandRouter.cs          # Bridges commands to GarageManager
├── Managers/
│   ├── GarageManager.cs          # Core logic: create garage, park, unpark
│   ├── GarageManager.Helpers.cs  # Internal helpers & normalization
│   ├── GarageManager.ListMethods.cs # Listing & filtering logic
│   └── GarageManager.DemoFeatures.cs # Random population & demo setup
├── Misc/
│   ├── IVehicle.cs               # Vehicle interface
│   ├── IGarage.cs                # Garage interface
│   ├── Garage.cs                 # Generic Garage<T> implementation
│   ├── Vehicles.cs               # Concrete vehicle classes
│   ├── VehicleFactory.cs         # Creates IVehicle instances from a Filter
│   ├── VehicleTypeRegistry.cs    # Registry of approved vehicle types
│   └── RandomHelper.cs           # Random vehicle/data generation
├── View/
│   └── View.cs                   # Console output helpers
├── ViewModel/
│   ├── Filter.cs                 # Shared filter/display model
│   ├── FilterFactory.cs          # Converts IVehicle ↔ Filter
│   └── OperationResult.cs        # Result wrapper (Ok/Fail + message)
└── Program.cs                    # Entry point
```

## Dependencies

| Package | Version | Purpose |
|---|---|---|
| `System.CommandLine` | 2.0.7 | CLI argument parsing |
| `ReadLine` | 2.0.1 | Command history & line editing |
