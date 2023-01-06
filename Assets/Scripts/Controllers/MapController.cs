using System;
using System.Collections.Generic;
using Enums;
using Map;
using Prefabs;
using UnityEngine;

namespace Controllers
{
    public class MapController : MonoBehaviour
    {
        private const float StartVertex = -0.5f;

        [SerializeField] private TerrainPrefab _terrain;

        public void UpdateMap(List<Tile> mapTiles)
        {
            CreateTerrain(mapTiles);
        }

        private void CreateTerrain(List<Tile> mapTiles)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            foreach (Tile mapTile in mapTiles)
            {
                int vertexIndex = vertices.Count;

                Vector3 leftBottomVertex = new Vector3(
                    StartVertex + mapTile.Position.x,
                    GetTileHeight(mapTile),
                    StartVertex + mapTile.Position.y);
                Vector3 leftTopVertex = leftBottomVertex + Vector3.forward;
                Vector3 rightTopVertex = leftTopVertex + Vector3.right;
                Vector3 rightBottomVertex = leftBottomVertex + Vector3.right;
                var tileVertices = new Vector3[] { leftBottomVertex, leftTopVertex, rightTopVertex, rightBottomVertex };
                vertices.AddRange(tileVertices);

                Vector2 uv = GetMapTileUV(mapTile);
                for (var tileVertex = 0; tileVertex < tileVertices.Length; tileVertex++)
                {
                    uvs.Add(uv);
                    normals.Add(Vector3.up);
                }

                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 3);
            }

            _terrain.UpdateMesh(vertices, triangles, uvs, normals);
        }

        private float GetTileHeight(Tile mapTile)
        {
            return mapTile.TileType switch
            {
                TileType.Land => Constants.TerrainPositionY,
                TileType.Water => Constants.TerrainWaterPositionY,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private Vector2 GetMapTileUV(Tile mapTile)
        {
            return mapTile.TileType switch
            {
                TileType.Land => new Vector2(1, 1),
                TileType.Water => new Vector2(0, 0),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}