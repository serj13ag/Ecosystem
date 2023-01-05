using UnityEngine;

namespace Services
{
    public class LocalStorageService
    {
        public void Save<T>(string key, T mapSettingsData)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(mapSettingsData));
        }

        public bool TryLoad<T>(string key, out T mapSettingsData) where T : class
        {
            mapSettingsData = null;

            if (PlayerPrefs.HasKey(key))
            {
                var mapSettings = PlayerPrefs.GetString(key);
                mapSettingsData = JsonUtility.FromJson<T>(mapSettings);
                return true;
            }

            return false;
        }
    }
}