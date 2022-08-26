using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator CurrentAnimator;
    private void Start()
    {
        CurrentAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if(Player.playerInstance.Grounded ||  Wallrun.WallRunInstance.WallRunMode)
        { 
            CurrentAnimator.SetFloat("Speed", Player.playerInstance.speed);
        }
        else
        {
            CurrentAnimator.SetFloat("Speed", 0);
        }

        if(Input.GetMouseButton(0))
        {
            CurrentAnimator.SetTrigger("Primary");
        }
        if (Input.GetMouseButton(1))
        {
            CurrentAnimator.SetTrigger("Secondary");
        }
    }
}
