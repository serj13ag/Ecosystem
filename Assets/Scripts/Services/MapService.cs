﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Map;
using UnityEngine;

namespace Services
{
    public class MapService
    {
        private const float HalfMapSize = Constants.MapSize / 2f;

        public Dictionary<Vector2Int, Tile> MapTiles { get; }

        public MapService()
        {
            MapTiles = new Dictionary<Vector2Int, Tile>();
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

                    bool onBorder = row == 0 ||
                                    column == 0 ||
                                    row == Constants.MapSize - 1 ||
                                    column == Constants.MapSize - 1;

                    bool walkable = tilePerlinHeight > mapSettingsData.WaterLevel;
                    bool suitableForPlants = tilePerlinHeight > mapSettingsData.WaterLevel + Constants.TreesShoreOffset;

                    Vector2Int tilePosition = new Vector2Int(row, column);
                    Tile tile = new Tile(tilePosition, tileActualHeight, onBorder, suitableForPlants, walkable);
                    MapTiles.Add(tilePosition, tile);
                }
            }
        }

        public Vector2Int[] GetSuitableForPlantsTilesPositions()
        {
            if (MapTiles == null)
            {
                return Array.Empty<Vector2Int>();
            }

            return MapTiles
                .Values
                .Where(x => x.SuitableForPlants)
                .Select(y => y.Position)
                .ToArray();
        }

        private static float GeneratePerlinNoise(int row, int column, int seed, float scale)
        {
            return Mathf.PerlinNoise((row - HalfMapSize) * scale + seed, (column - HalfMapSize) * scale + seed);
        }
    }
}