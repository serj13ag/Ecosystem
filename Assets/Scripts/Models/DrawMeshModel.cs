using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Models/Draw Mesh Model", fileName = "DrawModel")]
    public class DrawMeshModel : ScriptableObject
    {
        public Mesh Mesh;
        public Material[] SubMeshMaterials;
        public Vector3 Rotation;
        public float PivotOffsetY;
    }
}