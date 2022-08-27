using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //singleton
    public static Player playerInstance
    {
        get;
        set;
    }


    //movement
    public float speed;
    bool MovementEnabled = true;
    public bool Grounded;
    [SerializeField] float JumpForce;
    CharacterController controller;
    [SerializeField] float walkSpeed;
    [SerializeField] float SprintSpeed;
    [SerializeField] float SpeedMultiplier;
    public bool GrappleMode;

    //rotation
    [SerializeField] Transform cameraTransform;
    [SerializeField] float CameraSensitivity;
    float PlayerRotation = 0;
    float CameraRotation = 0;
    bool RotationEnabled = true;

    //Physcis
    Rigidbody playerRigidbody;

    public Vector3 moveDirection;
    private void Start()
    {
        playerInstance = this;
        playerRigidbody = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        controller = GetComponent<CharacterController>();
        
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        PlayerRotate();
    }
    void Movement()
    {
        if (MovementEnabled)
        {
            if(GrappleMode)
            {
                moveDirection.x = Input.GetAxis("Horizontal");
                moveDirection.z = Input.GetAxis("Vertical");
                moveDirection = cameraTransform.TransformDirection(moveDirection);
                playerRigidbody.AddForce(moveDirection);
            }

            else if (Grounded || Wallrun.WallRunInstance.WallRunMode)
            {
                //jump
                //movement
                moveDirection.x = Input.GetAxis("Horizontal");
                moveDirection.z = Input.GetAxis("Vertical");

                if (moveDirection.z > 0 && Input.GetAxis("Sprint") > 0)
                {
                    if (speed < SprintSpeed)
                    {
                        speed += Time.deltaTime;
                    }
                }
                else if (moveDirection.magnitude > 0)
                {
                    if (speed < walkSpeed)
                    {
                        speed += Time.deltaTime;
                    }
                    else if (speed > walkSpeed + 0.1)
                    {
                        speed -= Time.deltaTime;
                    }
                }
                else
                {
                    if (speed > 0)
                        speed -= Time.deltaTime;
                }
                moveDirection.Normalize();
                moveDirection *= speed*SpeedMultiplier;
                moveDirection = transform.TransformDirection(moveDirection);

                playerRigidbody.velocity = moveDirection;

                if (Input.GetKey(KeyCode.Space))
                {
                    playerRigidbody.AddForce(transform.up * JumpForce, ForceMode.VelocityChange);
                }


            }
            else
            {
                if (speed > 0)
                    speed -= Time.deltaTime;
            }
        }
        
    }
    void PlayerRotate()
    {
        if (RotationEnabled)
        {
            PlayerRotation += Input.GetAxisRaw("Mouse X") * CameraSensitivity;
            CameraRotation += Input.GetAxisRaw("Mouse Y") * CameraSensitivity;
            CameraRotation = Mathf.Clamp(CameraRotation, -90f, 30f);
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, PlayerRotation, transform.localRotation.eulerAngles.z);
            cameraTransform.localRotation = Quaternion.Euler(-CameraRotation, cameraTransform.localRotation.eulerAngles.y, cameraTransform.localRotation.eulerAngles.z);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = false;
        }
    }
}
