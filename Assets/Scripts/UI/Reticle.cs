using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public static Reticle ReticleReference
    {
        get;
        set;
    }
    public Image ReticleSprite;
    private void Awake()
    {
        ReticleSprite = GetComponent<Image>();  
        ReticleReference = this;                //gets the reference of reticle image which can be accessed from other scripts using singleton
    }
}
