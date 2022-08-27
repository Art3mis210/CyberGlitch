using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdBot : MonoBehaviour
{
    public Animator anim;
    NavMeshAgent agent;
    public GameObject player;
    public RaycastHit hitinfo;
    public GameObject[] goalLocations;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        GetComponent<Animator>().SetFloat("Offset", Random.Range(0.0f, 1.0f));
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);

    }

    public void ResetAgent()
    {
        //anim.SetFloat("wOffset", Random.Range(0,1));
        //anim.SetTrigger("isWalking");
        float speedMult = Random.Range(0.35f, 1.5f);
        //anim.SetFloat("speedMult", sppedMult);
        agent.speed *= speedMult;
        agent.angularSpeed = 120;
        agent.ResetPath();
        if (goalLocations.Length > 1)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);

        }
        else
        {
            anim.SetBool("walk", true);
        }
        agent.speed = 1f;

        if (Physics.Raycast(transform.position + transform.up, player.transform.position - transform.position, out hitinfo, 20))
        {
            Debug.Log(hitinfo.transform.gameObject.name);
            if (hitinfo.transform.gameObject == player)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 20)
                {
                    anim.SetBool("walk", false);
                    transform.LookAt(player.transform);
                    gameObject.GetComponent<PlayerDetection>().enabled = true;
                }

            }
            else
            {
                gameObject.GetComponent<PlayerDetection>().enabled = false;
            }
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetBool("walk", true);
            anim.SetBool("attack", false);
            gameObject.GetComponent<PlayerDetection>().gun.GetComponent<Weapon>().enabled = false;
        }
    }
}
