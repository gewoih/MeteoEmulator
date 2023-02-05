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
                PackageNumber = meteoData.DataPackageID,
                MeteoStationName = meteoData.EmulatorID,
                SensorData = new List<SensorDataDAO>()
            };

            foreach (var sensor in meteoData.SensorData)
            {
                meteoDataDTO.SensorData.Add(
                    new SensorDataDAO
                    {
                        Package = meteoDataDTO,
                        Name = sensor.SensorName,
                        Value = sensor.SensorValue,
                        Type = sensorDataType
                    });
            }

            return meteoDataDTO;
        }
    }
}
