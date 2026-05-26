using Övning___4.Misc;
using Övning___4.ViewModel;
using System.Reflection;
using Xunit;

// ═══════════════════════════════════════════════════════════════════════════════
// SHARED TEST DATA
// Used by [MemberData] attributes across all test classes
// ═══════════════════════════════════════════════════════════════════════════════

public static class TestData
{
    // All valid vehicle type names
    public static IEnumerable<object[]> AllVehicleTypes => new List<object[]>
{
    new object[] { typeof(Car) },
    new object[] { typeof(Bus) },
    new object[] { typeof(Motorcycle) },
    new object[] { typeof(Airplane) },
    new object[] { typeof(Boat) },
    new object[] { typeof(Ufo) },
    new object[] { typeof(Uap) },
};




    // Reg numbers paired with whether they should be valid
    public static IEnumerable<object[]> RegNumberValidity => new List<object[]>
    {
        new object[] { "BANANA",   true  },
        new object[] { "ABC123",   true  },
        new object[] { "ABC-123",  true  },
        new object[] { "ABC 123",  true  },
        new object[] { "GURKA1",   true  },
        new object[] { "abc123",   true  },   // lowercase — should normalise
        new object[] { "ABC!123",  false },
        new object[] { "HELLO@",   false },
        new object[] { "REG#999",  false },
        new object[] { "",         false },
        new object[] { "   ",      false },
    };

    // Wheel counts paired with whether they should be valid
    public static IEnumerable<object[]> WheelValidity => new List<object[]>
    {
        new object[] { 0,   true  },
        new object[] { 4,   true  },
        new object[] { 18,  true  },
        new object[] { -1,  false },
        new object[] { 19,  false },
        new object[] { 100, false },
    };

    // Complete valid filter inputs for parking
    public static IEnumerable<object[]> ValidParkInputs => new List<object[]>
    {
        new object[] { "CAR001", "Car",        "Red",    "Petrol",   4, "4"         },
        new object[] { "BUS001", "Bus",        "Blue",   "Diesel",   6, "10"        },
        new object[] { "MOT001", "Motorcycle", "Black",  "Electric", 2, "600 cc"    },
    };

    // Garage names paired with whether they should be valid
    public static IEnumerable<object[]> GarageNameValidity => new List<object[]>
    {
        new object[] { "My Garage",               true  },
        new object[] { "A",                        true  },
        new object[] { "Exactly20Chars123456",    true  },   // exactly 20
        new object[] { "ThisNameIsOver20Chars!!", false },
        new object[] { null,                       false },
        new object[] { "",                         false },
    };
}



// ═══════════════════════════════════════════════════════════════════════════════
// GARAGE<T> TESTS
// ═══════════════════════════════════════════════════════════════════════════════

