using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Extensions to optimize GC allocation
    /// </summary>
    public static class TerrainExtensions
    {
        public static void AddTileFaceUVs(this List<Vector2> uvs, Vector2 uv)
        {
            for (var i = 0; i < 4; i++)
            {
                uvs.Add(uv);
            }
        }

        public static void AddTileFaceTriangles(this List<int> triangles, int vertexIndex)
        {
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }

        public static void AddTileTopVertices(this List<Vector3> vertices, Tile mapTile, float startVertex,
            float tileHeight)
        {
            Vector3 leftBottomVertex = new Vector3(
                startVertex + mapTile.Position.X,
                tileHeight,
                startVertex + mapTile.Position.Y);
            Vector3 leftTopVertex = leftBottomVertex + Vector3.forward;
            Vector3 rightTopVertex = leftTopVertex + Vector3.right;
            Vector3 rightBottomVertex = leftBottomVertex + Vector3.right;

            vertices.Add(leftBottomVertex);
            vertices.Add(leftTopVertex);
            vertices.Add(rightTopVertex);
            vertices.Add(rightBottomVertex);
        }
    }
}