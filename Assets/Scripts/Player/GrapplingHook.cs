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
            if (Input.GetKey(KeyCode.Space))
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
    public void StartGrapple()
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

            joint.maxDistance = 5;//DistanceFromPoint * 0.8f;
            joint.minDistance = 2;//DistanceFromPoint * 0.25f;

            joint.spring = 200f;
            joint.damper = 7f;
            joint.massScale = 1f;
            lineR.positionCount = 2;
            GrappleMode = true;
            Player.playerInstance.GrappleMode = true;

        }
    }

    void DrawRope()
    {
        lineR.SetPosition(0, GrapplePoint);
        lineR.SetPosition(1, GrappleTip.transform.position);
    }
    void EndGrapple()
    {
        lineR.positionCount = 0;
        GrappleMode = false;
        Player.playerInstance.GrappleMode = false;
        Destroy(joint);
        if (joint != null)
            joint.spring = 0;
    }
    private void OnDisable()
    {
        EndGrapple();
    }
}
