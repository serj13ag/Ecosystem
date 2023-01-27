using Enums;
using UnityEngine;

namespace Controllers.Camera
{
    public class RotateCameraBehaviour : BaseCameraBehaviour, ICameraBehaviour
    {
        public CameraMode Mode => CameraMode.Rotate;

        public RotateCameraBehaviour(UnityEngine.Camera camera, Vector3 mapCenterPosition)
            : base(camera, mapCenterPosition)
        {
        }

        public void Update(float deltaTime)
        {
            Camera.transform.RotateAround(MapCenterPosition, Vector3.up, deltaTime * Constants.CameraRotationSpeed);
        }

        public void ResetCameraToInitialParameters()
        {
            ResetCameraToInitialParameters(Constants.CameraInitialPositionRotateMode, Constants.CameraRotateFieldOfView);
        }
    }
}