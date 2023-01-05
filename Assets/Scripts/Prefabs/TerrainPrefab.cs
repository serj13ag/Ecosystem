using System.Collections.Generic;
using UnityEngine;

namespace Prefabs
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TerrainPrefab : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private Mesh _mesh;

        public void InitMeshComponents()
        {
            _mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
            };

            _meshFilter.sharedMesh = _mesh;

            _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        public void InitMesh(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, List<Vector3> normals,
            Material material)
        {
            _mesh.SetVertices(vertices);
            _mesh.SetTriangles(triangles, 0, true);
            _mesh.SetUVs(0, uvs);
            _mesh.SetNormals(normals);

            _meshRenderer.sharedMaterial = material;
        }
    }
}