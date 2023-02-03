using MeteoEmulator.Models;
using System;

namespace MeteoEmulator.Helpers
{
    internal static class SensorExtension
    {
        private static Random _random;

        private static Random Random => _random ??= new Random();

        public static double GetNextValue(double val, int dataStability)
        {
            var prop = Random.Next(Math.Min(Math.Max(dataStability, 2), 10));

            return prop switch
            {
                0 => val + _random.NextDouble(), //едем вверх
                1 => val - _random.NextDouble(), //едем вниз
                _ => val
            };
        }

        public static Sensor MakeDataNoisy(Sensor sensor, bool withNoise)
        {
            //возвращаем данные с некоторыми "помехами". Иногда, с вероятностью 1 к 50 будем давать "всплески" плохих данных, выходящих за рамки или близко к ним
            var result = new Sensor(sensor);

            //"Дрожание" данных, чтоб данные былли всегда немного неточными
            if (withNoise)
            {
                result.SensorValue = MakeNoise(result.SensorValue);
            }

            if (_random.NextDouble() < 0.15)
            {
                result.SensorValue = _random.Next(2) > 0 ? sensor.SensorMinValue : sensor.SensorMaxValue;
                if (_random.NextDouble() < 0.25)
                {
                    result.SensorValue = short.MaxValue;
                }
            }

            return result;
        }

        private static double MakeNoise(double val)
        {
            var r1 = Random.Next(3);

            return r1 switch
            {
                0 => val + _random.NextDouble() * 0.5,
                1 => val - _random.NextDouble() * 0.5,
                _ => val
            };
        }
    }
}
