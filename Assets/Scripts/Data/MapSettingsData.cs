using System;

namespace Data
{
    [Serializable]
    public class MapSettingsData
    {
        public int Seed;
        public float Scale;
        public float WaterLevel;
        public float TreesPercentage;

        public MapSettingsData(int seed, float scale, float waterLevel, float treesPercentage)
        {
            Seed = seed;
            Scale = scale;
            WaterLevel = waterLevel;
            TreesPercentage = treesPercentage;
        }
    }
}