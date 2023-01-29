using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;
using Tree = Map.Tree;

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

        public void UpdateTrees(IEnumerable<Tree> trees)
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

        private void UpdateDrawMeshInstances(IEnumerable<Tree> trees)
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances.Values)
            {
                drawMeshInstance.ClearMatricesSets();
            }

            foreach (Tree tree in trees)
            {
                _drawMeshInstances[tree.Type].AddMatrix(tree.Position, tree.AngleRotation, tree.Scale);
            }
        }
    }
}