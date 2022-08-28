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
    private void Start()
    {
        ReticleSprite = GetComponent<Image>();
        ReticleReference = this;
    }
}
