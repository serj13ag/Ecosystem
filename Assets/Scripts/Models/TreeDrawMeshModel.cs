using Enums;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Models/Draw Mesh Model", fileName = "DrawModel")]
    public class TreeDrawMeshModel : ScriptableObject
    {
        public TreeType TreeType;
        public Mesh Mesh;
        public Material[] SubMeshMaterials;
        public Vector3 Rotation;
        public float PivotOffsetY;
    }
}