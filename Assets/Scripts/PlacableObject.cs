using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlacableObject : MonoBehaviour
{
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _selectMaterial;

    private Rigidbody _rigidbody;
    private MeshRenderer[] _renderers;
    private Collider[] _colliders;
    
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
        foreach (MeshRenderer meshRenderer in _renderers)
            meshRenderer.sharedMaterial = _selectMaterial;
        foreach (Collider coll in _colliders)
            coll.enabled = false;
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
        if (Physics.Raycast(raycastOrigin, Vector3.down * 1000f, out var hitInfo))
        {
            transform.position = hitInfo.point;

        }
    }
}
