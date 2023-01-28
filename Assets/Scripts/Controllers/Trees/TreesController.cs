﻿using System.Collections.Generic;
using Data;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public void UpdateTrees(HashSet<Point> treePositions)
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

        private void UpdateDrawMeshInstances(HashSet<Point> treePositions)
        {
            foreach (DrawMeshInstance drawMeshInstance in _drawMeshInstances)
            {
                drawMeshInstance.ClearMatricesSets();
            }

            foreach (Point treePosition in treePositions)
            {
                Vector3 position = new Vector3(treePosition.X, Constants.TerrainPositionY, treePosition.Y);
                AddPositionToRandomDrawMeshInstance(position);
            }
        }

        private void AddPositionToRandomDrawMeshInstance(Vector3 position)
        {
            int randomInstanceIndex = Random.Range(0, _drawMeshInstances.Count);
            _drawMeshInstances[randomInstanceIndex].AddMatrix(position);
        }
    }
}