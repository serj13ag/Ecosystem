using System;
using System.Collections.Generic;
using Data;
using Prefabs;
using UI;
using UnityEngine;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private TilePrefab _tilePrefab;
        [SerializeField] private Transform _tilesContainer;

        [SerializeField] private int _mapSize;

        [SerializeField] private Material _waterMaterial;
        [SerializeField] private Material _landMaterial;

        private TilePrefab[,] _tiles;

        public Vector3[] GetLandTilesPositions()
        {
            if (_tiles == null)
            {
                return Array.Empty<Vector3>();
            }

            var landTilesPositions = new List<Vector3>();

            foreach (var tile in _tiles)
            {
                if (tile.MeshRendererSharedMaterial == _landMaterial)
                {
                    landTilesPositions.Add(tile.transform.position);
                }
            }

            return landTilesPositions.ToArray();
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            if (_tiles == null)
            {
                CreateTiles(mapSettingsData.Seed, mapSettingsData.Refinement, mapSettingsData.WaterLevel);
            }
            else
            {
                UpdateTiles(mapSettingsData.Seed, mapSettingsData.Refinement, mapSettingsData.WaterLevel);
            }

            CombineTileMeshes();
        }

        private void CreateTiles(int seed, float refinement, float waterLevel)
        {
            _tiles = new TilePrefab[_mapSize, _mapSize];

            for (var row = 0; row < _mapSize; row++)
            {
                for (var column = 0; column < _mapSize; column++)
                {
                    TilePrefab tile = InstantiateTile(row, column);

                    TileType tileType = GenerateTileType(row, column, seed, refinement, waterLevel);
                    UpdateTileMaterial(tile, tileType);

                    _tiles[row, column] = tile;
                }
            }
        }

        private void UpdateTiles(int seed, float refinement, float waterLevel)
        {
            for (var row = 0; row < _mapSize; row++)
            {
                for (var column = 0; column < _mapSize; column++)
                {
                    TilePrefab tile = _tiles[row, column];

                    TileType tileType = GenerateTileType(row, column, seed, refinement, waterLevel);
                    UpdateTileMaterial(tile, tileType);
                }
            }
        }

        private void CombineTileMeshes()
        {
            StaticBatchingUtility.Combine(_tilesContainer.gameObject);
        }

        private static TileType GenerateTileType(int row, int column, int seed, float refinement, float waterLevel)
        {
            float tileHeight = Mathf.PerlinNoise(row * refinement + seed, column * refinement + seed);

            return tileHeight > waterLevel
                ? TileType.Land
                : TileType.Water;
        }

        private void UpdateTileMaterial(TilePrefab tile, TileType tileType)
        {
            Material tileMaterial = tileType switch
            {
                TileType.Land => _landMaterial,
                TileType.Water => _waterMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
            };

            tile.MeshRendererSharedMaterial = tileMaterial;
        }

        private TilePrefab InstantiateTile(int row, int column)
        {
            return Instantiate(_tilePrefab, new Vector3(row, 0, column), Quaternion.identity, _tilesContainer);
        }
    }
}