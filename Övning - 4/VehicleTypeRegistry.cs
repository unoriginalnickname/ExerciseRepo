using System;
using System.Collections.Generic;
using System.Text;

namespace Övning___4
{
    static class VehicleTypeRegistry
    {
       static readonly Dictionary<string, string> approvedVehicleTypes = new()
 {
    { "Airplane", "Wing span (m)" },
    { "Boat", "Hull length (m)" },
    { "Bus", "Number of stops" },
    { "Car", "Number of doors" },
    { "Motorcycle", "Engine size (cc)" },
    { "Ufo", "Abduction capacity" },
    { "Uap", "Classified" },
 };

        public static Dictionary<string, string> ApprovedVehicleTypes { get { return approvedVehicleTypes; } }

    }
}
