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
        private InputService _inputService;
        private MapService _mapService;
        private TreeService _treeService;

        private bool _mapGenerated;

        private void Awake()
        {
            _localStorageService = new LocalStorageService();
            _inputService = new InputService();
            _mapService = new MapService();
            _treeService = new TreeService();

            _cameraController.Init(_inputService);

            _mapSettingsWindow.Init(this, _localStorageService);
            _cameraModeWindow.Init(_cameraController);
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            _mapService.UpdateMap(mapSettingsData);
            _terrainController.UpdateMap(_mapService.MapTiles);

            _mapGenerated = true;

            UpdateTrees(mapSettingsData.TreesPercentage);
        }

        public void UpdateTrees(float treesPercentage)
        {
            if (!_mapGenerated)
            {
                return;
            }

            _treeService.GenerateTrees(treesPercentage, _mapService.GetSuitableForPlantsTilesPositions());
            _treesController.UpdateTrees(_treeService.TreePositions);
        }
    }
}