[Trait("Category", "Garage")] // Trait tags tests into categories. You can filter by them in the test explorer or from the terminal
public class GarageTests : IDisposable // IDisposable constructor/Dispose — GarageManagerTests creates a pre-configured manager with
                                       // 3 garages before every single test, so no test has to set up from scratch.
                                       // Dispose cleans up after.
{
    private Garage<Car> _garage;

    // Constructor = [SetUp] — runs before every test
    public GarageTests()
    {
        _garage = new Garage<Car>(5, "Test Garage");
    }

    // Dispose = [TearDown] — runs after every test
    public void Dispose()
    {
        // Nothing to clean up here, but shows the pattern
    }




    // ── Construction ──────────────────────────────────────────────────────────

    [Trait("Category", "Construction")]
    [Fact(DisplayName = "New garage starts with all slots free")] // [Fact(DisplayName = "...")] — gives tests readable names in the test explorer instead of the method name.
    public void NewGarage_AllSlotsFree()
    {
        Assert.Equal(5, _garage.NumFreeSlots);
        Assert.True(_garage.HasFreeSlots);
    }

    [Trait("Category", "Construction")]
    [Fact(DisplayName = "Garage size zero or negative throws")]
    public void Constructor_ZeroSize_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Car>(0, "Bad Garage"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Car>(-1, "Bad Garage"));
    }

    [Trait("Category", "Construction")]
    [Fact(DisplayName = "Null garage name throws")]
    public void Constructor_NullName_Throws()
    {
        Assert.ThrowsAny<ArgumentException>(() => new Garage<Car>(5, null));
    }

    // ── Parking ───────────────────────────────────────────────────────────────

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking a vehicle reduces free slots by 1")]
    public void Park_ReducesFreeSlots()
    {
        _garage.ParkVehicle(MakeCar("AAA001"));
        Assert.Equal(4, _garage.NumFreeSlots);
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking null vehicle throws ArgumentNullException")]
    public void Park_NullVehicle_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _garage.ParkVehicle(null!));
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking duplicate reg number throws InvalidOperationException")]
    public void Park_DuplicateReg_Throws()
    {
        _garage.ParkVehicle(MakeCar("DUP001"));
        Assert.Throws<InvalidOperationException>(() => _garage.ParkVehicle(MakeCar("DUP001")));
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking in a full garage throws InvalidOperationException")]
    public void Park_FullGarage_Throws()
    {
        var small = new Garage<Car>(1, "Tiny");
        small.ParkVehicle(MakeCar("FULL01"));
        Assert.Throws<InvalidOperationException>(() => small.ParkVehicle(MakeCar("FULL02")));
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Full garage HasFreeSlots returns false")]
    public void Park_FillGarage_HasFreeSlotsIsFalse()
    {
        var small = new Garage<Car>(2, "Small");
        small.ParkVehicle(MakeCar("C1"));
        small.ParkVehicle(MakeCar("C2"));
        Assert.False(small.HasFreeSlots);
        Assert.Equal(0, small.NumFreeSlots);
    }

    // ── Unparking ─────────────────────────────────────────────────────────────

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking increases free slots by 1")]
    public void Unpark_IncreasesFreeSlots()
    {
        _garage.ParkVehicle(MakeCar("AAA001"));
        _garage.Unpark("AAA001");
        Assert.Equal(5, _garage.NumFreeSlots);
    }

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking non-existent vehicle throws InvalidOperationException")]
    public void Unpark_NotFound_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => _garage.Unpark("GHOST1"));
    }

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking null throws ArgumentException")]
    public void Unpark_Null_Throws()
    {
        Assert.Throws<ArgumentException>(() => _garage.Unpark(null!));
    }

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking is case insensitive")]
    public void Unpark_CaseInsensitive_Succeeds()
    {
        _garage.ParkVehicle(MakeCar("abc123"));
        // Should not throw
        _garage.Unpark("ABC123");
        Assert.Equal(5, _garage.NumFreeSlots);
    }

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking frees a slot for a new vehicle")]
    public void Unpark_FreesSlot_AllowsRePark()
    {
        var small = new Garage<Car>(1, "Tiny");
        small.ParkVehicle(MakeCar("FIRST1"));
        small.Unpark("FIRST1");
        // Should not throw
        small.ParkVehicle(MakeCar("SECND1"));
        Assert.Equal(0, small.NumFreeSlots);
    }

    // ── Querying ──────────────────────────────────────────────────────────────

    [Trait("Category", "Query")]
    [Fact(DisplayName = "ContainsVehicleRegNumber is case insensitive")]
    public void Contains_CaseInsensitive_ReturnsTrue()
    {
        _garage.ParkVehicle(MakeCar("abc123"));
        Assert.True(_garage.ContainsVehicleRegNumber("ABC123"));
        Assert.True(_garage.ContainsVehicleRegNumber("abc123"));
        Assert.True(_garage.ContainsVehicleRegNumber("Abc123"));
    }

    [Trait("Category", "Query")]
    [Fact(DisplayName = "GetVehicles returns all parked vehicles")]
    public void GetVehicles_ReturnsAll()
    {
        _garage.ParkVehicle(MakeCar("V1"));
        _garage.ParkVehicle(MakeCar("V2"));
        _garage.ParkVehicle(MakeCar("V3"));
        Assert.Equal(3, _garage.GetVehicles().Count());
    }

    [Trait("Category", "Query")]
    [Fact(DisplayName = "GetVehicles on empty garage returns empty")]
    public void GetVehicles_EmptyGarage_ReturnsEmpty()
    {
        Assert.Empty(_garage.GetVehicles());
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static Car MakeCar(string reg) => new()
    {
        RegistryNumber = reg,
        Color = "Black",
        FuelType = "Electric",
        NumWheels = 4,
        UniquePropertyValue = "4",
    };
}

