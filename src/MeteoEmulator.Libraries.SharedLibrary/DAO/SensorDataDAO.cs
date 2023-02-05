using MeteoEmulator.Libraries.SharedLibrary.Enums;

namespace MeteoEmulator.Libraries.SharedLibrary.DAO
{
    public sealed class SensorDataDAO
    {
        public int Id { get; set; }
        public MeteoDataPackageDAO Package { get; set; }
        public SensorDataType Type { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
