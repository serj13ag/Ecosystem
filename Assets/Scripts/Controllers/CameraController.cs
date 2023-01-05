using System;
using Enums;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _speed;

        private Vector3 _mapCenterPosition;
        private CameraMode[] _cameraModes;

        private Vector3 _cameraInitialPosition;
        private Quaternion _cameraInitialRotation;

        public CameraMode CurrentMode { get; private set; }

        private void Awake()
        {
            _mapCenterPosition = new Vector3(Constants.MapSize / 2f, 0, Constants.MapSize / 2f);
            _cameraModes = new[] { CameraMode.Rotate, CameraMode.Fly };
        }

        private void Start()
        {
            _camera.transform.LookAt(_mapCenterPosition);

            _cameraInitialPosition = _camera.transform.position;
            _cameraInitialRotation = _camera.transform.rotation;

            CurrentMode = CameraMode.Rotate;
        }

        private void Update()
        {
            switch (CurrentMode)
            {
                case CameraMode.Rotate:
                    _camera.transform.RotateAround(_mapCenterPosition, Vector3.up, Time.deltaTime * _speed);
                    break;
                case CameraMode.Fly:
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
        }
    }
}