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

        [SerializeField] private Button _generateMap;

        private MainController _mainController;

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
            _treesRandomizePositions.onClick.AddListener(OnTreesRandomizePositionsButtonClick);
            _generateMap.onClick.AddListener(OnGenerateMapButtonClick);
        }

        private void OnSeedRandomizeButtonClick()
        {
            Seed = Random.Range(0, Constants.MaxSeedValue);
        }

        private void OnTreesRandomizePositionsButtonClick()
        {
            UpdateMap();
        }

        private void OnGenerateMapButtonClick()
        {
            UpdateMap();
        }

        private void UpdateMap()
        {
            var mapSettingsData =
                new MapSettingsData(Seed, _refinement.value, _waterLevel.value, _treesPercentage.value);
            _mainController.UpdateMap(mapSettingsData);
        }

        private void OnDisable()
        {
            _seedRandomize.onClick.RemoveListener(OnSeedRandomizeButtonClick);
            _treesRandomizePositions.onClick.RemoveListener(OnTreesRandomizePositionsButtonClick);
            _generateMap.onClick.AddListener(OnGenerateMapButtonClick);
        }
    }
}