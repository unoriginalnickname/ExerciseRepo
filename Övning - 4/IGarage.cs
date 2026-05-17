using GaragePractice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Övning___4
{
    internal interface IGarage
    {
        public void ParkVehicle(IVehicle vehicle);

        public void Unpark(string registryNumber);

        IEnumerable<IVehicle> GetVehicles(); // instead of inheriting IEnumerable

    }
   
}
