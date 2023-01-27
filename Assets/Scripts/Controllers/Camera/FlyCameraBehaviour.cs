using Enums;
using Services;
using UnityEngine;

namespace Controllers.Camera
{
    public class FlyCameraBehaviour : BaseCameraBehaviour, ICameraBehaviour
    {
        private readonly InputService _inputService;

        public CameraMode Mode => CameraMode.Fly;

        public FlyCameraBehaviour(UnityEngine.Camera camera, InputService inputService, Vector3 mapCenterPosition)
            : base(camera, mapCenterPosition)
        {
            _inputService = inputService;
        }

        public void Update(float deltaTime)
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
                MoveCamera(moveAxis, deltaTime);
            }
        }

        public void ResetCameraToInitialParameters()
        {
            ResetCameraToInitialParameters(Constants.CameraInitialPositionFlyMode, Constants.CameraFlyFieldOfView);
        }

        private void RotateCamera(Vector2 mouseAxis)
        {
            Vector3 cameraTransformPosition = Camera.transform.position;

            Camera.transform.RotateAround(cameraTransformPosition, Vector3.down, -mouseAxis.x);
            Camera.transform.RotateAround(cameraTransformPosition, Camera.transform.right, -mouseAxis.y);
        }

        private void MoveCamera(Vector3 moveAxis, float deltaTime)
        {
            Vector3 movement = CalculateMovement(Camera.transform, moveAxis, deltaTime);
            Vector3 newPosition = Camera.transform.position + movement;

            newPosition.y = Mathf.Max(newPosition.y, Constants.CameraMinPositionY);

            Vector3 offsetFromCenter = newPosition - MapCenterPosition;
            Camera.transform.position = MapCenterPosition +
                                        Vector3.ClampMagnitude(offsetFromCenter, Constants.CameraMaxDistanceFromCenter);
        }

        private Vector3 CalculateMovement(Transform cameraTransform, Vector3 moveAxis, float deltaTime)
        {
            Vector3 movement = Vector3.zero;

            float cameraSpeed = _inputService.ShiftPressed
                ? Constants.CameraFlySpeedWithShift
                : Constants.CameraFlySpeed;

            if (moveAxis.x != 0)
            {
                movement += cameraTransform.right * (moveAxis.x * deltaTime * cameraSpeed);
            }

            if (moveAxis.y != 0)
            {
                movement += Vector3.up * (moveAxis.y * deltaTime * cameraSpeed);
            }

            if (moveAxis.z != 0)
            {
                movement += cameraTransform.forward * (moveAxis.z * deltaTime * cameraSpeed);
            }

            return movement;
        }
    }
}