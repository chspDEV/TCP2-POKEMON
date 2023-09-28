using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] GameObject barraVida;

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

}
