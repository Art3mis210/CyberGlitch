using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RengeGames.HealthBars;
using UnityEngine.SceneManagement;

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
    public bool MovementEnabled = true;
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
    bool RotationEnabled = false;

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
    public float Health=3;
    public float BulletDamage = 0.1f;
    [SerializeField] UltimateCircularHealthBar healthBar;

    private void Start()
    {
        Application.targetFrameRate = -1;
        playerInstance = this;
        playerRigidbody = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        controller = GetComponent<CharacterController>();
        playerHandsWeapons = GetComponentsInChildren<PlayerAnimation>(true);
        PlayerRotation = transform.localRotation.eulerAngles.y;
    }
    // Update is called once per frame
    void Update()
    {
       // Movement();
        PlayerRotate();
        WeaponChange();
    }
    private void FixedUpdate()
    {
        Movement();
        Jump();
    }
    void Jump()
    {
        if (MovementEnabled)
        {
            if (Grounded && !Wallrun.WallRunInstance.WallRunMode)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    playerRigidbody.AddForce((transform.up * JumpForce), ForceMode.VelocityChange);
                }
            }
        }
    }
    void Movement()         //manages Player movement according to keyboard input
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
                    if (!RotationEnabled)
                        RotationEnabled = true;
                    if (speed < SprintSpeed)
                    {
                        speed += Time.deltaTime;
                    }
                }
                else if (moveDirection.x != 0 || moveDirection.z != 0)
                {
                    if (!RotationEnabled)
                        RotationEnabled = true;
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
    void PlayerRotate()         //rotate player according to mouse input
    {
        if (RotationEnabled && Time.timeScale!=0)
        {
            PlayerRotation += Input.GetAxis("Mouse X") * CameraSensitivity;
            CameraRotation += Input.GetAxis("Mouse Y") * CameraSensitivity;
            CameraRotation = Mathf.Clamp(CameraRotation, -90f, 60f);
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, PlayerRotation, transform.localRotation.eulerAngles.z);
            cameraTransform.localRotation = Quaternion.Euler(-CameraRotation, cameraTransform.localRotation.eulerAngles.y, cameraTransform.localRotation.eulerAngles.z);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);
            Health -= BulletDamage;
            healthBar.SetRemovedSegments(healthBar.RemovedSegments + BulletDamage);
            if(Health<=0)
            {
                Death();
            }
            
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
    void Death()
    {
        MovementEnabled = false;
        RotationEnabled = false;
        for(int i=0;i<UnlockedWeapons.Length;i++)
        {
            UnlockedWeapons[i] = false;
        }
        NextWeapon = null;
        CurrentWeapon.HideWeapon();
        StartCoroutine(FallingDown(5));
       
    }
    IEnumerator FallingDown(float Duration)         //creates player death effect
    {
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        float t = 0;
        while(t<Duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), t/Duration);
            t += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  //restarts the level
    }
}
