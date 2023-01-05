using Data;
using Services;
using UI;
using UnityEngine;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private MapSettingsWindow _mapSettingsWindow;

        [SerializeField] private MapController _mapController;
        [SerializeField] private TreesController _treesController;

        private LocalStorageService _localStorageService;
        private MapService _mapService;

        private void Awake()
        {
            _localStorageService = new LocalStorageService();
            _mapService = new MapService();

            _mapSettingsWindow.Init(this, _localStorageService);
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            _mapService.UpdateMap(mapSettingsData);

            _mapController.UpdateMap(_mapService.MapTiles);

            Vector2Int[] landTilesPositions = _mapService.GetLandTilesPositions();
            _treesController.UpdateTrees(mapSettingsData.TreesPercentage, landTilesPositions);

            _localStorageService.Save(Constants.MapSettingsKey, mapSettingsData);
        }
    }
}