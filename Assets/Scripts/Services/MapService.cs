using System;
using System.Collections.Generic;
using Data;
using Enums;
using Map;
using UnityEngine;

namespace Services
{
    public class MapService
    {
        public List<Tile> MapTiles { get; }

        public MapService()
        {
            MapTiles = new List<Tile>();
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            MapTiles.Clear();

            for (var row = 0; row < Constants.MapSize; row++)
            {
                for (var column = 0; column < Constants.MapSize; column++)
                {
                    float tilePerlinHeight =
                        GeneratePerlinNoise(row, column, mapSettingsData.Seed, mapSettingsData.Scale);

                    float tileActualHeight = tilePerlinHeight + (0.5f - mapSettingsData.WaterLevel);
                    TileType tileType = GetTileType(tilePerlinHeight, mapSettingsData.WaterLevel);

                    bool onBorder = row == 0 ||
                                    column == 0 ||
                                    row == Constants.MapSize - 1 ||
                                    column == Constants.MapSize - 1;

                    MapTiles.Add(new Tile(row, column, tileActualHeight, tileType, onBorder));
                }
            }
        }

        public Vector2Int[] GetLandTilesPositions()
        {
            if (MapTiles == null)
            {
                return Array.Empty<Vector2Int>();
            }

            var landTilesPositions = new List<Vector2Int>();
            foreach (var tile in MapTiles)
            {
                if (tile.Type == TileType.Land)
                {
                    landTilesPositions.Add(tile.Position);
                }
            }

            return landTilesPositions.ToArray();
        }

        private static float GeneratePerlinNoise(int row, int column, int seed, float scale)
        {
            return Mathf.PerlinNoise((row - Constants.MapSize / 2f) * scale + seed,
                (column - Constants.MapSize / 2f) * scale + seed);
        }

        private static TileType GetTileType(float tileHeight, float waterLevel)
        {
            return tileHeight >= waterLevel + Constants.TreesShoreOffset
                ? TileType.Land
                : TileType.Water;
        }
    }
}