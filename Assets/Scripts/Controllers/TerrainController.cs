using System.Collections.Generic;
using Data;
using Extensions;
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

        private static readonly Vector3[] TileTopNormals = { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        private static readonly Point[] SideDirections = { Point.Left, Point.Up, Point.Right, Point.Down };

        private static readonly int[][] SideVertexIndexByDirection =
        {
            new[] { 0, 1 }, new[] { 1, 2 },
            new[] { 2, 3 }, new[] { 3, 0 },
        };

        public void UpdateMap(Dictionary<Point, Tile> mapTiles)
        {
            CreateTerrain(mapTiles);
        }

        private void CreateTerrain(Dictionary<Point, Tile> mapTiles)
        {
            List<Vector3> vertices = ListPool<Vector3>.Get();
            List<Vector2> uvs = ListPool<Vector2>.Get();
            List<int> triangles = ListPool<int>.Get();
            List<Vector3> normals = ListPool<Vector3>.Get();

            foreach (Tile mapTile in mapTiles.Values)
            {
                triangles.AddTileFaceTriangles(vertices.Count);
                vertices.AddRange(GetTileTopVertices(mapTile));

                Vector2 mapTileUV = GetMapTileUV(mapTile);
                uvs.AddTileFaceUVs(mapTileUV);

                normals.AddRange(TileTopNormals);

                if (!mapTile.Walkable && !mapTile.OnBorder)
                {
                    continue;
                }

                for (var sideDirectionIndex = 0; sideDirectionIndex < SideDirections.Length; sideDirectionIndex++)
                {
                    Point sideDirection = SideDirections[sideDirectionIndex];
                    Point neighbourPosition = GetNeighbourPosition(mapTile, sideDirection);

                    if (mapTile.Walkable && !NeighbourOutOfBounds(neighbourPosition))
                    {
                        Tile neighbourTile = mapTiles[neighbourPosition];
                        if (!neighbourTile.Walkable)
                        {
                            Vector3[] tileTopVertices = GetTileTopVertices(mapTile);

                            triangles.AddTileFaceTriangles(vertices.Count);
                            vertices.AddRange(GetTileSideVertices(sideDirectionIndex, tileTopVertices, Constants.TerrainWaterPositionY));
                            uvs.AddTileFaceUVs(mapTileUV);
                            normals.AddRange(GetTileSideNormals(sideDirection));
                        }
                    }

                    if (mapTile.OnBorder && NeighbourOutOfBounds(neighbourPosition))
                    {
                        Vector3[] tileTopVertices = GetTileTopVertices(mapTile);

                        triangles.AddTileFaceTriangles(vertices.Count);
                        vertices.AddRange(GetTileSideVertices(sideDirectionIndex, tileTopVertices, Constants.BorderSideBottomPositionY));
                        uvs.AddTileFaceUVs(mapTileUV);
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

        private static Vector3[] GetTileTopVertices(Tile mapTile)
        {
            Vector3 leftBottomVertex = new Vector3(
                StartVertex + mapTile.Position.X,
                GetTileHeight(mapTile),
                StartVertex + mapTile.Position.Y);
            Vector3 leftTopVertex = leftBottomVertex + Vector3.forward;
            Vector3 rightTopVertex = leftTopVertex + Vector3.right;
            Vector3 rightBottomVertex = leftBottomVertex + Vector3.right;

            return new[] { leftBottomVertex, leftTopVertex, rightTopVertex, rightBottomVertex };
        }

        private static IEnumerable<Vector3> GetTileSideVertices(int sideDirectionIndex, Vector3[] tileTopVertices,
            float bottomPositionY)
        {
            int edgeVertexIndexA = SideVertexIndexByDirection[sideDirectionIndex][0];
            int edgeVertexIndexB = SideVertexIndexByDirection[sideDirectionIndex][1];

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

        private static IEnumerable<Vector3> GetTileSideNormals(Point sideNormal)
        {
            return new Vector3[]
            {
                new(sideNormal.X, 0f, sideNormal.Y),
                new(sideNormal.X, 0f, sideNormal.Y),
                new(sideNormal.X, 0f, sideNormal.Y),
                new(sideNormal.X, 0f, sideNormal.Y),
            };
        }

        private static Point GetNeighbourPosition(Tile mapTile, Point sideDirection)
        {
            int neighbourPositionX = mapTile.Position.X + sideDirection.X;
            int neighbourPositionY = mapTile.Position.Y + sideDirection.Y;

            return new Point(neighbourPositionX, neighbourPositionY);
        }

        private static bool NeighbourOutOfBounds(Point tilePosition)
        {
            return tilePosition.X < 0 || tilePosition.X >= Constants.MapSize ||
                   tilePosition.Y < 0 || tilePosition.Y >= Constants.MapSize;
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