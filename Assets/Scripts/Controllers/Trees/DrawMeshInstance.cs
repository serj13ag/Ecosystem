using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Controllers.Trees
{
    public class DrawMeshInstance
    {
        private const int DrawMeshInstancedMaxMatricesAmount = 1023;

        private readonly TreeDrawMeshModel _drawModel;
        private readonly List<List<Matrix4x4>> _matricesSets;

        public IEnumerable<List<Matrix4x4>> MatricesSets => _matricesSets;
        public Mesh Mesh => _drawModel.Mesh;
        public Material[] SubMeshMaterials => _drawModel.SubMeshMaterials;

        public DrawMeshInstance(TreeDrawMeshModel drawModel)
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

        public void AddMatrix(Vector3 initialPosition, int angleRotation, Vector3 scale)
        {
            List<Matrix4x4> matricesSet = _matricesSets[^1];

            Matrix4x4 matrix = CreateTransformMatrix(initialPosition, angleRotation, scale);

            matricesSet.Add(matrix);

            if (matricesSet.Count >= matricesSet.Capacity)
            {
                AddNewMatricesSet();
            }
        }

        private Matrix4x4 CreateTransformMatrix(Vector3 initialPosition, int angleRotation, Vector3 scale)
        {
            Vector3 position = new Vector3(
                initialPosition.x,
                initialPosition.y + _drawModel.PivotOffsetY,
                initialPosition.z);

            Quaternion rotation = GetRotation(_drawModel.Rotation, angleRotation);

            return Matrix4x4.TRS(position, rotation, scale);
        }

        private void AddNewMatricesSet()
        {
            _matricesSets.Add(new List<Matrix4x4>(DrawMeshInstancedMaxMatricesAmount));
        }

        private static Quaternion GetRotation(Vector3 drawModelRotation, int angleRotation)
        {
            Quaternion modelRotation = Quaternion.Euler(drawModelRotation);

            Vector3 modelRotationAxis = modelRotation * Vector3.up;

            modelRotation *= Quaternion.AngleAxis(angleRotation, modelRotationAxis);
            return modelRotation;
        }
    }
}