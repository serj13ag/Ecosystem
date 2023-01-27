using Enums;
using UnityEngine;

namespace Controllers.Camera
{
    public class RotateCameraBehaviour : ICameraBehaviour
    {
        private readonly UnityEngine.Camera _camera;
        private readonly Vector3 _mapCenterPosition;

        public CameraMode Mode => CameraMode.Rotate;

        public RotateCameraBehaviour(UnityEngine.Camera camera, Vector3 mapCenterPosition)
        {
            _camera = camera;
            _mapCenterPosition = mapCenterPosition;

            ResetCameraToInitialParameters();
        }

        public void Update(float deltaTime)
        {
            _camera.transform.RotateAround(_mapCenterPosition, Vector3.up, deltaTime * Constants.CameraRotationSpeed);
        }

        public void ResetCameraToInitialParameters()
        {
            _camera.transform.position = Constants.CameraInitialPositionRotateMode;
            _camera.transform.LookAt(_mapCenterPosition);

            _camera.fieldOfView = Constants.CameraRotateFieldOfView;
        }
    }
}