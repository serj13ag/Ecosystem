using System.Collections.Generic;
using DataTypes;
using UnityEngine;

namespace Controllers
{
    public class PlantController : MonoBehaviour
    {
        [SerializeField] private GameObject _plantPrefab;

        private List<GameObject> _plants;

        public void UpdatePlants(HashSet<Point> plantLocations)
        {
            _plants ??= new List<GameObject>();

            if (_plants.Count > 0)
            {
                foreach (GameObject plant in _plants)
                {
                    Destroy(plant);
                }

                _plants.Clear();
            }

            foreach (Point plantLocation in plantLocations)
            {
                Vector3 position = new Vector3(plantLocation.X, 0, plantLocation.Y);
                GameObject plant = Instantiate(_plantPrefab, position, Quaternion.identity);

                _plants.Add(plant);
            }
        }
    }
}