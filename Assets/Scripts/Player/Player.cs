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
    [SerializeField] float GrappleMoveForce;
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

    //Physics
    Rigidbody playerRigidbody;
    public Vector3 moveDirection;

    //player Hands and Weapons
    [SerializeField] PlayerAnimation[] playerHandsWeapons;
    [SerializeField] KeyCode[] WeaponKeybinds;
    public bool[] UnlockedWeapons;
    public PlayerAnimation CurrentWeapon;
    public PlayerAnimation NextWeapon;

    //Health
    public int Health=100;
    public int BulletDamage = 10;

    private void Start()
    {
        playerInstance = this;
        playerRigidbody = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        controller = GetComponent<CharacterController>();
        playerHandsWeapons = GetComponentsInChildren<PlayerAnimation>(true);
        
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        PlayerRotate();
        WeaponChange();
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
                moveDirection.Normalize();
                playerRigidbody.AddForce(moveDirection*GrappleMoveForce,ForceMode.Acceleration);
                if (speed > 0)
                    speed -= Time.deltaTime;
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
                else if (moveDirection.x != 0 || moveDirection.z != 0)
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
                moveDirection.x *= speed*SpeedMultiplier;
                moveDirection.z *= speed * SpeedMultiplier;
                moveDirection = transform.TransformDirection(moveDirection);

                playerRigidbody.velocity = moveDirection;
                if (!Wallrun.WallRunInstance.WallRunMode)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        playerRigidbody.AddForce((transform.up * JumpForce), ForceMode.VelocityChange);
                    }
                }
            }
            else
            {
                if (speed > 0)
                    speed -= Time.deltaTime;
            }
        }
        
    }
    void WeaponChange()
    {
        for(int i=0;i<WeaponKeybinds.Length;i++)
        {
            if(Input.GetKey(WeaponKeybinds[i]) && UnlockedWeapons[i])
            {
                if (CurrentWeapon != playerHandsWeapons[i])
                {
                    if (CurrentWeapon != null)
                    {
                        CurrentWeapon.HideWeapon();
                        NextWeapon = playerHandsWeapons[i];
                        
                    }
                    
                }
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Health -= BulletDamage;
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
