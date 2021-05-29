using System;

namespace GarageLite
{
    public class VehicleInfo
    {
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public ushort VehicleId { get; set; }
        public ushort VehicleHealth { get; set; }
        public ushort VehicleBattery { get; set; }
        public ushort VehicleFuel { get; set; }
    }
}
