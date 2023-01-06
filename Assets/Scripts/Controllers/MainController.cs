using Data;
using Services;
using UI;
using UnityEngine;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private MapSettingsWindow _mapSettingsWindow;
        [SerializeField] private CameraModeWindow _cameraModeWindow;

        [SerializeField] private CameraController _cameraController;
        [SerializeField] private TerrainController _terrainController;
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
            _cameraModeWindow.Init(_cameraController);
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            _mapService.UpdateMap(mapSettingsData);
            _terrainController.UpdateMap(_mapService.MapTiles);

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