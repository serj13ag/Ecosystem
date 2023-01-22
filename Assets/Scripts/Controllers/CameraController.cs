using System;
using Enums;
using Services;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _speed;

        private InputService _inputService;

        private Vector3 _mapCenterPosition;
        private CameraMode[] _cameraModes;

        private Vector3 _cameraInitialPosition;
        private Quaternion _cameraInitialRotation;

        public CameraMode CurrentMode { get; private set; }

        public void Init(InputService inputService)
        {
            _inputService = inputService;

            _mapCenterPosition = new Vector3(Constants.MapSize / 2f, 0, Constants.MapSize / 2f);
            _cameraModes = new[] { CameraMode.Rotate, CameraMode.Fly };

            _camera.transform.LookAt(_mapCenterPosition);

            _cameraInitialPosition = _camera.transform.position;
            _cameraInitialRotation = _camera.transform.rotation;

            CurrentMode = CameraMode.Rotate;

            UpdateFieldOfView();
        }

        private void Update()
        {
            Transform cameraTransform = _camera.transform;

            switch (CurrentMode)
            {
                case CameraMode.Rotate:
                    cameraTransform.RotateAround(_mapCenterPosition, Vector3.up, Time.deltaTime * _speed);
                    break;
                case CameraMode.Fly:
                    if (_inputService.RightMouseButtonPressed)
                    {
                        Vector2 mouseAxis = _inputService.GetMouseAxis();

                        Vector3 cameraTransformPosition = cameraTransform.position;
                        cameraTransform.RotateAround(cameraTransformPosition, Vector3.down, -mouseAxis.x);
                        cameraTransform.RotateAround(cameraTransformPosition, cameraTransform.right, -mouseAxis.y);
                    }

                    Vector2 moveAxis = _inputService.GetMoveAxis();

                    cameraTransform.Translate(new Vector3(moveAxis.x, 0, moveAxis.y));

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

            _camera.transform.position = _cameraInitialPosition;
            _camera.transform.rotation = _cameraInitialRotation;

            UpdateFieldOfView();
        }

        private void UpdateFieldOfView()
        {
            switch (CurrentMode)
            {
                case CameraMode.Rotate:
                    _camera.fieldOfView = Constants.CameraRotateFieldOfView;
                    break;
                case CameraMode.Fly:
                    _camera.fieldOfView = Constants.CameraFlyFieldOfView;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}