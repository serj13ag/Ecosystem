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

        [SerializeField] private Slider _scale;
        [SerializeField] private Slider _waterLevel;

        [SerializeField] private Slider _treesPercentage;

        [SerializeField] private Button _saveMap;

        private MainController _mainController;
        private LocalStorageService _localStorageService;
        private RandomService _randomService;

        private int _seed;

        private int Seed
        {
            get => _seed;
            set
            {
                _seed = value;

                _seedInputField.text = value.ToString();

                _randomService.SetMapSeed(value);
            }
        }

        public void Init(MainController mainController, LocalStorageService localStorageService,
            RandomService randomService)
        {
            _mainController = mainController;
            _randomService = randomService;
            _localStorageService = localStorageService;

            if (localStorageService.TryLoad(Constants.MapSettingsKey, out MapSettingsData mapSettingsData))
            {
                Seed = mapSettingsData.Seed;
                _scale.value = mapSettingsData.Scale;
                _waterLevel.value = mapSettingsData.WaterLevel;
                _treesPercentage.value = mapSettingsData.TreesPercentage;
            }
            else
            {
                Seed = Constants.DefaultSeed;
                _scale.value = Constants.ScaleDefaultValue;
                _waterLevel.value = Constants.WaterLevelDefaultValue;
                _treesPercentage.value = Constants.TreesPercentageDefaultValue;
            }
        }

        private void OnEnable()
        {
            _seedInputField.onValueChanged.AddListener(OnSeedInputValueChanged);
            _seedRandomize.onClick.AddListener(OnSeedRandomizeButtonClick);

            _scale.onValueChanged.AddListener(UpdateMap);
            _waterLevel.onValueChanged.AddListener(UpdateMap);

            _treesPercentage.onValueChanged.AddListener(UpdateMap);

            _saveMap.onClick.AddListener(OnSaveMapButtonClick);
        }

        private void OnSeedInputValueChanged(string value)
        {
            Seed = Mathf.Min(int.Parse(value), Constants.MaxSeedValue);

            UpdateMap();
        }

        private void OnSeedRandomizeButtonClick()
        {
            Seed = Random.Range(0, Constants.MaxSeedValue);

            UpdateMap();
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
            return new MapSettingsData(Seed, _scale.value, _waterLevel.value, _treesPercentage.value);
        }

        private void OnDisable()
        {
            _seedInputField.onValueChanged.RemoveListener(OnSeedInputValueChanged);
            _seedRandomize.onClick.RemoveListener(OnSeedRandomizeButtonClick);

            _scale.onValueChanged.RemoveListener(UpdateMap);
            _waterLevel.onValueChanged.RemoveListener(UpdateMap);

            _treesPercentage.onValueChanged.RemoveListener(UpdateMap);

            _saveMap.onClick.RemoveListener(OnSaveMapButtonClick);
        }
    }
}