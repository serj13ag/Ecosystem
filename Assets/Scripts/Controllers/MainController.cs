using Controllers.Camera;
using Controllers.Trees;
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
        private RandomService _randomService;
        private MapService _mapService;
        private TreeService _treeService;

        private void Awake()
        {
            _localStorageService = new LocalStorageService();
            _inputService = new InputService();
            _randomService = new RandomService();
            _mapService = new MapService(_randomService);
            _treeService = new TreeService(_randomService);

            _cameraController.Init(_inputService);

            _mapSettingsWindow.Init(this, _localStorageService, _randomService);
            _cameraModeWindow.Init(_cameraController);
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            _mapService.GenerateMapTiles(mapSettingsData);
            _treeService.GenerateTrees(_mapService.MapTiles.Values);

            _terrainController.UpdateMap(_mapService.MapTiles);
            _treesController.UpdateTrees(_treeService.Trees);
        }
    }
}