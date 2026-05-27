using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerSettingsSO playerSettingsSo;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Cameras: ")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 45f;
    
    private float _yaw;
    private float _pitch;
    private Vector3 _movement;
    private bool _isGrounded;
    private bool _isCrouching;
    private bool _jumpRequested;
    private bool _isRunning = false;
    private float _standHeight = 2f;
    private float _crouchHeight = 1f;
    private Vector3 _standCameraPos;
    private Vector3 _crouchCameraPos;
    
    private Vector3 targetPoint;
    private Vector2 _lastMousePos;

    private HealthSystem _healthSystem;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    
    public event Action OnPlayerHurt;
    public event Action OnPlayerDied;


    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        _healthSystem.onDie += HealthSystemV2_onDie;
    }

    private void Start()
    {
        _standCameraPos = _camera.transform.localPosition; 
        _crouchCameraPos = new Vector3(_standCameraPos.x, _standCameraPos.y - 0.5f, _standCameraPos.z); 
    }

    void Update()
    {
        _movement = (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized;
        
        if (Input.GetKeyDown(playerSettingsSo.crouchKey))
            Crouch(true);
        else if (Input.GetKeyUp(playerSettingsSo.crouchKey))
            Crouch(false);
        
        Run();
        if (Input.GetKeyDown(playerSettingsSo.jumpKey))
            _jumpRequested = true;
        CameraRotate();

    }

    private void FixedUpdate()
    {
        Jump();
        Movement();
    }

    private void OnDestroy()
    {
        _healthSystem.onDie -= HealthSystemV2_onDie;
    }

    private void HealthSystemV2_onDie()
    {
        OnPlayerDied?.Invoke();
    }
    
    private void Run()
    {
        if (Input.GetKeyDown(playerSettingsSo.runkey))
            _isRunning = true; 
        else if (Input.GetKeyUp(playerSettingsSo.runkey))
            _isRunning = false;
    }
    
    private void Movement()
    {
        float currentY = _rigidbody.linearVelocity.y;
        
        if (_isRunning && !_isCrouching)
            _rigidbody.linearVelocity = new Vector3( _movement.x * playerSettingsSo.runSpeed, currentY, _movement.z * playerSettingsSo.runSpeed); 
        else if (!_isRunning  && !_isCrouching)
            _rigidbody.linearVelocity = new Vector3( _movement.x * playerSettingsSo.speed, currentY, _movement.z * playerSettingsSo.speed); 
        else if (_isCrouching)
            _rigidbody.linearVelocity = new Vector3( _movement.x * playerSettingsSo.crouchSpeed, currentY, _movement.z * playerSettingsSo.crouchSpeed); 
    }
    
    private void Jump()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        if (_jumpRequested && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * playerSettingsSo.jumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
        else
        {
            _jumpRequested = false;
        }
    }
    

    private void Crouch(bool crouch)
    {
        _isCrouching = crouch;
        _collider.height = crouch ? _crouchHeight : _standHeight;
        _camera.transform.localPosition = crouch ? _crouchCameraPos : _standCameraPos;
    }
    
    private void CameraRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * playerSettingsSo.mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * playerSettingsSo.mouseSens * Time.deltaTime;

        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, -30f, 60f);

        _camera.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
