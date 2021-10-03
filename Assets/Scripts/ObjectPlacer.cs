using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour
{
    public event System.Action ObjectPlaced;
    
    [SerializeField] private PlacableObject[] _objects;
    [SerializeField] private Rigidbody _plank;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraController _cameraController;
    
    private List<PlacableObject> _placedObjects = new List<PlacableObject>();
    private PlacableObject _selectedObject;
    private float _maxHeight;
    private Vector3 _plankDefaultPos;

    private void Awake()
    {
        _plankDefaultPos = _plank.transform.position;
        LoadPlacedObjects();
    }

    public void SelectRandomObject()
    {
        _selectedObject = Instantiate(_objects[Random.Range(0, _objects.Length)]);
        _selectedObject.Select();
        if (_placedObjects.Count == 0)
            _maxHeight = 0f;
        else
            _maxHeight = _placedObjects.OrderByDescending(o => o.transform.position.y).First().transform.position.y;
    }

    public void LoadPlacedObjects()
    {
        _plank.transform.position = _plankDefaultPos;
        _plank.velocity = Vector3.zero;
        _plank.angularVelocity = Vector3.zero;
        _plank.transform.rotation = Quaternion.identity;
        _plank.isKinematic = true;
        foreach (var obj in _placedObjects)
        {
            obj.LoadPlace();
        }
    }

    public void ActivateObjects()
    {
        _plank.isKinematic = false;
        foreach (var obj in _placedObjects)
        {
            obj.Activate();
        }
    }

    private void Update()
    {
        if (_selectedObject == null)
            return;

        Vector3 raycastOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        raycastOrigin.z = _plank.transform.position.z;
        raycastOrigin.y = _maxHeight + 10f;

        _selectedObject.UpdateSelectedPosition(raycastOrigin);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _selectedObject.Place();
            _selectedObject.PlacedPosition = _selectedObject.transform.position;
            _placedObjects.Add(_selectedObject);
            _selectedObject = null;
            _cameraController.UpdateCameraScale(_maxHeight);
            ObjectPlaced?.Invoke();
        }
    }
}
