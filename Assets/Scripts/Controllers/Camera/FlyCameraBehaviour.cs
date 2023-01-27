using Enums;
using Services;
using UnityEngine;

namespace Controllers.Camera
{
    public class FlyCameraBehaviour : ICameraBehaviour
    {
        private readonly InputService _inputService;

        private readonly UnityEngine.Camera _camera;
        private readonly Vector3 _mapCenterPosition;

        public CameraMode Mode => CameraMode.Fly;

        public FlyCameraBehaviour(UnityEngine.Camera camera, InputService inputService, Vector3 mapCenterPosition)
        {
            _camera = camera;
            _inputService = inputService;
            _mapCenterPosition = mapCenterPosition;
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
                MoveCamera(moveAxis);
            }
        }

        public void ResetCameraToInitialParameters()
        {
            _camera.transform.position = Constants.CameraInitialPositionFlyMode;
            _camera.transform.LookAt(_mapCenterPosition);

            _camera.fieldOfView = Constants.CameraFlyFieldOfView;
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

            newPosition.y = Mathf.Max(newPosition.y, Constants.CameraMinPositionY);

            Vector3 offsetFromCenter = newPosition - _mapCenterPosition;
            _camera.transform.position = _mapCenterPosition +
                                         Vector3.ClampMagnitude(offsetFromCenter,
                                             Constants.CameraMaxDistanceFromCenter);
        }

        private Vector3 CalculateMovement(Transform cameraTransform, Vector3 moveAxis)
        {
            Vector3 movement = Vector3.zero;

            float cameraSpeed = _inputService.ShiftPressed
                ? Constants.CameraFlySpeedWithShift
                : Constants.CameraFlySpeed;

            if (moveAxis.x != 0)
            {
                movement += cameraTransform.right * (moveAxis.x * Time.deltaTime * cameraSpeed);
            }

            if (moveAxis.y != 0)
            {
                movement += Vector3.up * (moveAxis.y * Time.deltaTime * cameraSpeed);
            }

            if (moveAxis.z != 0)
            {
                movement += cameraTransform.forward * (moveAxis.z * Time.deltaTime * cameraSpeed);
            }

            return movement;
        }
    }
}