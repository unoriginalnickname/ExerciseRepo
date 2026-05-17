using System;
using System.Collections.Generic;
using System.Linq;

namespace GaragePractice
{
    public static class FilterFactory
    {
        private const int MinWheels = 0;
        private const int MaxWheels = 18;

        public static bool TryCreate(string? reg, string? type, string? fuel, string? color, string? unique, int? wheels, bool requireAllFields, out Filter filter, out List<string> errors)
        {
            errors = new List<string>();

            reg = reg?.Trim();
            type = type?.Trim();
            fuel = fuel?.Trim();
            color = color?.Trim();
            unique = unique?.Trim();

            string? normalizedReg = null;
            if (!string.IsNullOrWhiteSpace(reg))
            {
                if (reg.All(char.IsLetterOrDigit))
                    normalizedReg = reg.ToUpperInvariant();
                else
                    errors.Add("Registration number contains invalid characters.");
            }
            else if (requireAllFields)
            {
                errors.Add("Registration number is required.");
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

            if (requireAllFields)
            {
                if (string.IsNullOrWhiteSpace(normalizedType)) errors.Add("Vehicle type is required.");
                if (string.IsNullOrWhiteSpace(normalizedColor)) errors.Add("Color is required.");
                if (wheels is null) errors.Add("Number of wheels is required.");
            }

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

            return errors.Count == 0;
        }
    }
}
