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


        public static OperationResult TryCreate(out Filter filter, string? reg = null, string? type = null, string? fuel = null, string? color = null, string? unique = null, int? wheels = null)
        {
            var errors = new List<string>();

            reg = reg?.Trim();
            type = type?.Trim();
            fuel = fuel?.Trim();
            color = color?.Trim();
            unique = unique?.Trim();

            string? normalizedReg = null;
            if (!string.IsNullOrWhiteSpace(reg))
            {
                if (reg.All(c => char.IsLetterOrDigit(c) || c == '-' || c == ' '))
                    normalizedReg = reg.ToUpperInvariant();
                else
                    errors.Add("Registration number contains invalid characters.");
            }

            string? NormalizeWord(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;
                return char.ToUpperInvariant(s![0]) + s.Substring(1).ToLowerInvariant();
            }

            string? normalizedType = NormalizeWord(type);
            string? normalizedColor = NormalizeWord(color);
            string? normalizedFuel = NormalizeWord(fuel);
            string? normalizedUnique = string.IsNullOrWhiteSpace(unique) ? null : unique;

            if (wheels is not null)
            {
                if (wheels < MinWheels || wheels > MaxWheels)
                    errors.Add($"Number of wheels must be between {MinWheels} and {MaxWheels}.");
            }

            filter = new Filter
            {
                NumWheels = wheels,
                Color = normalizedColor,
                FuelType = normalizedFuel,
                RegistryNumber = normalizedReg,
                VehicleType = normalizedType,
                UniquePropertyValue = normalizedUnique
            };

            if (errors.Count > 0)
            {
                filter = null!;
                return OperationResult.Fail(string.Join("\n", errors));
            }

            return OperationResult.Ok("Filter created successfully.");
        }
    }
}
