using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BarraXP: MonoBehaviour
{
    [SerializeField] GameObject barraXP;
    public GameObject xpPlayer;


    public void Awake()
    {
       //VidaPlayer = GameObject.FindGameObjectWithTag("VidaPlayerBatalha");
    }

    public void SetXP(float xpNormalized)
    {
        barraXP.transform.localScale = new Vector3(xpNormalized, 1f);
    }
    public IEnumerator SetXPsmooth(float newXP)
    {
        float curXP = barraXP.transform.localScale.x;
        float changeAmt = curXP + newXP;

        while (curXP + newXP < Mathf.Epsilon)
        {
            curXP += changeAmt * Time.deltaTime;
            barraXP.transform.localScale = new Vector3(curXP, 1f);
            yield return null;
        }
        barraXP.transform.localScale = new Vector3(newXP, 1f);
       
    }

}
