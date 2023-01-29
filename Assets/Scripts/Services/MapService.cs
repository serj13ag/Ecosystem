using System.Collections.Generic;
using Controllers;
using Data;
using Map;
using UnityEngine;
using Random = System.Random;

namespace Services
{
    public class MapService
    {
        private const float HalfMapSize = Constants.MapSize / 2f;

        private readonly RandomService _randomService;

        public Dictionary<Point, Tile> MapTiles { get; }

        public MapService(RandomService randomService)
        {
            _randomService = randomService;

            MapTiles = new Dictionary<Point, Tile>();
        }

        public void GenerateMapTiles(MapSettingsData mapSettingsData)
        {
            MapTiles.Clear();

            Random random = _randomService.GetNewSeedRandom;

            for (var row = 0; row < Constants.MapSize; row++)
            {
                for (var column = 0; column < Constants.MapSize; column++)
                {
                    float tilePerlinHeight =
                        GeneratePerlinNoise(row, column, mapSettingsData.Seed, mapSettingsData.Scale);

                    float tileActualHeight = tilePerlinHeight + (0.5f - mapSettingsData.WaterLevel);

                    bool onBorder = row == 0 ||
                                    column == 0 ||
                                    row == Constants.MapSize - 1 ||
                                    column == Constants.MapSize - 1;

                    bool walkable = tilePerlinHeight > mapSettingsData.WaterLevel;
                    bool suitableForPlants = tilePerlinHeight > mapSettingsData.WaterLevel + Constants.TreesShoreOffset;
                    bool hasTree = random.Next(100) < mapSettingsData.TreesPercentage && suitableForPlants;

                    Point tilePosition = new Point(row, column);
                    Tile tile = new Tile(tilePosition, tileActualHeight, onBorder, suitableForPlants, walkable,
                        hasTree);
                    MapTiles.Add(tilePosition, tile);
                }
            }
        }

        private static float GeneratePerlinNoise(int row, int column, int seed, float scale)
        {
            return Mathf.PerlinNoise((row - HalfMapSize) * scale + seed, (column - HalfMapSize) * scale + seed);
        }
    }
}