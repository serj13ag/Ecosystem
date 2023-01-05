using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _speed;

        private Vector3 _mapCenterPosition;

        private void Awake()
        {
            _mapCenterPosition = new Vector3(Constants.MapSize / 2f, 0, Constants.MapSize / 2f);
        }

        private void Start()
        {
            _camera.transform.LookAt(_mapCenterPosition);
        }

        private void Update()
        {
            _camera.transform.RotateAround(_mapCenterPosition, Vector3.up, Time.deltaTime * _speed);
        }
    }
}