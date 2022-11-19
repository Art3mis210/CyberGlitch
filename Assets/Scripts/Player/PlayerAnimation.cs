using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerAnimation : MonoBehaviour
{
    public enum WeaponType
    {
        Melee, GrapplingHook, SingleShotGun, AutomaticGun           
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
    [SerializeField] float AimHeight;
    float IdleHeight;
    [SerializeField] float AimDistance;
    float IdleDistance;
    bool Aim;
    [SerializeField] bool Scope;
    [SerializeField] GameObject ScopeComponents;

    AudioSource weaponSound;

    Animator CurrentAnimator;
    RaycastHit hit;
    private void Start()
    {
        CurrentAnimator = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
        if (AmmoText != null)
            AmmoText.text = Ammo.ToString();
        if (CurrentMagText != null)
            CurrentMagText.text = CurrentMag.ToString();
        IdleHeight = transform.localPosition.y;
        IdleDistance= transform.localPosition.z;
    }
    private void OnEnable()
    {
        if (Reticle.ReticleReference != null)
            Reticle.ReticleReference.ReticleSprite.enabled = true;
    }
    void Update()               
    {
        CurrentAnimator.SetFloat("Speed", Player.playerInstance.speed);
        if (CurrentWeaponType == WeaponType.Melee || CurrentWeaponType == WeaponType.GrapplingHook) //plays animation of different weapon according to player input 
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                CurrentAnimator.SetTrigger("Primary");
            }
            if (Input.GetMouseButtonDown(1))
            {
                CurrentAnimator.SetTrigger("Secondary");
                if(CurrentWeaponType == WeaponType.GrapplingHook)
                {
                    Aim = !Aim;
                    if(Aim)
                    {
                        StopAllCoroutines();
                        StartCoroutine(AimPose(true, 2f));
                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(AimPose(false, 2f));
                    }
                }
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
                if (Scope)
                {
                    ScopeComponents.SetActive(CurrentAnimator.GetBool("Aim"));
                }
                if (CurrentAnimator.GetBool("Aim"))
                {
                    StopAllCoroutines();
                    StartCoroutine(AimPose(true, 2f));
                    
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(AimPose(false, 2f));
                }
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
            Player.playerInstance.NextWeapon.GetComponent<Animator>().Rebind();
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
    IEnumerator AimPose(bool Status,float Duration)    //adjusts the position of gun while aiming down sights
    {

        float t = 0;
        while(t<Duration)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, Status ? AimHeight:IdleHeight, Status ? AimDistance : IdleDistance), t/Duration);
            t += Time.deltaTime;
            yield return null;
        }
        
    }
   

}
