using MeteoEmulator.Libraries.SharedLibrary.DTO;
using MeteoEmulator.Libraries.SharedLibrary.Enums;
using MeteoEmulator.Libraries.SharedLibrary.Models;

namespace MeteoEmulator.Libraries.SharedLibrary.Extensions
{
    public static class MeteoDataPackageExtensions
    {
        public static MeteoDataPackageDTO ToDTO(this MeteoDataPackage meteoData, SensorDataType sensorDataType)
        {
            var meteoDataDTO = new MeteoDataPackageDTO
            {
                PackageID = meteoData.DataPackageID,
                MeteoStationName = meteoData.EmulatorID,
                SensorData = new List<SensorDataDTO>()
            };

            foreach (var sensor in meteoData.SensorData)
            {
                meteoDataDTO.SensorData.Add(
                    new SensorDataDTO
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
