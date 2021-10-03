using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    public event System.Action PlayerFelt;
    public event System.Action PlayerFinished;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private Transform _worldToMove;
    [SerializeField] private Transform _torso;
    [SerializeField] private Transform _plank;
    [SerializeField] private float _angleToFall;

    private float _currentSpeed;
    private bool _isWalking;
    private bool _moveWorld;
    private Animator _animator;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StartWalking()
    {
        _isWalking = true;
        _moveWorld = true;
        _animator.Play("Walk");
        _currentSpeed = 0f;
    }

    public void ResetPlayer()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _worldToMove.position = Vector3.zero;
        _animator.Play("Idle");
        _isWalking = false;
        _moveWorld = false;
        _torso.rotation = Quaternion.identity;
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (_moveWorld)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _speed, Time.deltaTime * _acceleration);
            _worldToMove.Translate(-Vector3.forward * _currentSpeed * Time.deltaTime);
        }
        
        if (!_isWalking)
            return;

        if (_worldToMove.position.z <= -23f)
        {
            Finish();
            return;
        }

        var rotation = _torso.eulerAngles;
        rotation.z = _plank.eulerAngles.z;
        _torso.eulerAngles = rotation;

        float z = rotation.z > 180f ? rotation.z - 360f : rotation.z;
        if (Mathf.Abs(z) >= _angleToFall)
        {
            Fall();
        }
    }

    private void Finish()
    {
        _moveWorld = false;
        _isWalking = false;
        _animator.Play("Finish");
        _torso.rotation = Quaternion.identity;
        _rigidbody.isKinematic = true;
        PlayerFinished?.Invoke();
    }

    private void Fall()
    {
        _isWalking = false;
        float angle = _torso.eulerAngles.z > 180f ? _torso.eulerAngles.z - 360f : _torso.eulerAngles.z;
        Vector3 torque = Vector3.forward * (angle > 0 ? 1f : -1f);
        _rigidbody.angularVelocity = torque;
        _rigidbody.isKinematic = false;
        PlayerFelt?.Invoke();
    }
}
