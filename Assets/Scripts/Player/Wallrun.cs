using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Wallrun : MonoBehaviour
{
    [SerializeField] LayerMask WallRunLayer;
    [SerializeField] float WallDistance;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    Rigidbody playerRigidbody;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool leftWall;
    bool rightWall;
    Vector3 wallRunJumpDirection;

    public bool WallRunMode;

    public static Wallrun WallRunInstance
    {
        get;
        set;
    }
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        WallRunInstance = this;
    }
    void Update()
    {

        if(!Player.playerInstance.Grounded && Input.GetAxis("Vertical")>0)
        {
            CheckWallRun();
            if(leftWall || rightWall)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }
    private void FixedUpdate()
    {
        if(WallRunMode)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (leftWall)
                    wallRunJumpDirection = transform.up + leftWallHit.normal;
                else if (rightWall)
                    wallRunJumpDirection = transform.up + rightWallHit.normal;
                playerRigidbody.AddForce(wallRunJumpDirection * wallRunJumpForce, ForceMode.VelocityChange);
            }
        }
    }
    void CheckWallRun()
    {
        leftWall = Physics.Raycast(transform.position, transform.right, out leftWallHit, WallDistance, WallRunLayer);
        rightWall = Physics.Raycast(transform.position,-transform.right, out rightWallHit, WallDistance, WallRunLayer);
    }
    void StartWallRun()
    {
        playerRigidbody.useGravity = false;
        if (!WallRunMode)
        {
            StopAllCoroutines();
            if (leftWall)
            {
                //playerCamera.transform.localRotation=Quaternion.RotateTowards(playerCamera.transform.localRotation,Quaternion.Euler())
                StartCoroutine(TiltCamera(25, 1.5f));

            }
            if (rightWall)
            {
                StartCoroutine(TiltCamera(-25, 1.5f));
            }
            StartCoroutine(ChangeCamFOV(70, 2f));
        }
        WallRunMode = true;
       
    }
    void StopWallRun()
    {
        playerRigidbody.useGravity = true;
        if(WallRunMode)
        {
            StopAllCoroutines();
            StartCoroutine(TiltCamera(0, 1.5f));
            StartCoroutine(ChangeCamFOV(60, 2f));
        }
        WallRunMode = false;
    }
    IEnumerator TiltCamera(float Tilt,float Duration)
    {
        float t = 0;
        while(t<Duration)
        {
            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, Quaternion.Euler(playerCamera.transform.localRotation.eulerAngles.x, playerCamera.transform.localRotation.eulerAngles.y, Tilt), t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
    IEnumerator ChangeCamFOV(float newFOV, float Duration)
    {
        float t = 0;
        while (t < Duration)
        {
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, newFOV, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
}
