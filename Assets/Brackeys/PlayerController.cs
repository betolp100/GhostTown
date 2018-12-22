using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float runningSpeed = 9f;
    [SerializeField]
    private float staminaBurn = 1;
    [SerializeField]
    private float regenStamina = 0.25f;
    private float stamina = 1f;//BRACKEYS LE PUSO 1
    [SerializeField]
    private float lookSensitivity = 5f;
    public float jumpVelocity;
    private bool isRunning = false;
    private bool exhausted = false;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    public float GetStaminaAmount()
    {
        return stamina;
    }

    private void Update()
    {
        if (PauseMenu.isMenuOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;
            return;
        }
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;


        if (Input.GetButton("Sprint") && stamina > 0) isRunning = true;
        else if (Input.GetButtonUp("Sprint") || stamina <= 0) isRunning = false;

        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");
        if (_xMov != 0 || _zMove != 0)
        {
            Vector3 _horizontal = transform.right * _xMov;
            Vector3 _vertical = transform.forward * _zMove;
            float _speed;
            if (isRunning == true && exhausted == false) _speed = runningSpeed;
            else _speed = speed;
            Vector3 _velocity = (_horizontal + _vertical).normalized * _speed;

            motor.Move(_velocity);
            if (isRunning == true && exhausted == false)
            {
                stamina -= staminaBurn * Time.deltaTime;
                if (stamina <= 0 && exhausted == false) exhausted = true;
            }
            else
            {
                stamina += regenStamina * Time.deltaTime;
                if (stamina > .5f && exhausted == true) exhausted = false;
            }
        }
        else
        {//EstamosParados
            motor.Move(new Vector3(0, 0, 0));
            if (stamina < 1)
            {
                stamina += regenStamina * Time.deltaTime;
                if (stamina > .5f && exhausted == true) exhausted = false;
            }
        }
        stamina=Mathf.Clamp(stamina,0,1);
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot,0f)*lookSensitivity;

        motor.Rotate(_rotation);

        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRot * lookSensitivity;

        motor.RotateCamera(_cameraRotationX);
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump")&&motor.CanJump())
        {
            motor.ApplyJumpForce(jumpVelocity);
        }
    }
}