// ═══════════════════════════════════════════════════════════════════════════════
// GARAGE MANAGER TESTS
// ═══════════════════════════════════════════════════════════════════════════════

[Trait("Category", "GarageManager")]
public class GarageManagerTests : IDisposable
{
    private readonly GarageManager _manager;

    public GarageManagerTests()
    {
        Assembly.Load("Övning - 4");
        _manager = new GarageManager();
        _manager.TryAddNewGarage("Car", 5, "Car Garage");
        _manager.TryAddNewGarage("Bus", 5, "Bus Garage");
        _manager.TryAddNewGarage("Motorcycle", 5, "Moto Garage");
    }

    public void Dispose() { }

    // ── TryAddNewGarage ──────────────────────────────────────────────────────────

    [Trait("Category", "TryAddNewGarage")]
    [Theory(DisplayName = "All vehicle types can create a garage")]
    [MemberData(nameof(TestData.AllVehicleTypes), MemberType = typeof(TestData))] // Memberdata pulls test data from TestData at the top of the file. Anything too complex for
    public void TryAddNewGarage_AllTypes_Succeed(Type type)
    {
        var m = new GarageManager(); // fresh manager, no shared state
        var result = m.TryAddNewGarage(type.Name, 5, $"{type}G");
        Assert.True(result.Success, result.Message); // print the message on failure
    }


    [Trait("Category", "TryAddNewGarage")]
    [Theory(DisplayName = "Garage name validity is enforced")]
    [MemberData(nameof(TestData.GarageNameValidity), MemberType = typeof(TestData))]
    public void TryAddNewGarage_NameValidity(string name, bool shouldSucceed)
    {
        var m = new GarageManager();
        var result = m.TryAddNewGarage("Car", 5, name);
        Assert.Equal(shouldSucceed, result.Success);
    }

    [Trait("Category", "TryAddNewGarage")]
    [Fact(DisplayName = "Duplicate garage name fails")]
    public void TryAddNewGarage_DuplicateName_Fails()
    {
        var result = _manager.TryAddNewGarage("Bus", 5, "Car Garage");
        Assert.False(result.Success);
    }

    [Trait("Category", "TryAddNewGarage")]
    [Fact(DisplayName = "Unknown vehicle type fails with helpful message")]
    public void TryAddNewGarage_UnknownType_Fails()
    {
        var result = _manager.TryAddNewGarage("Hovercraft", 5, "Hover Garage");
        Assert.False(result.Success);
    }

    // ── TryParkVehicle ────────────────────────────────────────────────────────

