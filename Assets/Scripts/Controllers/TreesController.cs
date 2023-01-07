using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers
{
    public class TreesController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _treePrefabs;
        [SerializeField] private Transform _treesContainer;

        private List<GameObject> _spawnedTreePrefabs;

        private void Awake()
        {
            _spawnedTreePrefabs = new List<GameObject>();
        }

        public void UpdateTrees(HashSet<Vector2Int> treePositions)
        {
            if (_spawnedTreePrefabs.Any())
            {
                ResetTrees();
            }

            foreach (Vector2Int treePosition in treePositions)
            {
                var position = new Vector3(treePosition.x, Constants.TerrainPositionY, treePosition.y);
                GameObject tree = Instantiate(GetRandomTreePrefab(), position, Quaternion.identity, _treesContainer);

                _spawnedTreePrefabs.Add(tree);
            }
        }

        private GameObject GetRandomTreePrefab()
        {
            return _treePrefabs[Random.Range(0, _treePrefabs.Length)];
        }

        private void ResetTrees()
        {
            foreach (GameObject tree in _spawnedTreePrefabs.ToArray())
            {
                Destroy(tree);
            }

            _spawnedTreePrefabs.Clear();
        }
    }
}