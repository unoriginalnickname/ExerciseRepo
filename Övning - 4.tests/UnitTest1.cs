using Övning___4.Misc;
using Övning___4.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class GarageManagerTests2
{


    // ─── Helpers ────────────────────────────────────────────────────────────

    private static GarageManager ManagerWithCarGarage(int size = 5, string name = "Test Cars")
    {
        var m = new GarageManager();
        m.TryCreateGarage("Car", size, name);
        return m;
    }

    private static Filter CarFilter(string reg = "ABC123") => new()
    {
        VehicleType = "Car",
        RegistryNumber = reg,
        Color = "Red",
        FuelType = "Petrol",
        NumWheels = 4,
        UniquePropertyValue = "4"
    };

    // ─── TryCreateGarage ───────────────────────────────────────────────────────

    [Fact]
    public void TryCreateGarage_ValidType_Succeeds()
    {
        var m = new GarageManager();
        var result = m.TryCreateGarage("Car", 10, "MyCars");
        Assert.True(result.Success);
    }

    [Theory] // — used inline when the data is short and specific to just that one test, like the normalisation cases.
    [InlineData("Car")]
    [InlineData("Bus")]
    [InlineData("Motorcycle")]
    [InlineData("Airplane")]
    [InlineData("Boat")]
    [InlineData("Ufo")]
    [InlineData("Uap")]
    public void TryCreateGarage_AllVehicleTypes_Succeed(string type)
    {
        var m = new GarageManager();
        var result = m.TryCreateGarage(type, 5, $"{type} Garage");
        Assert.True(result.Success);
    }

    [Fact]
    public void TryCreateGarage_UnknownType_Fails()
    {
        var m = new GarageManager();
        var result = m.TryCreateGarage("Hovercraft", 5, "Mystery");
        Assert.False(result.Success);
    }

    [Fact]
    public void TryCreateGarage_DuplicateName_Fails()
    {
        var m = new GarageManager();
        m.TryCreateGarage("Car", 5, "MyCars");
        var result = m.TryCreateGarage("Bus", 5, "MyCars");
        Assert.False(result.Success);
    }

    [Fact]
    public void TryCreateGarage_NameTooLong_Fails()
    {
        var m = new GarageManager();
        var result = m.TryCreateGarage("Car", 5, "ThisNameIsWayTooLongForAGarage");
        Assert.False(result.Success);
    }

    [Fact]
    public void TryCreateGarage_NullType_Fails()
    {
        var m = new GarageManager();
        var result = m.TryCreateGarage(null, 5, "SomeGarage");
        Assert.False(result.Success);
    }

    // ─── TryParkVehicle ─────────────────────────────────────────────────────

    [Fact]
    public void Park_ValidVehicle_Succeeds()
    {
        var m = ManagerWithCarGarage();
        var result = m.TryParkVehicle(CarFilter(), null);
        Assert.True(result.Success);
    }

    [Fact]
    public void Park_DuplicateRegNumber_Fails()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("XYZ999"), null);
        var result = m.TryParkVehicle(CarFilter("XYZ999"), null);
        Assert.False(result.Success);
    }

    [Fact]
    public void Park_RegNumberCaseInsensitive_DetectedAsDuplicate()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("abc123"), null);
        var result = m.TryParkVehicle(CarFilter("ABC123"), null);
        Assert.False(result.Success);
    }

    [Fact]
    public void Park_WrongTypeForGarage_Fails()
    {
        var m = new GarageManager();
        m.TryCreateGarage("Bus", 5, "Bus Garage");

        // Trying to park a Car in a Bus-only garage
        var result = m.TryParkVehicle(CarFilter(), null);
        Assert.False(result.Success);
    }

    [Fact]
    public void Park_FullGarage_Fails2()
    {
        var m = ManagerWithCarGarage(size: 1);
        m.TryParkVehicle(CarFilter("CAR001"), null);
        var result = m.TryParkVehicle(CarFilter("CAR002"), null);
        Assert.False(result.Success);
    }

    [Fact]
    public void Park_SpecificGarageName_ParksThere2()
    {
        var m = new GarageManager();
        m.TryCreateGarage("Car", 5, "Garage A");
        m.TryCreateGarage("Car", 5, "Garage B");

        var result = m.TryParkVehicle(CarFilter("AAA111"), "Garage A");
        Assert.True(result.Success);
        Assert.Contains("Garage A", result.Message);
    }

    [Fact]
    public void Park_NoGarages_Fails()
    {
        var m = new GarageManager();
        var result = m.TryParkVehicle(CarFilter(), null);
        Assert.False(result.Success);
    }

    // ─── Unpark ─────────────────────────────────────────────────────────────

    [Fact]
    public void Unpark_ExistingVehicle_Succeeds2()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("DEL001"), null);
        var result = m.Unpark("DEL001");
        Assert.True(result.Success);
    }

    [Fact]
    public void Unpark_NonExistentVehicle_Fails()
    {
        var m = ManagerWithCarGarage();
        var result = m.Unpark("GHOST01");
        Assert.False(result.Success);
    }

    [Fact]
    public void Unpark_CaseInsensitive_Succeeds()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("low123"), null);
        var result = m.Unpark("LOW123");
        Assert.True(result.Success);
    }

    [Fact]
    public void Unpark_EmptyRegNumber_Fails()
    {
        var m = ManagerWithCarGarage();
        var result = m.Unpark("");
        Assert.False(result.Success);
    }

    [Fact]
    public void Unpark_FreesSlot_AllowsReparking()
    {
        var m = ManagerWithCarGarage(size: 1);
        m.TryParkVehicle(CarFilter("FULL01"), null);
        m.Unpark("FULL01");
        var result = m.TryParkVehicle(CarFilter("NEW001"), null);
        Assert.True(result.Success);
    }

    // ─── ListVehicles (find/filter) ──────────────────────────────────────────

    [Fact]
    public void Find_ByColor_ReturnsMatch()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("RED001"), null);

        var result = m.ListVehicles(new Filter { Color = "Red" });
        Assert.True(result.Success);
        Assert.Contains("RED001", result.Message);
    }

    [Fact]
    public void Find_ByRegNumber_ReturnsExactMatch()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("FIND01"), null);
        m.TryParkVehicle(CarFilter("FIND02"), null);

        var result = m.ListVehicles(new Filter { RegistryNumber = "FIND01" });
        Assert.True(result.Success);
        Assert.Contains("FIND01", result.Message);
        Assert.DoesNotContain("FIND02", result.Message);
    }

    [Fact]
    public void Find_NoMatch_Fails()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter(), null);

        var result = m.ListVehicles(new Filter { Color = "Invisible" });
        Assert.False(result.Success);
    }

    [Fact]
    public void Find_ByWheels_ReturnsMatch()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("WHL001"), null);

        var result = m.ListVehicles(new Filter { NumWheels = 4 });
        Assert.True(result.Success);
        Assert.Contains("WHL001", result.Message);
    }

    [Fact]
    public void Find_ByFuelType_ReturnsMatch()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("FUEL01"), null);

        var result = m.ListVehicles(new Filter { FuelType = "Petrol" });
        Assert.True(result.Success);
        Assert.Contains("FUEL01", result.Message);
    }

    [Fact]
    public void Find_MultipleFilters_NarrowsResults()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("MATCH1"), null);
        m.TryParkVehicle(new Filter
        {
            VehicleType = "Car",
            RegistryNumber = "OTHER1",
            Color = "Blue",
            FuelType = "Petrol",
            NumWheels = 4
        }, null);

        var result = m.ListVehicles(new Filter { Color = "Red", FuelType = "Petrol" });
        Assert.True(result.Success);
        Assert.Contains("MATCH1", result.Message);
        Assert.DoesNotContain("OTHER1", result.Message);
    }

    // ─── ListAllVehicles ─────────────────────────────────────────────────────

    [Fact]
    public void ListAllVehicles_NoGarages_Fails()
    {
        var m = new GarageManager();
        Assert.False(m.ListAllVehicles().Success);
    }

    [Fact]
    public void ListAllVehicles_EmptyGarages_Fails()
    {
        var m = ManagerWithCarGarage();
        Assert.False(m.ListAllVehicles().Success);
    }

    [Fact]
    public void ListAllVehicles_WithVehicles_Succeeds()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("LIST01"), null);
        var result = m.ListAllVehicles();
        Assert.True(result.Success);
        Assert.Contains("LIST01", result.Message);
    }

    // ─── ListAllGarages ───────────────────────────────────────────────────────

    [Fact]
    public void ListAllGarages_NoGarages_Fails()
    {
        var m = new GarageManager();
        Assert.False(m.ListAllGarages().Success);
    }

    [Fact]
    public void ListAllGarages_AfterCreating_ShowsGarage()
    {
        var m = ManagerWithCarGarage(name: "ShowMe");
        var result = m.ListAllGarages();
        Assert.True(result.Success);
        Assert.Contains("ShowMe", result.Message);
    }

    // ─── Demo / Setup helpers ─────────────────────────────────────────────────

    [Fact]
    public void CreateOneOfEachGarage_Succeeds2()
    {
        var m = new GarageManager();
        var result = m.CreateOneOfEachGarage();
        Assert.True(result.Success);
    }

    [Fact]
    public void AutoPopulate_WithNoGarages_Fails()
    {
        var m = new GarageManager();
        Assert.False(m.AutoPopulateGarages().Success);
    }

    [Fact]
    public void AutoPopulate_FillsGarages2()
    {
        var m = new GarageManager();
        m.CreateOneOfEachGarage();
        var result = m.AutoPopulateGarages();
        Assert.True(result.Success);
        Assert.True(m.ListAllVehicles().Success);
    }

    [Fact]
    public void ParkRandom_NoGarages_Fails2()
    {
        var m = new GarageManager();
        Assert.False(m.ParkRandom().Success);
    }

    [Fact]
    public void ParkRandom_WithGarage_Succeeds()
    {
        var m = ManagerWithCarGarage();
        Assert.True(m.ParkRandom().Success);
    }

    [Fact]
    public void UnparkRandom_EmptyGarages_Fails()
    {
        var m = ManagerWithCarGarage();
        Assert.False(m.UnparkRandomVehicle().Success);
    }

    [Fact]
    public void UnparkRandom_WithVehicles_Succeeds2()
    {
        var m = ManagerWithCarGarage();
        m.TryParkVehicle(CarFilter("RND001"), null);
        Assert.True(m.UnparkRandomVehicle().Success);
    }
    private static Car MakeCar(string reg) => new()
    {
        RegistryNumber = reg,
        Color = "Black",
        FuelType = "Electric",
        NumWheels = 4
    };


    // ─── Garage<T> direct tests ──────────────────────────────────────────────────

    [Fact]
    public void NewGarage_HasAllSlotsFree()
    {
        var g = new Garage<Car>(5, "Test");
        Assert.Equal(5, g.NumFreeSlots);
        Assert.True(g.HasFreeSlots);
    }

    [Fact]
    public void Park_ReducesFreeSlots2()
    {
        var g = new Garage<Car>(5, "Test");
        g.ParkVehicle(MakeCar("A1"));
        Assert.Equal(4, g.NumFreeSlots);
    }

    [Fact]
    public void Unpark_IncreasesFreeSlots2()
    {
        var g = new Garage<Car>(5, "Test");
        g.ParkVehicle(MakeCar("A1"));
        g.Unpark("A1");
        Assert.Equal(5, g.NumFreeSlots);
    }

    [Fact]
    public void Unpark_NonExistent_Throws()
    {
        var g = new Garage<Car>(5, "Test");
        Assert.Throws<InvalidOperationException>(() => g.Unpark("NOPE"));
    }

    [Fact]
    public void ContainsVehicleRegNumber_CaseInsensitive()
    {
        var g = new Garage<Car>(5, "Test");
        g.ParkVehicle(MakeCar("abc123"));
        Assert.True(g.ContainsVehicleRegNumber("ABC123"));
    }

    [Fact]
    public void FullGarage_HasFreeSlots_IsFalse()
    {
        var g = new Garage<Car>(2, "Small");
        g.ParkVehicle(MakeCar("C1"));
        g.ParkVehicle(MakeCar("C2"));
        Assert.False(g.HasFreeSlots);
        Assert.Equal(0, g.NumFreeSlots);
    }

    [Fact]
    public void GetVehicles_ReturnsAllParked()
    {
        var g = new Garage<Car>(5, "Test");
        g.ParkVehicle(MakeCar("V1"));
        g.ParkVehicle(MakeCar("V2"));
        Assert.Equal(2, g.GetVehicles().Count());
    }
}

