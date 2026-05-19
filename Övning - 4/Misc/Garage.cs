using System.Collections;

public class Garage<T> : IGarage where T : IVehicle
{
    private List<T> internalGarage;
    private readonly int defaultSize = 15;
    public Type TypeOfGarage { get; }

    public int MaxSize { get; set; }

    public string GarageName { get; set; }

    public bool HasFreeSlots { get { return MaxSize > internalGarage.Count(); } }

    public int NumFreeSlots => MaxSize - this.Count();

    public Garage(int garageSize, string garageName)
    {
        internalGarage = new List<T>();
        GarageName = garageName;
        TypeOfGarage = typeof(T);
        MaxSize = garageSize;
    }

    public void ParkVehicle(IVehicle vehicle)
    {
        try
        {
            internalGarage.Add((T)vehicle);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Garage: Could not park vehicle. ");
            throw;
        }
    }

    public void Unpark(string registryNumber)
    {
        internalGarage.RemoveAll(v => v.RegistryNumber == registryNumber);
    }

    public IEnumerable<IVehicle> GetVehicles()
    {
        return internalGarage.Cast<IVehicle>();
    }
    public static string Header()
    {
        return $"{"Type",-20}{"Name",-25}{"Space"}";
    }
    public override string ToString()
    {
        return $"{TypeOfGarage.Name + " garage:",-20}" +
               $"{$"\"{GarageName}\",",-25}" +
               $"space:{NumFreeSlots}/{MaxSize}";
    }

    public IEnumerator<IVehicle> GetEnumerator()
    {
        return internalGarage.Cast<IVehicle>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
