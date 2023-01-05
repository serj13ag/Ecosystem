using Data;
using Map;
using Services;
using UI;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] private MapSettings _mapSettings;

    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private VegetationGenerator _vegetationGenerator;

    private LocalStorageService _localStorageService;

    private void Awake()
    {
        _localStorageService = new LocalStorageService();

        _mapSettings.Init(this, _localStorageService);
    }

    public void UpdateMap(MapSettingsData mapSettingsData)
    {
        _mapGenerator.UpdateMap(mapSettingsData);

        var landTilesPositions = _mapGenerator.GetLandTilesPositions();
        _vegetationGenerator.UpdateTrees(mapSettingsData.TreesPercentage, landTilesPositions);

        _localStorageService.Save(Constants.MapSettingsKey, mapSettingsData);
    }
}