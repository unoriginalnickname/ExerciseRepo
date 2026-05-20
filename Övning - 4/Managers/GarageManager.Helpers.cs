using Övning___4.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;


public partial class GarageManager
{
    private bool Matches(IVehicle v, Filter f) =>
        (f.RegistryNumber == null || v.RegistryNumber.Equals(f.RegistryNumber, StringComparison.OrdinalIgnoreCase)) &&
        (f.NumWheels == null || v.NumWheels == f.NumWheels) &&
        (f.Color == null || v.Color.Equals(f.Color, StringComparison.OrdinalIgnoreCase)) &&
        (f.VehicleType == null || v.GetType().Name.Equals(f.VehicleType, StringComparison.OrdinalIgnoreCase)) &&
        (f.FuelType == null || v.FuelType.ToString().Equals(f.FuelType, StringComparison.OrdinalIgnoreCase));

    private IEnumerable<IVehicle> GetAllVehicles() =>
    garages.SelectMany(g => g.GetVehicles());

    string? NormalizeWord(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        return char.ToUpperInvariant(s![0]) + s.Substring(1).ToLowerInvariant();
    }

    internal IEnumerable<string> GetGarageStrings() => garages.Select(g => g.ToString());
}
