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
        [SerializeField] private SimulationSettingsWindow _simulationSettingsWindow;
        [SerializeField] private CameraModeWindow _cameraModeWindow;

        [SerializeField] private CameraController _cameraController;
        [SerializeField] private TerrainController _terrainController;
        [SerializeField] private TreesController _treesController;
        [SerializeField] private PlantController _plantController;

        private LocalStorageService _localStorageService;
        private InputService _inputService;
        private RandomService _randomService;
        private MapService _mapService;
        private TreeService _treeService;
        private PlantService _plantService;

        private void Awake()
        {
            _localStorageService = new LocalStorageService();
            _inputService = new InputService();
            _randomService = new RandomService();
            _mapService = new MapService(_randomService);
            _treeService = new TreeService(_randomService);
            _plantService = new PlantService(_randomService);
        }

        private void Start()
        {
            _cameraController.Init(_inputService);

            _mapSettingsWindow.Init(this, _localStorageService, _randomService);
            _simulationSettingsWindow.Init(this);
            _cameraModeWindow.Init(_cameraController);
        }

        public void UpdateMap(MapSettingsData mapSettingsData)
        {
            _mapService.GenerateMapTiles(mapSettingsData);
            _treeService.GenerateTrees(_mapService.MapTiles.Values);

            _terrainController.UpdateMap(_mapService.MapTiles);
            _treesController.UpdateTrees(_treeService.Trees);
        }

        public void GeneratePlants()
        {
            _plantService.GeneratePlants(_mapService.MapTiles);
            _plantController.UpdatePlants(_plantService.PlantLocations);
        }

        public void ShowSimulationSettingsWindow()
        {
            _simulationSettingsWindow.Show();
        }

        public void ShowMapSettingsWindow()
        {
            ClearPlants();
            _mapSettingsWindow.Show();
        }

        private void ClearPlants()
        {
            _plantService.ClearPlants();
            _plantController.ClearPlants();
        }
    }
}