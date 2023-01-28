using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Controllers.Trees
{
    public class DrawMeshInstance
    {
        private const int DrawMeshInstancedMaxMatricesAmount = 1023;

        private readonly DrawMeshModel _drawModel;
        private readonly List<List<Matrix4x4>> _matricesSets;

        public IEnumerable<List<Matrix4x4>> MatricesSets => _matricesSets;
        public Mesh Mesh => _drawModel.Mesh;
        public Material[] SubMeshMaterials => _drawModel.SubMeshMaterials;

        public DrawMeshInstance(DrawMeshModel drawModel)
        {
            _drawModel = drawModel;

            _matricesSets = new List<List<Matrix4x4>>();

            AddNewMatricesSet();
        }

        public void ClearMatricesSets()
        {
            _matricesSets.Clear();

            AddNewMatricesSet();
        }

        public void AddPositionToMatrices(Vector3 position)
        {
            List<Matrix4x4> matricesSet = _matricesSets[^1];

            Vector3 modelPosition = new Vector3(position.x, position.y + _drawModel.PivotOffsetY, position.z);
            Matrix4x4 matrix = Matrix4x4.TRS(modelPosition, Quaternion.Euler(_drawModel.Rotation), Vector3.one);

            matricesSet.Add(matrix);

            if (matricesSet.Count >= DrawMeshInstancedMaxMatricesAmount)
            {
                AddNewMatricesSet();
            }
        }

        private void AddNewMatricesSet()
        {
            _matricesSets.Add(new List<Matrix4x4>());
        }
    }
}