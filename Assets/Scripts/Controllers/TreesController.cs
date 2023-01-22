using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers
{
    public class TreesController : MonoBehaviour
    {
        private const int DrawMeshInstancedMaxMatricesAmount = 1023;

        [SerializeField] private Mesh _treeMesh;
        [SerializeField] private Material[] _subMeshMaterials;

        private List<List<Matrix4x4>> _drawInstances;

        private void Awake()
        {
            _drawInstances = new List<List<Matrix4x4>>();
        }

        private void Update()
        {
            RenderInstances();
        }

        public void UpdateTrees(HashSet<Vector2Int> treePositions)
        {
            if (_drawInstances.Any())
            {
                _drawInstances.Clear();
            }

            CreateDrawInstances(treePositions);
        }

        private void RenderInstances()
        {
            foreach (List<Matrix4x4> transformMatrices in _drawInstances)
            {
                for (var i = 0; i < _treeMesh.subMeshCount; i++)
                {
                    Graphics.DrawMeshInstanced(_treeMesh, i, _subMeshMaterials[i], transformMatrices);
                }
            }
        }

        private void CreateDrawInstances(HashSet<Vector2Int> treePositions)
        {
            _drawInstances.Add(new List<Matrix4x4>());
            var addedMatrices = 0;

            foreach (Vector2Int treePosition in treePositions)
            {
                if (addedMatrices >= DrawMeshInstancedMaxMatricesAmount)
                {
                    _drawInstances.Add(new List<Matrix4x4>());
                    addedMatrices = 0;
                }

                var position = new Vector3(treePosition.x, Constants.TerrainPositionY + 1, treePosition.y);
                _drawInstances[^1].Add(Matrix4x4.TRS(position, Quaternion.identity, Vector3.one));

                addedMatrices++;
            }
        }
    }
}