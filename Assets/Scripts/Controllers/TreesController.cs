using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers
{
    public class TreesController : MonoBehaviour
    {
        [SerializeField] private GameObject _treePrefab;
        [SerializeField] private Transform _treesContainer;

        private List<GameObject> _treePrefabs;

        private void Awake()
        {
            _treePrefabs = new List<GameObject>();
        }

        public void UpdateTrees(HashSet<Vector2Int> treePositions)
        {
            if (_treePrefabs.Any())
            {
                ResetTrees();
            }

            foreach (Vector2Int treePosition in treePositions)
            {
                var position = new Vector3(treePosition.x, Constants.TerrainPositionY, treePosition.y);
                GameObject tree = Instantiate(_treePrefab, position, Quaternion.identity, _treesContainer);

                _treePrefabs.Add(tree);
            }
        }

        private void ResetTrees()
        {
            foreach (GameObject tree in _treePrefabs.ToArray())
            {
                Destroy(tree);
            }

            _treePrefabs.Clear();
        }
    }
}