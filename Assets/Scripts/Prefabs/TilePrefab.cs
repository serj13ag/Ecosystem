using UnityEngine;

namespace Prefabs
{
    public class TilePrefab : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public Material MeshRendererSharedMaterial
        {
            set => _meshRenderer.sharedMaterial = value;
        }
    }
}