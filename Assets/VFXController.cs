using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffectAsset visualEffectAsset;
    private VisualEffect visualEffect;

    // Start is called before the first frame update
    void Start()
    {
        visualEffect.visualEffectAsset = visualEffectAsset;

        visualEffect.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
