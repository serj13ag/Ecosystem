using System;
using Enums;
using Services;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private InputService _inputService;

        private Vector3 _mapCenterPosition;
        private CameraMode[] _cameraModes;

        public CameraMode CurrentMode { get; private set; }

        public void Init(InputService inputService)
        {
            _inputService = inputService;

            _mapCenterPosition = new Vector3(Constants.MapSize / 2f, 0, Constants.MapSize / 2f);
            _cameraModes = new[] { CameraMode.Rotate, CameraMode.Fly };

            CurrentMode = CameraMode.Rotate;

            ResetPositionAndRotation();
            UpdateFieldOfView();
        }

        private void Update()
        {
            switch (CurrentMode)
            {
                case CameraMode.Rotate:
                    HandleRotateModeUpdate();
                    break;
                case CameraMode.Fly:
                    HandleFlyModeUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SwitchMode()
        {
            if ((int)CurrentMode < _cameraModes.Length - 1)
            {
                CurrentMode++;
            }
            else
            {
                CurrentMode = 0;
            }

            ResetPositionAndRotation();
            UpdateFieldOfView();
        }

        private void HandleRotateModeUpdate()
        {
            _camera.transform.RotateAround(_mapCenterPosition, Vector3.up,
                Time.deltaTime * Constants.CameraRotationSpeed);
        }

        private void HandleFlyModeUpdate()
        {
            Transform cameraTransform = _camera.transform;

            if (_inputService.RightMouseButtonPressed)
            {
                Vector2 mouseAxis = _inputService.GetMouseAxis();

                Vector3 cameraTransformPosition = cameraTransform.position;
                cameraTransform.RotateAround(cameraTransformPosition, Vector3.down, -mouseAxis.x);
                cameraTransform.RotateAround(cameraTransformPosition, cameraTransform.right, -mouseAxis.y);
            }

            Vector3 moveAxis = _inputService.GetMoveAxis();
            cameraTransform.Translate(new Vector3(moveAxis.x, moveAxis.y, moveAxis.z));

            Vector3 cameraPosition = cameraTransform.position;
            float posX = Mathf.Clamp(cameraPosition.x, -100f, 300f);
            float posY = Mathf.Clamp(cameraPosition.y, 0f, 100f);
            float posZ = Mathf.Clamp(cameraPosition.z, -100f, 300f);

            cameraTransform.position = new Vector3(posX, posY, posZ);
        }

        private void UpdateFieldOfView()
        {
            _camera.fieldOfView = CurrentMode switch
            {
                CameraMode.Rotate => Constants.CameraRotateFieldOfView,
                CameraMode.Fly => Constants.CameraFlyFieldOfView,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private void ResetPositionAndRotation()
        {
            Vector3 position = CurrentMode switch
            {
                CameraMode.Rotate => Constants.CameraInitialPositionRotateMode,
                CameraMode.Fly => Constants.CameraInitialPositionFlyMode,
                _ => throw new ArgumentOutOfRangeException(),
            };

            _camera.transform.position = position;
            _camera.transform.LookAt(_mapCenterPosition);
        }
    }
}