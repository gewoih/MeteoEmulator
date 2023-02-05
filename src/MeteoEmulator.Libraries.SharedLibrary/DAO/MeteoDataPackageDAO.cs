namespace MeteoEmulator.Libraries.SharedLibrary.DTO
{
    public sealed class MeteoDataPackageDTO
    {
        public int Id { get; set; }
        public long PackageID { get; set; }
        public string MeteoStationName { get; set; }
        public List<SensorDataDTO> SensorData { get; set; }
    }
}
