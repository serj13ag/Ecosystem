using UnityEngine;

namespace Services
{
    public class LocalStorageService
    {
        public void Save<T>(string key, T data)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
        }

        public bool TryLoad<T>(string key, out T data) where T : class
        {
            data = null;

            if (PlayerPrefs.HasKey(key))
            {
                string jsonData = PlayerPrefs.GetString(key);
                data = JsonUtility.FromJson<T>(jsonData);
                return true;
            }

            return false;
        }
    }
}