using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlacableObject : MonoBehaviour
{
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _selectMaterial;

    private Rigidbody _rigidbody;
    private MeshRenderer[] _renderers;
    private Collider[] _colliders;
    private Vector3 _boundsSize;
    
    public Vector3 PlacedPosition { get; set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _colliders = GetComponents<Collider>();
    }

    public void Place()
    {
        foreach (MeshRenderer meshRenderer in _renderers)
            meshRenderer.sharedMaterial = _mainMaterial;
        
        foreach (Collider coll in _colliders)
            coll.enabled = true;
    }

    public void Select()
    {
        _rigidbody.isKinematic = true;
        _boundsSize = GenerateBounds();

        foreach (MeshRenderer meshRenderer in _renderers)
            meshRenderer.sharedMaterial = _selectMaterial;
        foreach (Collider coll in _colliders)
            coll.enabled = false;

        print(_boundsSize);
    }

    public void Activate()
    {
        _rigidbody.isKinematic = false;
    }

    public void LoadPlace()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = PlacedPosition;
    }

    public void UpdateSelectedPosition(Vector3 raycastOrigin)
    {
        if (Physics.BoxCast(raycastOrigin, _boundsSize * .5f, Vector3.down * 1000f, out var hitInfo))
        {
            transform.position = new Vector3(raycastOrigin.x, hitInfo.point.y, raycastOrigin.z);
        }
    }

    private Vector3 GenerateBounds()
    {
        Vector3 max = Vector3.one * -10000f;
        Vector3 min = Vector3.one * 10000f;
        
        foreach (var c in _colliders)
        {
            max = Vector3.Max(max, c.bounds.max);
            print("Max: " + max);
            min = Vector3.Min(min, c.bounds.min);
            print("Min: " + min);
        }

        return max - min;
    }
}
