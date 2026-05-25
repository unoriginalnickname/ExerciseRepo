    internal interface IGarage : IEnumerable<IVehicle>
    {
    public static string Header()
    {
        return $"{"Type",-20}{"Name",-25}{"Space"}";
    }
    public string Name { get; set; }
        public int MaxSize { get; }
        public bool HasFreeSlots { get; }
        public int NumFreeSlots { get; }

        public Type GarageVehicleType { get; }
        public void ParkVehicle(IVehicle vehicle);
        public bool ContainsVehicleRegNumber(string regNumber);

        public void Unpark(string registryNumber);

        IEnumerable<IVehicle> GetVehicles();

    }
