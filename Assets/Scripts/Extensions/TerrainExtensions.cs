using System.Collections.Generic;
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
    }
}