using System;

namespace Controllers
{
    public class RandomService
    {
        private int _seed;

        public Random GetNewSeedRandom => new Random(_seed);

        public void SetMapSeed(int seed)
        {
            _seed = seed;
        }
    }
}