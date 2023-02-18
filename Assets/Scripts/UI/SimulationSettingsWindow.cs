﻿using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SimulationSettingsWindow : BaseWindow
    {
        [SerializeField] private Button _back;
        [SerializeField] private Button _generatePlants;

        private MainController _mainController;

        public void Init(MainController mainController)
        {
            _mainController = mainController;
        }

        private void OnEnable()
        {
            _back.onClick.AddListener(ShowMapSettingsWindow);
            _generatePlants.onClick.AddListener(GeneratePlants);
        }

        private void ShowMapSettingsWindow()
        {
            Hide();
            _mainController.ShowMapSettingsWindow();
        }

        private void GeneratePlants()
        {
            _mainController.GeneratePlants();
        }

        private void OnDisable()
        {
            _back.onClick.RemoveListener(ShowMapSettingsWindow);
            _generatePlants.onClick.RemoveListener(GeneratePlants);
        }
    }
}