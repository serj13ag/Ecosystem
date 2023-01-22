using UnityEngine;

namespace Services
{
    public class InputService
    {
        public bool RightMouseButtonPressed => Input.GetMouseButton(1);

        public Vector2 GetMouseAxis()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public Vector3 GetMoveAxis()
        {
            return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Elevate"), Input.GetAxis("Vertical"));
        }
    }
}