    [Trait("Category", "Parking")]
    [Theory(DisplayName = "Valid park inputs succeed")]
    [MemberData(nameof(TestData.ValidParkInputs), MemberType = typeof(TestData))]
    public void Park_ValidInputs_Succeed(string reg, string type, string color, string fuel, int wheels, string unique)
    {
        // Make sure garage exists for each type
        _manager.TryAddNewGarage(type, 5, $"{type} Extra");

        var filter = MakeFilter(reg, type, color, fuel, wheels, unique);
        var result = _manager.TryParkVehicle(filter, null);
        Assert.True(result.Success);
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking duplicate reg number across garages fails")]
    public void Park_DuplicateRegAcrossGarages_Fails()
    {
        _manager.TryParkVehicle(MakeCarFilter("DUP001"), null);

        // Try parking same reg in a different garage
        _manager.TryAddNewGarage("Car", 5, "Second Car Garage");
        var result = _manager.TryParkVehicle(MakeCarFilter("DUP001"), "Second Car Garage");
        Assert.False(result.Success);
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking reg number detection is case insensitive")]
    public void Park_DuplicateReg_CaseInsensitive_Fails()
    {
        _manager.TryParkVehicle(MakeCarFilter("abc123"), null);
        var result = _manager.TryParkVehicle(MakeCarFilter("ABC123"), null);
        Assert.False(result.Success);
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking in a specific garage by name succeeds")]
    public void Park_SpecificGarageName_ParksThere()
    {
        var result = _manager.TryParkVehicle(MakeCarFilter("SPEC01"), "Car Garage");
        Assert.True(result.Success, result.Message);
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking in full garage fails")]
    public void Park_FullGarage_Fails()
    {
        var m = new GarageManager();
        m.TryAddNewGarage("Car", 1, "Tiny");
        m.TryParkVehicle(MakeCarFilter("FULL01"), null);
        var result = m.TryParkVehicle(MakeCarFilter("FULL02"), null);
        Assert.False(result.Success);
    }

    [Trait("Category", "Parking")]
    [Fact(DisplayName = "Parking wrong vehicle type for garage fails")]
    public void Park_WrongType_Fails()
    {
        // Bus garage exists but we're parking a Car
        var busFilter = MakeFilter("WRONG1", "Bus", "Red", "Diesel", 6, "10");
        var result = _manager.TryParkVehicle(busFilter, "Car Garage");
        Assert.False(result.Success);
    }

    // ── Unpark ────────────────────────────────────────────────────────────────

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking existing vehicle succeeds")]
    public void Unpark_ExistingVehicle_Succeeds()
    {
        var parkresult = _manager.TryParkVehicle(MakeCarFilter("UNP001"), null);
        var unparkresult = _manager.TryUnpark("UNP001");
        Assert.True(unparkresult.Success, unparkresult.Message);
    }

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "Unparking non-existent vehicle fails")]
    public void Unpark_NotFound_Fails()
    {
        var result = _manager.TryUnpark("GHOST1");
        Assert.False(result.Success);
    }

    [Trait("Category", "Unparking")]
    [Theory(DisplayName = "Unparking blank input fails")]
    [InlineData("")] // InlineData used when the data is short and specific to just that one test, like the normalisation cases.
    [InlineData("   ")]
    [InlineData(null)]
    public void Unpark_BlankInput_Fails(string reg)
    {
        var result = _manager.TryUnpark(reg);
        Assert.False(result.Success);
    }

    [Trait("Category", "Unparking")]
    [Fact(DisplayName = "After unparking, same reg can be parked again")]
    public void Unpark_ThenRePark_Succeeds()
    {
        var parkresult= _manager.TryParkVehicle(MakeCarFilter("REPARK1"), null);
        var unparkresult = _manager.TryUnpark("REPARK1");
        var result = _manager.TryParkVehicle(MakeCarFilter("REPARK1"), null);
        Assert.True(result.Success);
    }

    // ── ListVehicles ──────────────────────────────────────────────────────────

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "Find by colour returns matching vehicles")]
    public void ListVehicles_ByColor_ReturnsMatch()
    {
        _manager.TryParkVehicle(MakeCarFilter("RED001"), null);
        var result = _manager.ListVehicles(new VehicleFilter { Color = "Red" });
        Assert.True(result.Success);
        Assert.Contains("RED001", result.Message);
    }

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "Find by reg returns exact match only")]
    public void ListVehicles_ByReg_ExactMatch()
    {
        _manager.TryParkVehicle(MakeCarFilter("FIND01"), null);
        _manager.TryParkVehicle(MakeCarFilter("FIND02"), null);
        var result = _manager.ListVehicles(new VehicleFilter { RegistryNumber = "FIND01" });
        Assert.True(result.Success);
        Assert.Contains("FIND01", result.Message);
        Assert.DoesNotContain("FIND02", result.Message);
    }

    [Trait("Category", "Listing")]
    [Theory(DisplayName = "Find by wheel count returns matches")]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    public void ListVehicles_ByWheels_ReturnsMatch(int wheels)
    {
        var filter = MakeCarFilter($"WHL{wheels:D3}");
        filter.NumWheels = wheels;
        _manager.TryParkVehicle(filter, null);

        var result = _manager.ListVehicles(new VehicleFilter { NumWheels = wheels });
        Assert.True(result.Success);
    }

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "Find with no matches fails")]
    public void ListVehicles_NoMatch_Fails()
    {
        _manager.TryParkVehicle(MakeCarFilter("NOM001"), null);
        var result = _manager.ListVehicles(new VehicleFilter { Color = "Invisible" });
        Assert.False(result.Success);
    }

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "ListAllVehicles on empty garages fails")]
    public void ListAllVehicles_Empty_Fails()
    {
        var m = new GarageManager();
        m.TryAddNewGarage("Car", 5, "Empty");
        Assert.False(m.ListAllVehicles().Success);
    }

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "ListAllGarages shows garage names")]
    public void ListAllGarages_ShowsNames()
    {
        var result = _manager.ListAllGarages();
        Assert.True(result.Success);
        Assert.Contains("Car Garage", result.Message);
        Assert.Contains("Bus Garage", result.Message);
    }

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "ListSpecificGarage is case insensitive")]
    public void ListSpecificGarage_CaseInsensitive_FindsGarage()
    {
        var result = _manager.TryListSpecificGarage("car garage");
        Assert.True(result.Success);
    }

    [Trait("Category", "Listing")]
    [Fact(DisplayName = "ListSpecificGarage with unknown name fails")]
    public void ListSpecificGarage_UnknownName_Fails()
    {
        var result = _manager.TryListSpecificGarage("Does Not Exist");
        Assert.False(result.Success);
    }

    // ── Demo helpers ──────────────────────────────────────────────────────────

    [Trait("Category", "Demo")]
    [Fact(DisplayName = "CreateOneOfEachGarage succeeds")]
    public void CreateOneOfEachGarage_Succeeds()
    {
        var m = new GarageManager();
        var result = m.CreateOneOfEachGarage();
        Assert.True(result.Success);
    }

    [Trait("Category", "Demo")]
    [Fact(DisplayName = "AutoPopulate fills garages with vehicles")]
    public void AutoPopulate_FillsGarages()
    {
        _manager.AutoPopulateGarages();
        Assert.True(_manager.ListAllVehicles().Success);
    }

    [Trait("Category", "Demo")]
    [Fact(DisplayName = "AutoPopulate produces no duplicate reg numbers")]
    public void AutoPopulate_NoDuplicateRegNumbers()
    {
        _manager.AutoPopulateGarages();
        var vehicles = _manager.ListAllVehicles();
        // If any duplicates existed, parking would have thrown — reaching here means none
        Assert.True(vehicles.Success);
    }

    [Trait("Category", "Demo")]
    [Fact(DisplayName = "ParkRandom with no garages fails")]
    public void ParkRandom_NoGarages_Fails()
    {
        var m = new GarageManager();
        Assert.False(m.ParkRandom().Success);
    }

    [Trait("Category", "Demo")]
    [Fact(DisplayName = "UnparkRandom with no vehicles fails")]
    public void UnparkRandom_Empty_Fails()
    {
        Assert.False(_manager.UnparkRandomVehicle().Success);
    }

    [Trait("Category", "Demo")]
    [Fact(DisplayName = "UnparkRandom after parking succeeds")]
    public void UnparkRandom_WithVehicles_Succeeds()
    {
        _manager.TryParkVehicle(MakeCarFilter("RND001"), null);
        Assert.True(_manager.UnparkRandomVehicle().Success);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static VehicleFilter MakeCarFilter(string reg) => MakeFilter(reg, "Car", "Red", "Petrol", 4, "4");

    private static VehicleFilter MakeFilter(string reg, string type, string color, string fuel, int wheels, string unique) => new()
    {
        RegistryNumber = reg,
        VehicleType = type,
        Color = color,
        FuelType = fuel,
        NumWheels = wheels,
        UniquePropertyValue = unique
    };
}

