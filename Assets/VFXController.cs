using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffectAsset visualEffectAsset;
    private VisualEffect visualEffect;

    public void RodarVFX(VisualEffectAsset vfxAsset)
    {
        visualEffect.visualEffectAsset = vfxAsset;

        visualEffect.Play();
    }
}
