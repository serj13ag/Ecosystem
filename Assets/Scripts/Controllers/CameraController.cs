using System;
using Enums;
using Services;
using UnityEngine;
using static Constants;

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

            _mapCenterPosition = new Vector3(MapSize / 2f, 0, MapSize / 2f);
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
            _camera.transform.RotateAround(_mapCenterPosition, Vector3.up, Time.deltaTime * CameraRotationSpeed);
        }

        private void HandleFlyModeUpdate()
        {
            if (_inputService.RightMouseButtonPressed)
            {
                Vector2 mouseAxis = _inputService.GetMouseAxis();
                if (mouseAxis != Vector2.zero)
                {
                    RotateCamera(mouseAxis);
                }
            }

            Vector3 moveAxis = _inputService.GetMoveAxis();
            if (moveAxis != Vector3.zero)
            {
                MoveCamera(moveAxis);
            }
        }

        private void RotateCamera(Vector2 mouseAxis)
        {
            Vector3 cameraTransformPosition = _camera.transform.position;

            _camera.transform.RotateAround(cameraTransformPosition, Vector3.down, -mouseAxis.x);
            _camera.transform.RotateAround(cameraTransformPosition, _camera.transform.right, -mouseAxis.y);
        }

        private void MoveCamera(Vector3 moveAxis)
        {
            Vector3 movement = CalculateMovement(_camera.transform, moveAxis);
            Vector3 newPosition = _camera.transform.position + movement;

            newPosition.y = Mathf.Max(newPosition.y, CameraMinPositionY);

            Vector3 offsetFromCenter = newPosition - _mapCenterPosition;
            _camera.transform.position = _mapCenterPosition +
                                         Vector3.ClampMagnitude(offsetFromCenter, CameraMaxDistanceFromCenter);
        }

        private Vector3 CalculateMovement(Transform cameraTransform, Vector3 moveAxis)
        {
            Vector3 movement = Vector3.zero;

            if (moveAxis.x != 0)
            {
                movement += cameraTransform.right * moveAxis.x;
            }

            if (moveAxis.y != 0)
            {
                movement += Vector3.up * moveAxis.y;
            }

            if (moveAxis.z != 0)
            {
                movement += cameraTransform.forward * moveAxis.z;
            }

            return movement;
        }

        private void UpdateFieldOfView()
        {
            _camera.fieldOfView = CurrentMode switch
            {
                CameraMode.Rotate => CameraRotateFieldOfView,
                CameraMode.Fly => CameraFlyFieldOfView,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private void ResetPositionAndRotation()
        {
            Vector3 newPosition = CurrentMode switch
            {
                CameraMode.Rotate => CameraInitialPositionRotateMode,
                CameraMode.Fly => CameraInitialPositionFlyMode,
                _ => throw new ArgumentOutOfRangeException(),
            };

            _camera.transform.position = newPosition;
            _camera.transform.LookAt(_mapCenterPosition);
        }
    }
}