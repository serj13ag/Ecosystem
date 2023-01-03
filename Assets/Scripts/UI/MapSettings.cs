using Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class MapSettings : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _seedInputField;
        [SerializeField] private Button _seedRandomize;

        [SerializeField] private Slider _refinement;
        [SerializeField] private Slider _waterLevel;

        [SerializeField] private MapGenerator _mapGenerator;

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

        private void Awake()
        {
            Seed = Constants.DefaultSeed;
            _refinement.value = Constants.RefinementDefaultValue;
            _waterLevel.value = Constants.WaterLevelDefaultValue;
        }

        private void OnEnable()
        {
            _seedInputField.onValueChanged.AddListener(OnSeedValueChanged);
            _seedRandomize.onClick.AddListener(OnSeedRandomizeButtonClick);

            _refinement.onValueChanged.AddListener(OnRefinementValueChanged);
            _waterLevel.onValueChanged.AddListener(OnWaterLevelValueChanged);
        }

        private void Start()
        {
            UpdateMap();
        }

        private void OnSeedValueChanged(string value)
        {
            UpdateMap();
        }

        private void OnSeedRandomizeButtonClick()
        {
            Seed = Random.Range(0, 999999);
        }

        private void OnRefinementValueChanged(float value)
        {
            UpdateMap();
        }

        private void OnWaterLevelValueChanged(float value)
        {
            UpdateMap();
        }

        private void UpdateMap()
        {
            _mapGenerator.UpdateMap(Seed, _refinement.value, _waterLevel.value);
        }

        private void OnDisable()
        {
            _seedInputField.onValueChanged.RemoveListener(OnSeedValueChanged);
            _seedRandomize.onClick.RemoveListener(OnSeedRandomizeButtonClick);

            _refinement.onValueChanged.RemoveListener(OnRefinementValueChanged);
            _waterLevel.onValueChanged.RemoveListener(OnWaterLevelValueChanged);
        }
    }
}