using System;

namespace Data
{
    [Serializable]
    public class MapSettingsData
    {
        public int Seed;
        public float Refinement;
        public float WaterLevel;
        public float TreesPercentage;

        public MapSettingsData(int seed, float refinement, float waterLevel, float treesPercentage)
        {
            Seed = seed;
            Refinement = refinement;
            WaterLevel = waterLevel;
            TreesPercentage = treesPercentage;
        }
    }
}