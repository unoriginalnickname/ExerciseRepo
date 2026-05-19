


    internal interface IGarage : IEnumerable<IVehicle>
    {
        public string GarageName { get; set; }
        public int MaxSize { get; set; }
        public bool HasFreeSlots { get; }
        public int NumFreeSlots { get; }

        public Type TypeOfGarage { get; }
        public void ParkVehicle(IVehicle vehicle);

        public void Unpark(string registryNumber);

        IEnumerable<IVehicle> GetVehicles(); // instead of inheriting IEnumerable

    }
