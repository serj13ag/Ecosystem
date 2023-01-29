using System.Collections.Generic;
using Data;
using Models;
using UnityEngine;

namespace Controllers.Trees
{
    public class TreesController : MonoBehaviour
    {
        [SerializeField] private DrawMeshModel[] _drawMeshModels;

        private List<DrawMeshInstance> _drawMeshInstances;

        private void Awake()
        {
            _drawMeshInstances = CreateDrawMeshInstances();
        }

        private void Update()
        {
            RenderDrawMeshInstances();
        }

        public void UpdateTrees(IEnumerable<Point> treePositions)
        {
            UpdateDrawMeshInstances(treePositions);
        }

        private List<DrawMeshInstance> CreateDrawMeshInstances()
        {
            var drawMeshInstances = new List<DrawMeshInstance>();

            foreach (DrawMeshModel drawModel in _drawMeshModels)
            {
                drawMeshInstances.Add(new DrawMeshInstance(drawModel));
            }

            return drawMeshInstances;
        }

        private void RenderDrawMeshInstances()
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances)
            {
                foreach (List<Matrix4x4> matricesSet in drawMeshInstance.MatricesSets)
                {
                    for (var i = 0; i < drawMeshInstance.Mesh.subMeshCount; i++)
                    {
                        Graphics.DrawMeshInstanced(drawMeshInstance.Mesh, i, drawMeshInstance.SubMeshMaterials[i],
                            matricesSet);
                    }
                }
            }
        }

        private void UpdateDrawMeshInstances(IEnumerable<Point> treePositions)
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances)
            {
                drawMeshInstance.ClearMatricesSets();
            }

            foreach (Point treePosition in treePositions)
            {
                AddPositionToRandomDrawMeshInstance(treePosition);
            }
        }

        private void AddPositionToRandomDrawMeshInstance(Point treePosition)
        {
            int randomInstanceIndex = ((treePosition.X ^ 2) + (treePosition.Y ^ 2)) % _drawMeshInstances.Count;

            Vector3 position = new Vector3(treePosition.X, Constants.TerrainPositionY, treePosition.Y);
            _drawMeshInstances[randomInstanceIndex].AddMatrix(position);
        }
    }
}