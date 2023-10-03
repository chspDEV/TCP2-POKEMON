using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BarraVida : MonoBehaviour
{
    [SerializeField] GameObject barraVida;
    public GameObject VidaPlayer;


    public void Awake()
    {
       VidaPlayer = GameObject.FindGameObjectWithTag("VidaPlayerBatalha");
    }
    public void SetHP(float hpNormalized)
    {
        barraVida.transform.localScale = new Vector3(hpNormalized, 1f);
        //  barraVida.fillAmount = hpNormalized;
    }
    public IEnumerator SetHPsmooth(float newHP)
    {
        float curHP = barraVida.transform.localScale.x;
        float changeAmt = curHP - newHP;

        while (curHP - newHP > Mathf.Epsilon)
        {
            curHP -= changeAmt * Time.deltaTime;
            barraVida.transform.localScale = new Vector3(curHP, 1f);
            yield return null;
        }
        barraVida.transform.localScale = new Vector3(newHP, 1f);
        
      
    }

    public void MudaCorAmarelo()
    {
        var _Image = VidaPlayer.GetComponent<UnityEngine.UI.Image>();
        _Image.color = new Color32(255, 255, 255, 100);
    }

    public void MudaCorVermelho()
    {
        var _Image = VidaPlayer.GetComponent<UnityEngine.UI.Image>();
        _Image.color = new Color(1, 0, 0, 1);
    }

}
