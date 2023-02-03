namespace MeteoEmulator.Libraries.SharedLibrary.Models
{
    public class MeteoDataPackage
    {
        public long DataPackageID { get; set; }
        public string EmulatorID { get; set; }
        public List<SensorData> SensorData { get; set; }
    }
}
