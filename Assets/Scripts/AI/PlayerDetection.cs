using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDetection : MonoBehaviour
{
    public Transform Player;
    public NavMeshAgent agent;
    public float visDistance;
    public float visAngle;
    public float rotationSpeed;
    public float shootDist;
    public string State = "Idle";
    public float Speed;
    public Animator anim;
    public Vector3 Offset;
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("Offset", Random.Range(0.0f, 1.0f));
        Player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        
        agent.SetDestination(Player.position - gameObject.transform.forward);

        Vector3 direction = Player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        if (direction.magnitude < visDistance && angle < visAngle)
        {
            
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

            if (direction.magnitude > shootDist)
            {
                
                State = "Running";
                agent.speed = 3;
                Speed = 1;
                anim.SetBool("run", true);
                anim.SetBool("attack", false);
                gun.GetComponent<Weapon>().S = false;
                gun.GetComponent<Weapon>().enabled = false;
            }
            else
            {
                State = "Shooting";
                agent.speed = 0;
                Speed = 0;
                anim.SetBool("attack", true);
                anim.SetBool("run", false);
                gun.GetComponent<Weapon>().enabled = true;
                gun.GetComponent<Weapon>().S = true;
                gun.transform.localRotation = Quaternion.Euler(-217.87f, -103.19f, 70.95f);
                gameObject.GetComponent<CrowdBot>().anim.SetBool("walk", false);
                gameObject.GetComponent<CrowdBot>().enabled = false;
            }
        }
        else
        {
            State = "Idle";
            agent.speed = 0;
            Speed = 0;
            anim.SetBool("attack", false);
            anim.SetBool("run", false);
            agent.SetDestination(transform.position);
            anim.SetBool("walk", true);
            gun.transform.localPosition = new Vector3(1.78f, 1.54f, -0.46f);
            gun.transform.localRotation = Quaternion.Euler(-191.29f, -90.866f, 98.523f);
            gun.GetComponent<Weapon>().S = false;
            gun.GetComponent<Weapon>().enabled = false;
            gameObject.GetComponent<CrowdBot>().enabled = true;

        }

        if (State == "Running")
        {
            this.transform.Translate(0, 0, Time.deltaTime * Speed);

        }

    }

    private void LateUpdate()
    {
        if (gameObject.GetComponent<Weapon>().S == true)
        {
            Transform Chest = anim.GetBoneTransform(HumanBodyBones.Spine);
            //Debug.Log(Chest);
            Chest.LookAt(Player.transform.position);
            Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
        }
    }
}
