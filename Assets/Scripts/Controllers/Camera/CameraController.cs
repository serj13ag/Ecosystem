using System;
using Enums;
using Services;
using UnityEngine;
using static Constants;

namespace Controllers.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        private InputService _inputService;

        private Vector3 _mapCenterPosition;
        private CameraMode[] _cameraModes;

        private ICameraBehaviour _cameraBehaviour;

        public CameraMode CameraBehaviourMode => _cameraBehaviour.Mode;

        public void Init(InputService inputService)
        {
            _inputService = inputService;

            _mapCenterPosition = new Vector3(MapSize / 2f, 0, MapSize / 2f);
            _cameraModes = new[] { CameraMode.Rotate, CameraMode.Fly };

            ChangeCameraBehaviour(CameraMode.Rotate);
        }

        private void Update()
        {
            _cameraBehaviour.Update(Time.deltaTime);
        }

        public void SwitchToNextMode()
        {
            CameraMode currentMode = _cameraBehaviour.Mode;

            CameraMode newCameraMode = (int)currentMode < _cameraModes.Length - 1
                ? currentMode + 1
                : 0;

            ChangeCameraBehaviour(newCameraMode);
        }

        private void ChangeCameraBehaviour(CameraMode cameraMode)
        {
            if (_cameraBehaviour?.Mode == cameraMode)
            {
                return;
            }

            _cameraBehaviour = CreateCameraBehaviour(cameraMode);

            _cameraBehaviour.ResetCameraToInitialParameters();
        }

        private ICameraBehaviour CreateCameraBehaviour(CameraMode cameraMode)
        {
            return cameraMode switch
            {
                CameraMode.Rotate => new RotateCameraBehaviour(_camera, _mapCenterPosition),
                CameraMode.Fly => new FlyCameraBehaviour(_camera, _inputService, _mapCenterPosition),
                _ => throw new ArgumentOutOfRangeException(nameof(cameraMode), cameraMode, null),
            };
        }
    }
}