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
        private TreeService _treeService;

        private bool _mapGenerated;

        private void Awake()
        {
            _localStorageService = new LocalStorageService();
            _mapService = new MapService();
            _treeService = new TreeService();

            _mapSettingsWindow.Init(this, _localStorageService);
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            _mapService.UpdateMap(mapSettingsData);
            _mapController.UpdateMap(_mapService.MapTiles);

            _mapGenerated = true;

            UpdateTrees(mapSettingsData.TreesPercentage);

            _localStorageService.Save(Constants.MapSettingsKey, mapSettingsData);
        }

        public void UpdateTrees(float treesPercentage)
        {
            if (!_mapGenerated)
            {
                return;
            }

            _treeService.GenerateTrees(treesPercentage, _mapService.GetLandTilesPositions());
            _treesController.UpdateTrees(_treeService.TreePositions);
        }
    }
}