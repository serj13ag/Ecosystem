using System;
using System.Collections.Generic;
using Map;
using Prefabs;
using UnityEngine;

namespace Controllers
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private TilePrefab _tilePrefab;
        [SerializeField] private Transform _tilesContainer;

        [SerializeField] private Material _waterMaterial;
        [SerializeField] private Material _landMaterial;

        private Dictionary<Vector2Int, TilePrefab> _tilePrefabs;

        public void UpdateMap(List<Tile> mapTiles)
        {
            if (_tilePrefabs == null)
            {
                _tilePrefabs = new Dictionary<Vector2Int, TilePrefab>();
                CreateTiles(mapTiles);
            }
            else
            {
                UpdateTiles(mapTiles);
            }

            CombineTileMeshes();
        }

        private void CreateTiles(List<Tile> mapTiles)
        {
            foreach (Tile mapTile in mapTiles)
            {
                TilePrefab tilePrefab = InstantiateTile(mapTile.Position.x, mapTile.Position.y);
                UpdateTileMaterial(tilePrefab, mapTile.TileType);
                _tilePrefabs.Add(mapTile.Position, tilePrefab);
            }
        }

        private void UpdateTiles(List<Tile> mapTiles)
        {
            foreach (Tile mapTile in mapTiles)
            {
                TilePrefab tilePrefab = _tilePrefabs[mapTile.Position];
                UpdateTileMaterial(tilePrefab, mapTile.TileType);
            }
        }

        private void CombineTileMeshes()
        {
            StaticBatchingUtility.Combine(_tilesContainer.gameObject);
        }

        private void UpdateTileMaterial(TilePrefab tilePrefab, TileType tileType)
        {
            Material tileMaterial = tileType switch
            {
                TileType.Land => _landMaterial,
                TileType.Water => _waterMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
            };

            tilePrefab.MeshRendererSharedMaterial = tileMaterial;
        }

        private TilePrefab InstantiateTile(int positionX, int positionY)
        {
            var position = new Vector3(positionX, Constants.TerrainPositionY, positionY);
            return Instantiate(_tilePrefab, position, Quaternion.identity, _tilesContainer);
        }
    }
}