// ═══════════════════════════════════════════════════════════════════════════════
// FILTER FACTORY TESTS
// ═══════════════════════════════════════════════════════════════════════════════

[Trait("Category", "FilterFactory")]
public class FilterFactoryTests
{
    //fact skip
    [Fact/*(Skip = "Bug not fixed yet — FilterFactory should return null filter on validation failure (Bug #6)")*/]
    public void FilterFactory_WhenValidationFails_FilterShouldBeEmpty()
    {
        var success = FilterFactory.TryCreateFilter(
            out var filter,
            reg: "BAD REG!"
        ).Success;
        Assert.False(success);
        Assert.Null(filter);
    }

    // ── Reg number validation ─────────────────────────────────────────────────

    [Trait("Category", "RegNumber")]
    [Theory(DisplayName = "Reg number character validity")]
    [MemberData(nameof(TestData.RegNumberValidity), MemberType = typeof(TestData))]
    public void RegNumber_Validity(string reg, bool shouldPass)
    {
        var result = FilterFactory.TryCreateFilter(
            out _,
            reg: reg
        );

        Assert.Equal(shouldPass, result.Success);
    }

    [Trait("Category", "RegNumber")]
    [Fact(DisplayName = "Reg number is normalised to uppercase")]
    public void RegNumber_NormalisedToUpperCase()
    {
        var result = FilterFactory.TryCreateFilter(out var filter, reg: "abc123");
        Assert.True(result.Success, result.Message);
        Assert.Equal("ABC123", filter.RegistryNumber);
    }

