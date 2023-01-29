using System.Collections.Generic;
using Entities;
using Enums;
using Models;
using UnityEngine;

namespace Controllers.Trees
{
    public class TreesController : MonoBehaviour
    {
        [SerializeField] private TreeDrawMeshModel[] _drawMeshModels;

        private Dictionary<TreeType, DrawMeshInstance> _drawMeshInstances;

        private void Awake()
        {
            _drawMeshInstances = CreateDrawMeshInstances();
        }

        private void Update()
        {
            RenderDrawMeshInstances();
        }

        public void UpdateTrees(IEnumerable<TileTree> trees)
        {
            UpdateDrawMeshInstances(trees);
        }

        private Dictionary<TreeType, DrawMeshInstance> CreateDrawMeshInstances()
        {
            var drawMeshInstances = new Dictionary<TreeType, DrawMeshInstance>();

            foreach (TreeDrawMeshModel drawModel in _drawMeshModels)
            {
                drawMeshInstances.Add(drawModel.TreeType, new DrawMeshInstance(drawModel));
            }

            return drawMeshInstances;
        }

        private void RenderDrawMeshInstances()
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances.Values)
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

        private void UpdateDrawMeshInstances(IEnumerable<TileTree> trees)
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances.Values)
            {
                drawMeshInstance.ClearMatricesSets();
            }

            foreach (TileTree tree in trees)
            {
                _drawMeshInstances[tree.Type].AddMatrix(tree.Position, tree.AngleRotation, tree.Scale);
            }
        }
    }
}