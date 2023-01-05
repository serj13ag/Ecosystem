using UnityEngine;

namespace Prefabs
{
    public class TilePrefab : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public Material MeshRendererSharedMaterial
        {
            get => _meshRenderer.sharedMaterial;
            set => _meshRenderer.sharedMaterial = value;
        }
    }
}