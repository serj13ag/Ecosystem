using System.Collections.Generic;
using Map;
using Prefabs;
using UnityEngine;

namespace Controllers
{
    public class TerrainController : MonoBehaviour
    {
        private const float StartVertex = -0.5f;

        [SerializeField] private TerrainPrefab _terrain;

        private readonly Vector3[] _tileTopNormals = { Vector3.up, Vector3.up, Vector3.up, Vector3.up };

        private readonly Vector2Int[] _sideDirections =
            { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };

        private readonly int[][] _sideVertexIndexByDirection =
        {
            new[] { 0, 1 }, new[] { 1, 2 },
            new[] { 2, 3 }, new[] { 3, 0 },
        };

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
                triangles.AddRange(GetTileFaceTriangles(vertices.Count));

                Vector3[] tileTopVertices = GetTileTopVertices(mapTile);
                vertices.AddRange(tileTopVertices);

                Vector2 mapTileUV = GetMapTileUV(mapTile);
                uvs.AddRange(GetTileFaceUVs(mapTileUV));

                normals.AddRange(_tileTopNormals);

                if (mapTile.OnBorder)
                {
                    for (var sideDirectionIndex = 0; sideDirectionIndex < _sideDirections.Length; sideDirectionIndex++)
                    {
                        Vector2Int sideDirection = _sideDirections[sideDirectionIndex];

                        var neighbourPosition = GetNeighbourPosition(mapTile, sideDirection);

                        if (NeighbourOutOfBounds(neighbourPosition))
                        {
                            triangles.AddRange(GetTileFaceTriangles(vertices.Count));
                            vertices.AddRange(GetTileSideVertices(sideDirectionIndex, tileTopVertices));
                            uvs.AddRange(GetTileFaceUVs(mapTileUV));
                            normals.AddRange(GetTileSideNormals(sideDirection));
                        }
                    }
                }
            }

            _terrain.UpdateMesh(vertices, triangles, uvs, normals);
        }

        private Vector3[] GetTileTopVertices(Tile mapTile)
        {
            Vector3 leftBottomVertex = new(
                StartVertex + mapTile.Position.x,
                GetTileHeight(mapTile),
                StartVertex + mapTile.Position.y);
            Vector3 leftTopVertex = leftBottomVertex + Vector3.forward;
            Vector3 rightTopVertex = leftTopVertex + Vector3.right;
            Vector3 rightBottomVertex = leftBottomVertex + Vector3.right;

            return new[] { leftBottomVertex, leftTopVertex, rightTopVertex, rightBottomVertex };
        }

        private Vector3[] GetTileSideVertices(int sideDirectionIndex, Vector3[] tileTopVertices)
        {
            int edgeVertexIndexA = _sideVertexIndexByDirection[sideDirectionIndex][0];
            int edgeVertexIndexB = _sideVertexIndexByDirection[sideDirectionIndex][1];

            Vector3 leftBottomVertex = tileTopVertices[edgeVertexIndexB] + Vector3.down;
            Vector3 leftTopVertex = tileTopVertices[edgeVertexIndexB];
            Vector3 rightTopVertex = tileTopVertices[edgeVertexIndexA];
            Vector3 rightBottomVertex = tileTopVertices[edgeVertexIndexA] + Vector3.down;

            return new[] { leftBottomVertex, leftTopVertex, rightTopVertex, rightBottomVertex };
        }

        private Vector3[] GetTileSideNormals(Vector2Int sideNormal)
        {
            return new Vector3[]
            {
                new(sideNormal.x, 0f, sideNormal.y),
                new(sideNormal.x, 0f, sideNormal.y),
                new(sideNormal.x, 0f, sideNormal.y),
                new(sideNormal.x, 0f, sideNormal.y),
            };
        }

        private Vector2Int GetNeighbourPosition(Tile mapTile, Vector2Int sideDirection)
        {
            int neighbourPositionX = mapTile.Position.x + sideDirection.x;
            int neighbourPositionY = mapTile.Position.y + sideDirection.y;

            return new Vector2Int(neighbourPositionX, neighbourPositionY);
        }

        private bool NeighbourOutOfBounds(Vector2Int tilePosition)
        {
            return tilePosition.x < 0 || tilePosition.x >= Constants.MapSize ||
                   tilePosition.y < 0 || tilePosition.y >= Constants.MapSize;
        }

        private static int[] GetTileFaceTriangles(int vertexIndex)
        {
            return new[]
            {
                vertexIndex, vertexIndex + 1, vertexIndex + 2,
                vertexIndex, vertexIndex + 2, vertexIndex + 3,
            };
        }

        private static Vector2[] GetTileFaceUVs(Vector2 uv)
        {
            return new[] { uv, uv, uv, uv };
        }

        private static float GetTileHeight(Tile mapTile)
        {
            return mapTile.Walkable
                ? Constants.TerrainPositionY
                : Constants.TerrainWaterPositionY;
        }

        private static Vector2 GetMapTileUV(Tile mapTile)
        {
            float uv = mapTile.IsShallow
                ? Constants.ShallowUV
                : mapTile.Height;
            return new Vector2(uv, uv);
        }
    }
}