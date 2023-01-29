using DataTypes;
using Random = System.Random;

namespace Services
{
    public class RandomService
    {
        private int _seed;

        public Random GetNewSeedRandom => new Random(_seed);

        public void SetMapSeed(int seed)
        {
            _seed = seed;
        }

        public int RandomFunction(Point additionalSeed, int maxValue)
        {
            return RandomFunction(additionalSeed, 0, maxValue);
        }

        public int RandomFunction(Point additionalSeed, int minValue, int maxValue)
        {
            int range = maxValue - minValue;

            var a = (uint)(additionalSeed.X * additionalSeed.X + additionalSeed.Y * additionalSeed.Y + _seed);
            double b = a % 123 / 123f;
            double c = b * range + minValue;
            return (int)c;
        }
    }
}