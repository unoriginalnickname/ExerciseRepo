using GaragePractice;

public static class FilterValidator
{
    private static bool IsAlphanumeric(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return value.All(char.IsLetterOrDigit);
    }

    public static bool ValidateForPark(Filter f)
    {
        if (!IsAlphanumeric(f.RegNumber))
            return false;

        if (!IsAlphanumeric(f.VehicleType))
            return false;

        if (!IsAlphanumeric(f.Color))
            return false;

        if (f.NumWheels is null or < 0 or > 18)
            return false;

        return true;
    }

    public static bool ValidateForFind(Filter f)
    {
        if (f.RegNumber != null && !IsAlphanumeric(f.RegNumber))
            return false;

        if (f.VehicleType != null && !IsAlphanumeric(f.VehicleType))
            return false;

        if (f.Color != null && !IsAlphanumeric(f.Color))
            return false;

        if (f.NumWheels is < 0 or > 18)
            return false;

        return true;
    }
}