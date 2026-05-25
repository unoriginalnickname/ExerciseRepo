using Övning___4.Misc;
//test
public abstract class VehicleBase : IVehicle
{
    public required string RegistryNumber { get; set; }
    public required string Color { get; set; }
    public required int NumWheels { get; set; }
    public required string FuelType { get; set; }
    public string UniquePropertyValue { get; set; }
    public string UniquePropertyString { get { return VehicleTypeRegistry.ApprovedVehicleTypes[this.GetType().Name]; }}
}

public class Airplane : VehicleBase {  } 
public class Motorcycle : VehicleBase {  } 
public class Car : VehicleBase {  }  
public class Bus : VehicleBase {  } 
public class Boat : VehicleBase {  }
public class Ufo : VehicleBase {  } 
public class Uap : VehicleBase {  } 
