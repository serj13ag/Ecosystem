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

        public void AddMatrix(Vector3 initialPosition)
        {
            List<Matrix4x4> matricesSet = _matricesSets[^1];

            Matrix4x4 matrix = CreateTransformMatrix(initialPosition);

            matricesSet.Add(matrix);

            if (matricesSet.Count >= matricesSet.Capacity)
            {
                AddNewMatricesSet();
            }
        }

        private Matrix4x4 CreateTransformMatrix(Vector3 initialPosition)
        {
            Vector3 position = new Vector3(
                initialPosition.x,
                initialPosition.y + _drawModel.PivotOffsetY,
                initialPosition.z);

            Quaternion rotation = GetRandomRotation(_drawModel.Rotation);
            Vector3 scale = GetRandomScale();

            return Matrix4x4.TRS(position, rotation, scale);
        }

        private void AddNewMatricesSet()
        {
            _matricesSets.Add(new List<Matrix4x4>(DrawMeshInstancedMaxMatricesAmount));
        }

        private static Quaternion GetRandomRotation(Vector3 drawModelRotation)
        {
            Quaternion modelRotation = Quaternion.Euler(drawModelRotation);

            int randomAngle = Random.Range(0, Constants.TreeMeshMaxRotationAngle);
            Vector3 modelRotationAxis = modelRotation * Vector3.up;

            modelRotation *= Quaternion.AngleAxis(randomAngle, modelRotationAxis);
            return modelRotation;
        }

        private static Vector3 GetRandomScale()
        {
            float randomScale = Random.Range(Constants.TreeMeshScaleMin, Constants.TreeMeshScaleMax);

            return new Vector3(randomScale, randomScale, randomScale);
        }
    }
}