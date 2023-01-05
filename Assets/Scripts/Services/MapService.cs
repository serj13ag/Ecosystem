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
                    TileType tileType = GenerateTileType(row, column,
                        mapSettingsData.Seed, mapSettingsData.Refinement, mapSettingsData.WaterLevel);

                    bool onBorder = row == 0
                                    || column == 0
                                    || row == Constants.MapSize - 1
                                    || column == Constants.MapSize - 1;

                    MapTiles.Add(new Tile(row, column, tileType, onBorder));
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
                if (tile.TileType == TileType.Land)
                {
                    landTilesPositions.Add(tile.Position);
                }
            }

            return landTilesPositions.ToArray();
        }

        private TileType GenerateTileType(int row, int column, int seed, float refinement, float waterLevel)
        {
            float tileHeight = Mathf.PerlinNoise(row * refinement + seed, column * refinement + seed);

            return tileHeight > waterLevel
                ? TileType.Land
                : TileType.Water;
        }
    }
}