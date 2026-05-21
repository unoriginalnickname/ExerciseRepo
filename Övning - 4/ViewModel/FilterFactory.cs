namespace Övning___4.ViewModel
{
    public static class FilterFactory
    {
        private const int MinWheels = 0;
        private const int MaxWheels = 18;

        public static Filter ConvertVehicleToFilter(IVehicle v) => new Filter
        {
            VehicleType = v.GetType().Name,
            RegistryNumber = v.RegistryNumber,
            Color = v.Color,
            NumWheels = v.NumWheels,
            FuelType = v.FuelType,
            UniquePropertyValue = v.UniquePropertyValue,
            UniquePropertyString = v.UniquePropertyString
        };


        public static OperationResult TryCreateFilter(out Filter filter, string? reg = null, string? type = null, string? fuel = null, string? color = null, string? unique = null, int? wheels = null)
        {
            filter = null!;

            reg = reg?.Trim();
            type = type?.Trim();
            fuel = fuel?.Trim();
            color = color?.Trim();
            unique = unique?.Trim();

            string? normalizedReg = null;
            if (reg != null)
            {
                if (string.IsNullOrWhiteSpace(reg))
                    return OperationResult.Fail("Registration number cannot be empty.");
                else if (reg.All(c => char.IsLetterOrDigit(c) || c == '-' || c == ' '))
                    normalizedReg = reg.ToUpperInvariant();
                else
                    return OperationResult.Fail("Registration number contains invalid characters.");
            }

            if (wheels is not null && (wheels < MinWheels || wheels > MaxWheels))
                return OperationResult.Fail($"Number of wheels must be between {MinWheels} and {MaxWheels}.");

            string? NormalizeWord(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;
                return char.ToUpperInvariant(s![0]) + s.Substring(1).ToLowerInvariant();
            }

            filter = new Filter
            {
                NumWheels = wheels,
                Color = NormalizeWord(color),
                FuelType = NormalizeWord(fuel),
                RegistryNumber = normalizedReg,
                VehicleType = NormalizeWord(type),
                UniquePropertyValue = string.IsNullOrWhiteSpace(unique) ? null : unique
            };

            return OperationResult.Ok("Filter created successfully.");
        }
    }
}
