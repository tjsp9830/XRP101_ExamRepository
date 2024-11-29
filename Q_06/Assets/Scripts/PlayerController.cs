using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    private Gun _gun;
    
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float _verticalRotateRange;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Camera _playerCam;

    private float _verticalRotation = 0f;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Move();
        Fire();
    }

    private void Init()
    {
        _gun = GetComponent<Gun>();
        
        CameraController cam = _playerCam.GetComponent<CameraController>();
        cam.FollowTarget = _muzzlePoint;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            _gun.Fire(_muzzlePoint);    
        }
    }

    private void Move()
    {
        Vector2 rotateInput = new(
            rotateInput.x = Input.GetAxis("Mouse X"),
            rotateInput.y = Input.GetAxis("Mouse Y")
            );
        SetRotation(rotateInput);
        
        Vector3 moveInput = new(
            Input.GetAxis("Horizontal"), 
            0, 
            Input.GetAxis("Vertical")
            );
        SetPosition(moveInput);
    }

    private void SetRotation(Vector2 input)
    {
        if (input.x != 0)
        {
            transform.Rotate(Vector3.up, input.x * _rotateSpeed * Time.deltaTime);
        }
        
        if (input.y != 0)
        {
            _verticalRotation = Mathf.Clamp(
                _verticalRotation + -input.y * _rotateSpeed * Time.deltaTime, 
                -_verticalRotateRange, 
                _verticalRotateRange
                );
            _muzzlePoint.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }
    }

    private void SetPosition(Vector3 input)
    {
        if (input == Vector3.zero) return;

        Vector3 moveDirection = transform.forward * input.z + transform.right * input.x;
        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
    }
}