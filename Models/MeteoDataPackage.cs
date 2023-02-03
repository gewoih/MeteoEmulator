using System.Collections.Generic;

namespace MeteoEmulator.Models
{
    public class MeteoDataPackage
    {
        public long DataPackageID { get; set; }
        public string EmulatorID { get; set; }
        public List<SensorData> SensorData { get; set; }
    }
}
