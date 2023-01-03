using System;
using UnityEngine;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Transform _tilesContainer;

        [SerializeField] private int _mapSize;

        [SerializeField] private Material _waterMaterial;
        [SerializeField] private Material _landMaterial;

        private GameObject[,] _tiles;

        public void UpdateMap(int seed, float refinement, float waterLevel)
        {
            if (_tiles == null)
            {
                CreateTiles(seed, refinement, waterLevel);
            }
            else
            {
                UpdateTiles(seed, refinement, waterLevel);
            }

            CombineTileMeshes();
        }

        private void CreateTiles(int seed, float refinement, float waterLevel)
        {
            _tiles = new GameObject[_mapSize, _mapSize];

            for (var row = 0; row < _mapSize; row++)
            {
                for (var column = 0; column < _mapSize; column++)
                {
                    GameObject tile = InstantiateTile(row, column);

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
                    GameObject tile = _tiles[row, column];

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

        private void UpdateTileMaterial(GameObject tile, TileType tileType)
        {
            Material tileMaterial = tileType switch
            {
                TileType.Land => _landMaterial,
                TileType.Water => _waterMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
            };

            tile.GetComponent<MeshRenderer>().sharedMaterial = tileMaterial;
        }

        private GameObject InstantiateTile(int row, int column)
        {
            return Instantiate(_tilePrefab, new Vector3(row, 0, column), Quaternion.identity, _tilesContainer);
        }
    }
}