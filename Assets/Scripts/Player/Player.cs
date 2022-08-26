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

    //rotation
    [SerializeField] Transform cameraTransform;
    [SerializeField] float CameraSensitivity;
    float PlayerRotation = 0;
    float CameraRotation = 0;
    bool RotationEnabled = true;

    //animation
    public Animator CurrentAnimator;

    //Physcis
    Rigidbody playerRigidbody;

    Vector3 moveDirection;
    private void Start()
    {
        playerInstance = this;
        playerRigidbody = GetComponent<Rigidbody>();
        
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
            if (Grounded || Wallrun.WallRunInstance.WallRunMode)
            {
                //movement
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                if (moveDirection.z > 0)
                    speed = 5 + Input.GetAxis("Sprint") * 10;
                else
                    speed = 5;
                moveDirection.Normalize();
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                moveDirection.y = playerRigidbody.velocity.y;
                playerRigidbody.velocity = moveDirection;

                CurrentAnimator.SetFloat("Speed", playerRigidbody.velocity.magnitude);
                //jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerRigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
                }
            }
            else
            {
                CurrentAnimator.SetFloat("Speed", 0);
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
