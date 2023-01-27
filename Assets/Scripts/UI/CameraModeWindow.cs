using Controllers.Camera;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CameraModeWindow : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _title;

        private CameraController _cameraController;

        public void Init(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(SwitchCameraMode);
        }

        private void SwitchCameraMode()
        {
            _cameraController.SwitchToNextMode();
            _title.text = _cameraController.CameraBehaviourMode.ToString();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(SwitchCameraMode);
        }
    }
}