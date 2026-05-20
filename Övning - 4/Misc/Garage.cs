using Övning___4.Misc;
using System.Collections;

public class Garage<T> : IGarage where T : IVehicle
{
    public bool HasFreeSlots { get { return MaxSize > internalGarage.Count; } }
    public int NumFreeSlots => MaxSize - internalGarage.Count;
    public IEnumerator<IVehicle> GetEnumerator() { return internalGarage.Cast<IVehicle>().GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    private List<T> internalGarage;
    public Type TypeOfGarage { get; }
    public int MaxSize { get; }
    public string GarageName { get; set; }
    public Garage(int garageSize, string garageName)
    {
        internalGarage = new List<T>();
        GarageName = garageName;
        TypeOfGarage = typeof(T);
        MaxSize = garageSize;
    }

    public void ParkVehicle(IVehicle vehicle) => internalGarage.Add((T)vehicle);    // Cast is safe as long as callers respect TypeOfGarage — enforced by GarageManager
    public void Unpark(string registryNumber) => internalGarage.RemoveAll(v => v.RegistryNumber == registryNumber);
    public IEnumerable<IVehicle> GetVehicles() => internalGarage.Cast<IVehicle>();
    public override string ToString()
    {
        return $"{TypeOfGarage.Name + " garage:",-20}" +
               $"{$"\"{GarageName}\",",-25}" +
               $"space:{NumFreeSlots}/{MaxSize}";
    }

}
