using System.Collections.Generic;
using Map;
using Prefabs;
using UnityEngine;
using UnityEngine.Pool;

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

        public void UpdateMap(Dictionary<Vector2Int, Tile> mapTiles)
        {
            CreateTerrain(mapTiles);
        }

        private void CreateTerrain(Dictionary<Vector2Int, Tile> mapTiles)
        {
            var vertices = ListPool<Vector3>.Get();
            var uvs = ListPool<Vector2>.Get();
            var triangles = ListPool<int>.Get();
            var normals = ListPool<Vector3>.Get();

            foreach (Tile mapTile in mapTiles.Values)
            {
                triangles.AddRange(GetTileFaceTriangles(vertices.Count));
                vertices.AddRange(GetTileTopVertices(mapTile));

                Vector2 mapTileUV = GetMapTileUV(mapTile);
                uvs.AddRange(GetTileFaceUVs(mapTileUV));

                normals.AddRange(_tileTopNormals);

                if (!mapTile.Walkable && !mapTile.OnBorder)
                {
                    continue;
                }

                for (var sideDirectionIndex = 0; sideDirectionIndex < _sideDirections.Length; sideDirectionIndex++)
                {
                    Vector2Int sideDirection = _sideDirections[sideDirectionIndex];
                    Vector2Int neighbourPosition = GetNeighbourPosition(mapTile, sideDirection);

                    if (mapTile.Walkable && !NeighbourOutOfBounds(neighbourPosition))
                    {
                        Tile neighbourTile = mapTiles[neighbourPosition];
                        if (!neighbourTile.Walkable)
                        {
                            Vector3[] tileTopVertices = GetTileTopVertices(mapTile);

                            triangles.AddRange(GetTileFaceTriangles(vertices.Count));
                            vertices.AddRange(GetTileSideVertices(sideDirectionIndex, tileTopVertices, Constants.TerrainWaterPositionY));
                            uvs.AddRange(GetTileFaceUVs(mapTileUV));
                            normals.AddRange(GetTileSideNormals(sideDirection));
                        }
                    }

                    if (mapTile.OnBorder && NeighbourOutOfBounds(neighbourPosition))
                    {
                        Vector3[] tileTopVertices = GetTileTopVertices(mapTile);

                        triangles.AddRange(GetTileFaceTriangles(vertices.Count));
                        vertices.AddRange(GetTileSideVertices(sideDirectionIndex, tileTopVertices, Constants.BorderSideBottomPositionY));
                        uvs.AddRange(GetTileFaceUVs(mapTileUV));
                        normals.AddRange(GetTileSideNormals(sideDirection));
                    }
                }
            }

            _terrain.UpdateMesh(vertices, triangles, uvs, normals);

            ListPool<Vector3>.Release(vertices);
            ListPool<Vector2>.Release(uvs);
            ListPool<int>.Release(triangles);
            ListPool<Vector3>.Release(normals);
        }

        private Vector3[] GetTileTopVertices(Tile mapTile)
        {
            Vector3 leftBottomVertex = new Vector3(
                StartVertex + mapTile.Position.x,
                GetTileHeight(mapTile),
                StartVertex + mapTile.Position.y);
            Vector3 leftTopVertex = new Vector3(leftBottomVertex.x, leftBottomVertex.y, leftBottomVertex.z + 1);
            Vector3 rightTopVertex = new Vector3(leftTopVertex.x + 1, leftTopVertex.y, leftTopVertex.z);
            Vector3 rightBottomVertex = new Vector3(leftBottomVertex.x + 1, leftBottomVertex.y, leftBottomVertex.z);

            return new[] { leftBottomVertex, leftTopVertex, rightTopVertex, rightBottomVertex };
        }

        private Vector3[] GetTileSideVertices(int sideDirectionIndex, Vector3[] tileTopVertices, float bottomPositionY)
        {
            int edgeVertexIndexA = _sideVertexIndexByDirection[sideDirectionIndex][0];
            int edgeVertexIndexB = _sideVertexIndexByDirection[sideDirectionIndex][1];

            Vector3 leftBottomVertex = new Vector3(
                tileTopVertices[edgeVertexIndexB].x,
                bottomPositionY,
                tileTopVertices[edgeVertexIndexB].z);
            Vector3 leftTopVertex = tileTopVertices[edgeVertexIndexB];
            Vector3 rightTopVertex = tileTopVertices[edgeVertexIndexA];
            Vector3 rightBottomVertex = new Vector3(
                tileTopVertices[edgeVertexIndexA].x,
                bottomPositionY,
                tileTopVertices[edgeVertexIndexA].z);

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