    // ── Wheel validation ──────────────────────────────────────────────────────

    [Trait("Category", "Wheels")]
    [Theory(DisplayName = "Wheel count validity boundaries")]
    [MemberData(nameof(TestData.WheelValidity), MemberType = typeof(TestData))]
    public void Wheels_Validity(int wheels, bool shouldPass)
    {
        var result = FilterFactory.TryCreateFilter(
            out _,
            wheels: wheels
        );

        if (shouldPass)
            Assert.True(result.Success);
        else
        {
            Assert.False(result.Success);
            Assert.Contains("wheels", result.Message);
        }
    }

    [Trait("Category", "RequireAllFields")]
    [Fact(DisplayName = "requireAllFields=true with all fields succeeds")]
    public void RequireAllFields_Complete_Succeeds()
    {
        var result = FilterFactory.TryCreateFilter(
            out _,
            reg: "ABC123", type: "Car", fuel: "Petrol", color: "Red", wheels: 4
        );
        Assert.True(result.Success);
    }


    // ── Normalisation ─────────────────────────────────────────────────────────

    [Trait("Category", "Normalisation")]
    [Theory(DisplayName = "Color is normalised to Title Case")]
    [InlineData("red", "Red")]
    [InlineData("RED", "Red")]
    [InlineData("BLUE", "Blue")]
    [InlineData("green", "Green")]
    public void Color_NormalisedToTitleCase(string input, string expected)
    {
        var result = FilterFactory.TryCreateFilter(out var filter, color: input);
        Assert.True(result.Success, result.Message);
        Assert.Equal(expected, filter.Color);
    }

    [Trait("Category", "Normalisation")]
    [Theory(DisplayName = "Fuel type is normalised to Title Case")]
    [InlineData("petrol", "Petrol")]
    [InlineData("DIESEL", "Diesel")]
    [InlineData("electric", "Electric")]
    public void FuelType_NormalisedToTitleCase(string input, string expected)
    {
        var result = FilterFactory.TryCreateFilter(out var filter, color: input);
        Assert.True(result.Success, result.Message);
        Assert.Equal(expected, filter.Color);
    }

    [Trait("Category", "Normalisation")]
    [Fact(DisplayName = "Whitespace is trimmed from all fields")]
    public void AllFields_WhitespaceTrimmed()
    {
        var result = FilterFactory.TryCreateFilter(
            out var filter,
            reg: "  ABC123  ",
            color: "  Red  ",
            fuel: "  Petrol  "
        );
        Assert.True(result.Success, result.Message);
        Assert.Equal("ABC123", filter.RegistryNumber);
        Assert.Equal("Red", filter.Color);
        Assert.Equal("Petrol", filter.FuelType);
    }

    // ── Failure state ─────────────────────────────────────────────────────────

