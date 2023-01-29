using System.Collections.Generic;
using Models;
using UnityEngine;
using Tree = Map.Tree;

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

        public void UpdateTrees(IEnumerable<Tree> trees)
        {
            UpdateDrawMeshInstances(trees);
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

        private void UpdateDrawMeshInstances(IEnumerable<Tree> trees)
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances)
            {
                drawMeshInstance.ClearMatricesSets();
            }

            foreach (Tree tree in trees)
            {
                _drawMeshInstances[(int)tree.Type].AddMatrix(tree.Position, tree.AngleRotation, tree.Scale);
            }
        }
    }
}