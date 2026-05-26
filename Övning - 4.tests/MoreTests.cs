using Övning___4.ViewModel;

public class FailingTests
{
    [Fact]
    public void TryAddNewGarage_NullName_ReturnsFailInsteadOfThrowing()
    {
        var m = new GarageManager();
        var result = m.TryAddNewGarage("Car", 5, null);
        Assert.False(result.Success);
    }

    [Fact]
    public void TryAddNewGarage_DuplicateName_ActuallyFails()
    {
        var m = new GarageManager();
        m.TryAddNewGarage("Car", 5, "MyGarage");

        // Second call with the same name — should Fail, but currently Succeeds
        var result = m.TryAddNewGarage("Bus", 5, "MyGarage");
        Assert.False(result.Success);
    }


    [Fact]
    public void TryAddNewGarage_AllCapsTypeName_Succeeds()
    {
        var m = new GarageManager();
        // NormalizeWord("CAR") → "Car" → should resolve fine
        // Currently fails because Type.GetType("Car") returns null when the
        // assembly isn't searched — depends on runtime; worth guarding explicitly
        var result = m.TryAddNewGarage("CAR", 5, "UpperCaseTest");
        Assert.True(result.Success);
    }


    [Fact]
    public void Garage_Unpark_NullRegNumber_ThrowsArgumentException()
    {
        var g = new Garage<Car>(5, "Test");
        // Should throw ArgumentException per the guard — may throw
        // NullReferenceException instead on some runtimes
        Assert.Throws<ArgumentException>(() => g.Unpark(null!));
    }



    [Theory]
    [InlineData("BANANA", true)]
    [InlineData("BÅNANÅ", true)]
    [InlineData("ABC-123", true)]
    [InlineData("ABC 123", true)]
    [InlineData("GURKA1", true)]
    [InlineData("ABC!123", false)]
    [InlineData("hello@world", false)]
    public void FilterFactory_RegNumberValidation(string reg, bool shouldPass)
    {
        var success = FilterFactory.TryCreateFilter(
            out var filter,
            reg: reg,
            type: "Car",
            fuel: "Petrol",
            color: "Red",
            unique: "4 doors",
            wheels: 4
        ).Success;

        Assert.Equal(shouldPass, success);
    }

    [Fact]
    public void FilterFactory_WhenValidationFails_FilterShouldBeEmpty()
    {
        var success = FilterFactory.TryCreateFilter(
            out var filter,
            reg: "BAD REG!", // invalid — contains spaces and !
            type: null,
            fuel: null,
            color: null,
            unique: null,
            wheels: null
        ).Success;

        Assert.False(success);
        // filter should be null or a safe default — currently it is a partially
        // populated object with whatever valid fields were passed
        Assert.Null(filter);
    }

    [Fact]
    public void ListSpecificGarage_CaseInsensitiveName_FindsGarage()
    {
        var m = new GarageManager();
        m.TryAddNewGarage("Car", 5, "My Cars");

        // All lowercase — should still find the garage
        var result = m.TryListSpecificGarage("my cars");
        Assert.True(result.Success);
    }


    [Fact]
    public void Garage_ParkVehicle_DoesNotPreventDuplicateRegNumbers()
    {
        var g = new Garage<Car>(5, "Test");
        var car1 = new Car { RegistryNumber = "DUP001", Color = "Red", FuelType = "Petrol", NumWheels = 4 };
        var car2 = new Car { RegistryNumber = "DUP001", Color = "Blue", FuelType = "Diesel", NumWheels = 4 };

        g.ParkVehicle(car1);

        Assert.Throws<InvalidOperationException>(() => g.ParkVehicle(car2));
    }
}