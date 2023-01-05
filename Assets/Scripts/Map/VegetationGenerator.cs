using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class VegetationGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _treePrefab;
        [SerializeField] private Transform _treesContainer;

        private readonly List<GameObject> _trees = new List<GameObject>();

        public void UpdateTrees(float treesPercentageValue, Vector3[] landTilesPositions)
        {
            if (_trees.Any())
            {
                foreach (var tree in _trees.ToArray())
                {
                    Destroy(tree);
                }

                _trees.Clear();
            }

            foreach (var landTilesPosition in landTilesPositions)
            {
                if (Random.Range(0, 100) < treesPercentageValue)
                {
                    var tree = Instantiate(_treePrefab, landTilesPosition, quaternion.identity, _treesContainer);
                    _trees.Add(tree);
                }
            }
            
            StaticBatchingUtility.Combine(_treesContainer.gameObject);
        }
    }
}