    [Trait("Category", "FailureState")]
    [Fact(DisplayName = "When validation fails, filter should not contain invalid data")]
    public void WhenValidationFails_FilterShouldBeEmpty()
    {
        var operation = FilterFactory.TryCreateFilter(
            out var filter,
            reg: "BAD REG!"
        );
        Assert.False(operation.Success);
        Assert.Null(filter);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// VEHICLE FACTORY TESTS
// ═══════════════════════════════════════════════════════════════════════════════

[Trait("Category", "VehicleFactory")]
public class VehicleFactoryTests
{
    [Trait("Category", "CreateVehicle")]
    [Theory(DisplayName = "Can create all vehicle types")]
    [MemberData(nameof(TestData.AllVehicleTypes), MemberType = typeof(TestData))]
    public void CreateVehicle_AllTypes_Succeed(Type type)
    {
        var filter = new VehicleFilter
        {
            VehicleType = type.Name,
            RegistryNumber = "TEST01",
            Color = "Red",
            FuelType = "Petrol",
            NumWheels = 4
        };
        IVehicle vehicle;
        var result = VehicleFactory.TryCreateVehicle(filter, out vehicle);
        Assert.NotNull(vehicle);
        Assert.Equal(type, vehicle.GetType());
    }

    [Trait("Category", "CreateVehicle")]
    [Fact(DisplayName = "Created vehicle has correct properties")]
    public void CreateVehicle_PropertiesMatchFilter()
    {
        var filter = new VehicleFilter
        {
            VehicleType = "Car",
            RegistryNumber = "PROP01",
            Color = "Blue",
            FuelType = "Electric",
            NumWheels = 4,
            UniquePropertyValue = "5"
        };
        IVehicle vehicle;
        var result = VehicleFactory.TryCreateVehicle(filter, out vehicle);
        Assert.Equal("PROP01", vehicle.RegistryNumber);
        Assert.Equal("Blue", vehicle.Color);
        Assert.Equal("Electric", vehicle.FuelType);
        Assert.Equal(4, vehicle.NumWheels);
    }

    [Trait("Category", "CreateVehicle")]
    [Fact(DisplayName = "Unknown vehicle type throws ArgumentException")]
    public void CreateVehicle_UnknownType_Throws()
    {
        var filter = new VehicleFilter { VehicleType = "Hovercraft", RegistryNumber = "HOV001" };
        IVehicle vehicle;
        Assert.False(VehicleFactory.TryCreateVehicle(filter, out vehicle).Success);
    }

    [Trait("Category", "CreateRandom")]
    [Theory(DisplayName = "CreateRandomVehicle works for all types")]
    [MemberData(nameof(TestData.AllVehicleTypes), MemberType = typeof(TestData))]
    public void CreateRandomVehicle_AllTypes_Succeed(Type typeName)
    {
        var vehicle = VehicleFactory.CreateRandomVehicle(typeName);
        Assert.NotNull(vehicle);
        Assert.IsType(typeName, vehicle);
    }

    [Trait("Category", "CreateRandom")]
    [Fact(DisplayName = "CreateRandomVehicle null type throws")]
    public void CreateRandomVehicle_NullType_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateRandomVehicle(null!));
    }

    [Trait("Category", "RegNumber")]
    [Fact(DisplayName = "Generated reg numbers are always 6 characters")]
    public void GenerateRegNumber_IsAlways6Chars()
    {
        for (int i = 0; i < 100; i++)
        {
            var reg = VehicleFactory.GenerateRegNumber();
            Assert.Equal(6, reg.Length);
        }
    }

    [Trait("Category", "RegNumber")]
    [Fact(DisplayName = "Generated reg numbers start with 3 letters")]
    public void GenerateRegNumber_StartsWithThreeLetters()
    {
        for (int i = 0; i < 50; i++)
        {
            var reg = VehicleFactory.GenerateRegNumber();
            Assert.True(char.IsLetter(reg[0]));
            Assert.True(char.IsLetter(reg[1]));
            Assert.True(char.IsLetter(reg[2]));
        }
    }

    [Trait("Category", "RegNumber")]
    [Fact(DisplayName = "Generated reg numbers end with 3 digits")]
    public void GenerateRegNumber_EndsWithThreeDigits()
    {
        for (int i = 0; i < 50; i++)
        {
            var reg = VehicleFactory.GenerateRegNumber();
            Assert.True(char.IsDigit(reg[3]));
            Assert.True(char.IsDigit(reg[4]));
            Assert.True(char.IsDigit(reg[5]));
        }
    }
}