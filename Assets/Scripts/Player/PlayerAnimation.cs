using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] float WeaponRange;
    [SerializeField] int WeaponDamage;
    [SerializeField] Transform WeaponParticleSystem;
    [SerializeField] Text CurrentMagText;
    [SerializeField] Text AmmoText;
    [SerializeField] LayerMask BulletDamageLayer;
    AudioSource weaponSound;

    Animator CurrentAnimator;
    RaycastHit hit;
    private void Start()
    {
        CurrentAnimator = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
        if(AmmoText != null)
            AmmoText.text = Ammo.ToString();
        if(CurrentMagText != null)
            CurrentMagText.text = CurrentMag.ToString();
    }
    private void OnEnable()
    {
        if(CurrentAnimator!=null)
        {
            CurrentAnimator.Rebind();
        }
        if(Reticle.ReticleReference!=null)
            Reticle.ReticleReference.ReticleSprite.enabled = true;


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
                if (Input.GetMouseButtonDown(0) && CurrentMag>0)
                {
                    CurrentAnimator.SetTrigger("Primary");
                }
            }
            else
            {
                if(Input.GetMouseButton(0) && CurrentMag >0)
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
                Reticle.ReticleReference.ReticleSprite.enabled = !CurrentAnimator.GetBool("Aim");
            }
            if (Input.GetKeyDown(KeyCode.R) && Ammo > 0 && CurrentMag<MagSize)
            {
                CurrentAnimator.SetTrigger("Reload");
                if (Ammo >= MagSize)
                {
                    CurrentMag = MagSize;
                    Ammo -= MagSize;
                }
                else
                    CurrentMag = Ammo;
                AmmoText.text = Ammo.ToString();
                CurrentMagText.text = CurrentMag.ToString();
            }
        }
    }
    public void HideWeapon()
    {
        CurrentAnimator.SetTrigger("Hide");
    }
    public void DisableWeapon()
    {
        if (Player.playerInstance.NextWeapon != null)
        {
            Player.playerInstance.NextWeapon.gameObject.SetActive(true);
            Player.playerInstance.CurrentWeapon = Player.playerInstance.NextWeapon;
        }
        gameObject.SetActive(false);
    }
    public void Shoot()
    {
        if(CurrentWeaponType==WeaponType.Melee)
        {
            if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, WeaponRange, BulletDamageLayer))
            {
                if (hit.transform.gameObject.tag == "enemy")
                {

                    hit.transform.gameObject.GetComponent<AIHealthSystem>().health -= WeaponDamage;
                    Debug.Log(hit.transform.gameObject.GetComponent<AIHealthSystem>().health -= WeaponDamage);
                }
            }
        }
        else if (CurrentMag > 0)
        {
            if (WeaponParticleSystem != null)
            {
                WeaponParticleSystem.gameObject.SetActive(true);
                if (weaponSound != null)
                {
                    weaponSound.Play();
                }
            }
            if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, WeaponRange,BulletDamageLayer))
            {
                if (hit.transform.gameObject.tag == "enemy")
                {
                    
                    hit.transform.gameObject.GetComponent<AIHealthSystem>().health -= WeaponDamage;
                    Debug.Log(hit.transform.gameObject.GetComponent<AIHealthSystem>().health -= WeaponDamage);
                }
            }
            CurrentMag--;
            CurrentMagText.text = CurrentMag.ToString();
        }
    }
    public void TurnParticleEffectOff()
    {
        WeaponParticleSystem.gameObject.SetActive(false);
    }

}
