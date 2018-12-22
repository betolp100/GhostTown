using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] private bool cameraInverse=false;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowMultiplier = 2f;
    private bool canJump = false;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0;
    private float currentCameraRotationX = 0f;
    private Vector3 jumpForce = Vector3.zero;
    private Rigidbody rb;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotation)
    {
        cameraRotationX = _cameraRotation;
    }


    void FixedUpdate()
    {
        if (PauseMenu.isMenuOn) return;
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y*Time.deltaTime;
        }

    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation*Quaternion.Euler(rotation));
        if (cam != null)
        {
            if (cameraInverse == false)
            {
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

                cam.transform.localEulerAngles=new Vector3(currentCameraRotationX,0,0);
            }
            else
            {
                currentCameraRotationX += cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
            } 

        }
    }

    public void ApplyJumpForce(float _jumpForce)
    {
        rb.velocity = Vector3.up* _jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Grass"|| other.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Grass" || other.gameObject.tag == "Ground")
        {
            canJump = false;
        }
    }

    public bool CanJump()
    {
        return canJump;
    }
}
