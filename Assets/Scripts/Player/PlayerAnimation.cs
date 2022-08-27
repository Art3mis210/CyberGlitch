using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum WeaponType
    { 
        Melee,Shoot
    }
    [SerializeField] WeaponType CurrentWeaponType;
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


        if(CurrentWeaponType==WeaponType.Melee)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CurrentAnimator.SetTrigger("Primary");
            }
            if (Input.GetMouseButtonDown(1))
            {
                CurrentAnimator.SetTrigger("Secondary");
            }
        }
       
    }
}
