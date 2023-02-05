namespace MeteoEmulator.Libraries.SharedLibrary.Models
{
    public sealed class MeteoStationsStatistics
    {
        public int TotalMeteoStationsCount { get; set; }
        public int TotalMeteoStationsSensorsCount { get; set; }
        public Dictionary<string, int> MeteoStationsSensorsCount { get; set; }
        public Dictionary<string, int> SensorsDataCount { get; set; }
    }
}
