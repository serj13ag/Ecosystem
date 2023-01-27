using Enums;

namespace Controllers.Camera
{
    public interface ICameraBehaviour
    {
        CameraMode Mode { get; }

        void Update(float deltaTime);
        void ResetCameraToInitialParameters();
    }
}