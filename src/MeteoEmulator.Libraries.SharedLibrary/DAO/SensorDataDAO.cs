using MeteoEmulator.Libraries.SharedLibrary.Enums;

namespace MeteoEmulator.Libraries.SharedLibrary.DAO
{
    public sealed class SensorDataDAO
    {
        public int Id { get; set; }
        public MeteoDataPackageDAO Package { get; set; }
        public SensorDataType Type { get; set; }
        public string SensorName { get; set; }
        public double SensorValue { get; set; }
    }
}
