using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour
{
    public GameObject parent;
    public Animator anim;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        anim = parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            anim.SetBool("death", true);
            parent.GetComponent<PlayerDetection>().gun.GetComponent<Weapon>().enabled = false;
            parent.GetComponent<CrowdBot>().enabled = false;
            parent.GetComponent<PlayerDetection>().enabled = false;
            parent.GetComponent<Weapon>().enabled = false;
        }

        if (anim.GetBool("death") == true)
        {
            parent.GetComponent<PlayerDetection>().anim.SetBool("run", false);
            parent.GetComponent<PlayerDetection>().anim.SetBool("walk", false);
            parent.GetComponent<PlayerDetection>().anim.SetBool("attack", false);
        }
    }
}
