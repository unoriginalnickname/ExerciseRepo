public abstract class VehicleBase : IVehicle
{
    public required string RegistryNumber { get; set; }
    public required string Color { get; set; }
    public required int NumWheels { get; set; }
    public required string FuelType { get; set; }
    public required string UniquePropertyValue { get; set; }
    public required string UniquePropertyString { get; set; }
}

public class Airplane : VehicleBase {  } 
public class Motorcycle : VehicleBase {  } 
public class Car : VehicleBase {  }  
public class Bus : VehicleBase {  } 
public class Boat : VehicleBase {  }
public class Ufo : VehicleBase {  } 
public class Uap : VehicleBase {  } 
