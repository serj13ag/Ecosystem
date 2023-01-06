using Controllers;
using Data;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class MapSettingsWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _seedInputField;
        [SerializeField] private Button _seedRandomize;

        [SerializeField] private Slider _refinement;
        [SerializeField] private Slider _waterLevel;

        [SerializeField] private Slider _treesPercentage;
        [SerializeField] private Button _treesRandomizePositions;

        [SerializeField] private Button _saveMap;

        private MainController _mainController;
        private LocalStorageService _localStorageService;

        private int _seed;

        private int Seed
        {
            get => _seed;
            set
            {
                _seed = value;

                _seedInputField.text = value.ToString();
            }
        }

        public void Init(MainController mainController, LocalStorageService localStorageService)
        {
            _mainController = mainController;
            _localStorageService = localStorageService;

            if (localStorageService.TryLoad(Constants.MapSettingsKey, out MapSettingsData mapSettingsData))
            {
                Seed = mapSettingsData.Seed;
                _refinement.value = mapSettingsData.Refinement;
                _waterLevel.value = mapSettingsData.WaterLevel;
                _treesPercentage.value = mapSettingsData.TreesPercentage;
            }
            else
            {
                Seed = Constants.DefaultSeed;
                _refinement.value = Constants.RefinementDefaultValue;
                _waterLevel.value = Constants.WaterLevelDefaultValue;
                _treesPercentage.value = Constants.TreesPercentageDefaultValue;
            }
        }

        private void OnEnable()
        {
            _seedRandomize.onClick.AddListener(OnSeedRandomizeButtonClick);

            _refinement.onValueChanged.AddListener(UpdateMap);
            _waterLevel.onValueChanged.AddListener(UpdateMap);

            _treesPercentage.onValueChanged.AddListener(UpdateMap);
            _treesRandomizePositions.onClick.AddListener(OnTreesRandomizePositionsButtonClick);

            _saveMap.onClick.AddListener(OnSaveMapButtonClick);
        }

        private void OnSeedRandomizeButtonClick()
        {
            Seed = Random.Range(0, Constants.MaxSeedValue);

            UpdateMap();
        }

        private void OnTreesRandomizePositionsButtonClick()
        {
            _mainController.UpdateTrees(_treesPercentage.value);
        }

        private void OnSaveMapButtonClick()
        {
            _localStorageService.Save(Constants.MapSettingsKey, CreateMapSettingsData());
        }

        private void UpdateMap(float arg0)
        {
            UpdateMap();
        }

        private void UpdateMap()
        {
            _mainController.UpdateMap(CreateMapSettingsData());
        }

        private MapSettingsData CreateMapSettingsData()
        {
            return new MapSettingsData(Seed, _refinement.value, _waterLevel.value, _treesPercentage.value);
        }

        private void OnDisable()
        {
            _seedRandomize.onClick.RemoveListener(OnSeedRandomizeButtonClick);

            _refinement.onValueChanged.RemoveListener(UpdateMap);
            _waterLevel.onValueChanged.RemoveListener(UpdateMap);

            _treesPercentage.onValueChanged.RemoveListener(UpdateMap);
            _treesRandomizePositions.onClick.RemoveListener(OnTreesRandomizePositionsButtonClick);

            _saveMap.onClick.RemoveListener(OnSaveMapButtonClick);
        }
    }
}