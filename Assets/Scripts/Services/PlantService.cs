using System.Collections.Generic;
using DataTypes;
using Entities;

namespace Services
{
    public class PlantService
    {
        private readonly RandomService _randomService;

        public HashSet<Point> PlantLocations { get; }

        public PlantService(RandomService randomService)
        {
            _randomService = randomService;

            PlantLocations = new HashSet<Point>();
        }

        public void GeneratePlants(Dictionary<Point, Tile> mapTiles)
        {
            PlantLocations.Clear();

            foreach (KeyValuePair<Point, Tile> mapTile in mapTiles)
            {
                if (mapTile.Value.SuitableForPlants
                    && !mapTile.Value.HasTree
                    && _randomService.SeedRandom.Next(100) < 20)
                {
                    PlantLocations.Add(mapTile.Key);
                }
            }
        }
    }
}