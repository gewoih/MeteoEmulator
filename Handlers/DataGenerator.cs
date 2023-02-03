using MeteoEmulator.Helpers;
using MeteoEmulator.Models;
using System.Collections.Generic;
using System.Linq;
using MeteoEmulator.Utils;
using Microsoft.Extensions.Options;

namespace MeteoEmulator.Handlers
{
    public class DataGenerator
    {
        private long _packageID;
        private List<Sensor> _sensors;
        private readonly string _emulatorID;
        private readonly bool _withNoise;
        private readonly int _dataStability;

        public DataGenerator(IOptions<Arguments> argOptions)
        {
            _emulatorID = argOptions.Value.InstanceID;
            _packageID = 0;
            _withNoise = argOptions.Value.WithNoise;
            _dataStability = argOptions.Value.DataStability;

            _sensors = new List<Sensor>()
            {
                new Sensor()
                {
                    SensorName = "temperature",
                    SensorShortName = "t",
                    SensorValue = 22.0,
                    SensorMinValue = 18.0,
                    SensorMaxValue = 28.0
                },
                new Sensor()
                {
                    SensorName = "humidity",
                    SensorShortName = "h",
                    SensorValue = 35.0,
                    SensorMinValue = 22.0,
                    SensorMaxValue = 45.0
                },
                new Sensor()
                {
                    SensorName = "pressure",
                    SensorShortName = "p",
                    SensorValue = 756.0,
                    SensorMinValue = 750.0,
                    SensorMaxValue = 768.0
                }
            };
        }

        public void UpdateSensorData()
        {
            //Обновляем данные датчиков в заданных пределах. Постепенно "плавая" в некотором диапазоне
            _sensors.ForEach(s => s.SensorValue = SensorExtension.GetNextValue(s.SensorValue, _dataStability));
        }

        public MeteoDataPackage GetDataWithNoise()
        {
            return new MeteoDataPackage()
            {
                DataPackageID = _packageID++,
                EmulatorID = _emulatorID,
                SensorData = _sensors.Select(s => SensorExtension.MakeDataNoisy(s, _withNoise).GetData()).ToList()
            };
        }

        public MeteoDataPackage GetData()
        {
            return new MeteoDataPackage()
            {
                DataPackageID = _packageID,
                EmulatorID = _emulatorID,
                SensorData = _sensors.Select(s => s.GetData()).ToList()
            };
        }
    }
}
