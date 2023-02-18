using System.Collections.Generic;
using DataTypes;
using UnityEngine;

namespace Controllers
{
    public class PlantController : MonoBehaviour
    {
        [SerializeField] private GameObject _plantPrefab;

        private List<GameObject> _plants;

        private void Awake()
        {
            _plants = new List<GameObject>();
        }

        public void UpdatePlants(HashSet<Point> plantLocations)
        {
            ResetPlants();

            foreach (Point plantLocation in plantLocations)
            {
                Vector3 position = new Vector3(plantLocation.X, 0, plantLocation.Y);
                GameObject plant = Instantiate(_plantPrefab, position, Quaternion.identity);

                _plants.Add(plant);
            }
        }

        public void ClearPlants()
        {
            ResetPlants();
        }

        private void ResetPlants()
        {
            if (_plants.Count <= 0)
            {
                return;
            }

            foreach (GameObject plant in _plants)
            {
                Destroy(plant);
            }

            _plants.Clear();
        }
    }
}