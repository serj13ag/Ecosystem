using UnityEngine;

namespace Controllers.Camera
{
    public abstract class BaseCameraBehaviour
    {
        protected readonly UnityEngine.Camera Camera;
        protected readonly Vector3 MapCenterPosition;

        protected BaseCameraBehaviour(UnityEngine.Camera camera, Vector3 mapCenterPosition)
        {
            Camera = camera;
            MapCenterPosition = mapCenterPosition;
        }

        protected void ResetCameraToInitialParameters(Vector3 position, float fieldOfView)
        {
            Camera.transform.position = position;
            Camera.transform.LookAt(MapCenterPosition);

            Camera.fieldOfView = fieldOfView;
        }
    }
}