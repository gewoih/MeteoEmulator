using MeteoEmulator.Models;

namespace MeteoEmulator.Utils
{
    /// <summary>
    /// Не совсем абстрактное хранилище, чтоб упростить процесс синзхронизации доставаемых данных
    /// </summary>
    public class StateStorage
    {
        private readonly object _lock = new object();
        private StateStorageData _data;

        public StateStorage()
        {
            _data = null;
        }

        public void Put(StateStorageData data)
        {
            lock (_lock)
            {
                _data = data;
            }
        }

        public StateStorageData Get()
        {
            StateStorageData res = null;

            lock (_lock)
            {
                res = _data;
            }

            return res;
        }
    }
}
