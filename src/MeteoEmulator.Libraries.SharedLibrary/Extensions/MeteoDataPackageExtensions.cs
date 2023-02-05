using MeteoEmulator.Libraries.SharedLibrary.DAO;
using MeteoEmulator.Libraries.SharedLibrary.Enums;
using MeteoEmulator.Libraries.SharedLibrary.Models;

namespace MeteoEmulator.Libraries.SharedLibrary.Extensions
{
    public static class MeteoDataPackageExtensions
    {
        public static MeteoDataPackageDAO ToDTO(this MeteoDataPackage meteoData, SensorDataType sensorDataType)
        {
            var meteoDataDTO = new MeteoDataPackageDAO
            {
                PackageID = meteoData.DataPackageID,
                MeteoStationName = meteoData.EmulatorID,
                SensorData = new List<SensorDataDAO>()
            };

            foreach (var sensor in meteoData.SensorData)
            {
                meteoDataDTO.SensorData.Add(
                    new SensorDataDAO
                    {
                        Package = meteoDataDTO,
                        SensorName = sensor.SensorName,
                        SensorValue = sensor.SensorValue,
                        Type = sensorDataType
                    });
            }

            return meteoDataDTO;
        }
    }
}
