using System.Collections;
using System.Diagnostics;

public class Garage<T> : IGarage where T : IVehicle
{

    public bool HasFreeSlots { get { return MaxSize > internalGarage.Count; } }
    public int NumFreeSlots => MaxSize - internalGarage.Count;
    public IEnumerator<IVehicle> GetEnumerator() { return internalGarage.Cast<IVehicle>().GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    private List<T> internalGarage;
    public Type GarageVehicleType { get; }
    public int MaxSize { get; }
    public string Name { get; set; }
    public Garage(int garageSize, string garageName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(garageName);
        if (garageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(garageSize), "Garage size must be greater than zero.");

        internalGarage = new List<T>();
        Name = garageName;
        GarageVehicleType = typeof(T);
        MaxSize = garageSize;
    }

    public bool ContainsVehicleRegNumber(string regNumber) =>
      internalGarage.Any(v => string.Equals(v.RegistryNumber, regNumber, StringComparison.OrdinalIgnoreCase));
    public void ParkVehicle(IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (internalGarage.Any(v => string.Equals(v.RegistryNumber,
            vehicle.RegistryNumber, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Duplicate reg number: '{vehicle.RegistryNumber}'.");

        if (!HasFreeSlots)
            throw new InvalidOperationException($"Garage '{Name}' is full.");

        internalGarage.Add((T)vehicle);
    }
    public void Unpark(string regNumber)
    {
        if (string.IsNullOrWhiteSpace(regNumber))
            throw new ArgumentException("Registration number cannot be empty.", nameof(regNumber));

        var vehicle = internalGarage.FirstOrDefault(v =>
            string.Equals(v.RegistryNumber, regNumber, StringComparison.OrdinalIgnoreCase));

        if (vehicle == null)
            throw new InvalidOperationException($"Vehicle {regNumber} is not parked in this garage.");

        internalGarage.Remove(vehicle);
    }

    public IEnumerable<IVehicle> GetVehicles() => internalGarage.Cast<IVehicle>();


    public override string ToString()
    {
        return $"{GarageVehicleType.Name + " Garage",-20}" +
               $"{$"\"{Name}\"",-25}" +
               $"space:{NumFreeSlots}/{MaxSize}";
    }
}