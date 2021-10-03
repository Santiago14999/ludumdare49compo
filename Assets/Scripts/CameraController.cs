using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _menuCameraOrigin;
    [SerializeField] private Transform _placingCameraOrigin;
    [SerializeField] private Transform _walkingCameraOrigin;
    [SerializeField] private float _followSpeed;
    [SerializeField] private Transform _cameraHolder;
    
    private Camera _camera;
    private Vector3 _menuCameraDefaultPosition;
    private Quaternion _menuCameraDefaultRotation;

    private Transform _activeOrigin;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        _menuCameraDefaultPosition = _menuCameraOrigin.position;
        _menuCameraDefaultRotation = _menuCameraOrigin.rotation;
        ActivateMenuCamera();
    }

    public void ActivateMenuCamera()
    {
        _activeOrigin = _menuCameraOrigin;
        _menuCameraOrigin.position = _menuCameraDefaultPosition;
        _menuCameraOrigin.rotation = _menuCameraDefaultRotation;
        _menuCameraOrigin.GetComponentInParent<Animator>().Play("Idle");
    }

    public void ActivatePlacingCamera()
    {
        _activeOrigin = _placingCameraOrigin;
    }

    public void ActivateWalkingCameraOrigin()
    {
        _activeOrigin = _walkingCameraOrigin;
    }

    public void UpdateCameraScale(float maxHeight)
    {
        StartCoroutine(CameraScaleCoroutine(maxHeight));
    }

    private IEnumerator CameraScaleCoroutine(float maxHeight)
    {
        _cameraHolder.transform.position = Vector3.up * maxHeight * .5f;

        float targetScale = 10 + maxHeight * .5f;
        float initialScale = _camera.orthographicSize;
        float startTime = Time.time;
        while (_camera.orthographicSize < targetScale)
        {
            _camera.orthographicSize = Mathf.Lerp(initialScale, targetScale, Time.time - startTime);
            yield return new WaitForEndOfFrame();
        }

        _camera.orthographicSize = targetScale;
    }

    private void Update()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _activeOrigin.position,
            Time.deltaTime * _followSpeed);

        _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, _activeOrigin.rotation,
            Time.deltaTime * _followSpeed);
    }
}
