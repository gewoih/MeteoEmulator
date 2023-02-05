namespace MeteoEmulator.Libraries.SharedLibrary.DAO
{
    public sealed class MeteoDataPackageDAO
    {
        public int Id { get; set; }
        public long PackageID { get; set; }
        public string MeteoStationName { get; set; }
        public List<SensorDataDAO> SensorData { get; set; }
    }
}
