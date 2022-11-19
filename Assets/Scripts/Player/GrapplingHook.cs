using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] LayerMask GrappleLayer;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] float MaxDistance;
    [SerializeField] Transform player;
    [SerializeField] Transform GrappleTip;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float jumpForce;

    LineRenderer lineR;
    Vector3 GrapplePoint;
    SpringJoint joint;
    bool GrappleMode;

    RaycastHit hit;
    void Start()
    {
        lineR = GetComponent<LineRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (GrappleMode)
        {
            if (Input.GetKey(KeyCode.Space))            //Makes the player jump while in grapple mode resulting in exiting grapple mode
            {
                playerRigidbody.AddForce(player.transform.up * jumpForce, ForceMode.VelocityChange);
                EndGrapple();
            }
        }
    }
    private void LateUpdate()
    {
        if (GrappleMode)
        {
            DrawRope();
        }
        
            
    }
    public void StartGrapple()  //raycasts from the front of grappling gun and then adds a spring joint on player with respect to grappled object
    {
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,out hit,MaxDistance,GrappleLayer))
        {
            if (joint != null)
                Destroy(joint);
            GrapplePoint = hit.point;
            joint=player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;

            float DistanceFromPoint = Vector3.Distance(GrapplePoint, GrappleTip.position);

            joint.maxDistance = 5;
            joint.minDistance = 2;

            joint.spring = 200f;
            joint.damper = 7f;
            joint.massScale = 1f;
            lineR.positionCount = 2;
            GrappleMode = true;
            Player.playerInstance.GrappleMode = true;

        }
    }

    void DrawRope()         //sets the position of the line renderer to the grapple point and grapple gun
    {
        lineR.SetPosition(0, GrapplePoint);
        lineR.SetPosition(1, GrappleTip.transform.position);
    }
    void EndGrapple()       //exits grapple mode
    {
        lineR.positionCount = 0;
        GrappleMode = false;
        Player.playerInstance.GrappleMode = false;
        Destroy(joint);
        if (joint != null)
            joint.spring = 0;
    }
    private void OnDisable()    //when grappling gun is holstered grapple mode ends
    {
        EndGrapple();
    }
}
