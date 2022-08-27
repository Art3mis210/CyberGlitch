using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum WeaponType
    { 
        Melee,SingleShotGun,AutomaticGun
    }
    [SerializeField] WeaponType CurrentWeaponType;
    [SerializeField] float Ammo;
    [SerializeField] float MagSize;
    [SerializeField] float CurrentMag;
    Animator CurrentAnimator;
    private void Start()
    {
        CurrentAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        CurrentAnimator.SetFloat("Speed", Player.playerInstance.speed);
       /* if (Player.playerInstance.Grounded ||  Wallrun.WallRunInstance.WallRunMode)
        { 
           
        }
        else
        {
            CurrentAnimator.SetFloat("Speed", 0);
        }*/


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
        else
        {
            if (CurrentWeaponType == WeaponType.SingleShotGun)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CurrentAnimator.SetTrigger("Primary");
                }
            }
            else
            {
                if(Input.GetMouseButton(0))
                {
                    CurrentAnimator.SetBool("Primary", true);
                }
                else
                {
                    CurrentAnimator.SetBool("Primary", false);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                CurrentAnimator.SetBool("Aim", !CurrentAnimator.GetBool("Aim"));
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentAnimator.SetTrigger("Reload");
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            CurrentAnimator.SetTrigger("Hide");
        }

    }
}
