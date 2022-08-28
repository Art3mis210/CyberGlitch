using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnlock : MonoBehaviour
{
    [SerializeField] int WeaponID;
    bool WeaponUnlocked;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            if(!WeaponUnlocked)
            {
                WeaponUnlocked = true;
                Player.playerInstance.UnlockedWeapons[WeaponID] = true;
            }
        }
    }
}
