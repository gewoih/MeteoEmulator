using MeteoEmulator.Libraries.SharedLibrary.Enums;

namespace MeteoEmulator.Libraries.SharedLibrary.DTO
{
    public sealed class SensorDataDTO
    {
        public int Id { get; set; }
        public MeteoDataPackageDTO Package { get; set; }
        public SensorDataType Type { get; set; }
        public string SensorName { get; set; }
        public double SensorValue { get; set; }
    }
}
