using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class TreesController : MonoBehaviour
    {
        [SerializeField] private GameObject _treePrefab;
        [SerializeField] private Transform _treesContainer;

        private List<GameObject> _trees;

        private void Awake()
        {
            _trees = new List<GameObject>();
        }

        public void UpdateTrees(float treesPercentageValue, Vector2Int[] landTilesPositions)
        {
            if (_trees.Any())
            {
                ResetTrees();
            }

            CreateTrees(treesPercentageValue, landTilesPositions);

            StaticBatchingUtility.Combine(_treesContainer.gameObject);
        }

        private void CreateTrees(float treesPercentageValue, Vector2Int[] landTilesPositions)
        {
            foreach (var landTilesPosition in landTilesPositions)
            {
                if (Random.Range(0, 100) > treesPercentageValue)
                {
                    continue;
                }

                var position = new Vector3(landTilesPosition.x, Constants.TerrainPositionY, landTilesPosition.y);
                GameObject tree = Instantiate(_treePrefab, position, Quaternion.identity, _treesContainer);

                _trees.Add(tree);
            }
        }

        private void ResetTrees()
        {
            foreach (GameObject tree in _trees.ToArray())
            {
                Destroy(tree);
            }

            _trees.Clear();
        }
    }
}