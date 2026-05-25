using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerSettingsSO playerSettingsSo;
    [SerializeField] private LayerMask groundLayer;

    [Header("Cameras: ")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 45f;
    
    private float _yaw;
    private float _pitch;
    private float _horizontal;
    private float _vertical;
    private bool _isGrounded;
    
    private Vector3 targetPoint;
    private Vector2 _lastMousePos;

    private HealthSystem _healthSystem;
    private Rigidbody _rigidbody;

    public event Action OnPlayerHurt;
    public event Action OnPlayerDied;


    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _rigidbody = GetComponent<Rigidbody>();

        _healthSystem.onDie += HealthSystemV2_onDie;
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        CameraRotate();

    }

    private void FixedUpdate()
    {
        Vector3 direction = (transform.forward * _vertical + transform.right * _horizontal).normalized;
        _rigidbody.linearVelocity = direction * playerSettingsSo.speed;
        
        
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        if (Input.GetButtonDown("Jump") && _isGrounded)
            _rigidbody.AddForce(Vector3.up * playerSettingsSo.jumpForce, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        _healthSystem.onDie -= HealthSystemV2_onDie;
    }

    private void HealthSystemV2_onDie()
    {
        OnPlayerDied?.Invoke();
    }

    private void CameraRotate()
    {
        float mouseX, mouseY;

        mouseX = Input.GetAxis("Mouse X") * playerSettingsSo.mouseSens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * playerSettingsSo.mouseSens * Time.deltaTime;

        _yaw += mouseX;
        _pitch -= mouseY;

        _pitch = Mathf.Clamp(_pitch, -30f, 60f);
        _yaw = Mathf.Clamp(_yaw, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
        transform.localRotation = Quaternion.Euler(0, _yaw, 0);
    }
}
