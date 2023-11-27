using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum TIPO_ITEM
{ 
    CONSUMIVEL, POKEBOLA
}

public enum STATUS_AUMENTAR
{
    LEVEL, VIDA, NENHUM
}


[CreateAssetMenu(fileName = "ItemNovo", menuName = "Pokemon/Criar um Item")]
public class ItemBase : ScriptableObject
{
    public string nome;
    public TIPO_ITEM tipoItem;
    public STATUS_AUMENTAR statsUP;
    
    [Header("Valores")]
    public float quantiaStatsUP;
    public float valorCaptura;
    public float preco;

    [Space(10f)]

    [Header("Graficos")]

    [TextArea]
    public string descricao;

    public Sprite sprite;
    public GameObject modelo3D;
    public VisualEffectAsset vfx;
    public float C, F;
    
    public bool Captura(Pokemon enemy)
    {
        var tentativas = 0;
        var tentativas_max = 4;
        var consegui = false;
        
       

        //CAPTURA
        if (enemy.Status != null)
        {
            C = (3 * enemy.VidaMax - 2 * enemy.HP) * (enemy.ChanceCaptura * valorCaptura) * enemy.Status.ChanceCaptura / 3 * enemy.VidaMax;
        }
        else 
        {
            C = (3 * enemy.VidaMax - 2 * enemy.HP) * (enemy.ChanceCaptura * valorCaptura) / 3 * enemy.VidaMax; 
        }

        //CONTENCAO
        F = 1048560 / Mathf.Sqrt( Mathf.Sqrt(16711680 / C) );

        Debug.Log(F);

        while (tentativas < tentativas_max)
        {
            var _resultado = Random.Range(0, 65526);
            Debug.Log(_resultado);

            if (_resultado < F)//consegui capturar
            {
                consegui = true;
                break;
            }

            tentativas++;
        }

        Debug.Log(consegui);

        //SE O VALOR SORTEADO FOR MAIOR QUE F ENTAO FALSE
        C = 0f;
        F = 0f;

        return consegui;
    }

    public void StatsUp(Pokemon myPokemon)
    {

        Debug.Log("Rodei statusUp");

        switch (statsUP)
        {
            case STATUS_AUMENTAR.LEVEL:
                myPokemon.level += Mathf.RoundToInt(quantiaStatsUP);
                myPokemon.AttGolpes();
                break;

            case STATUS_AUMENTAR.VIDA:
                //usando a funcao de dano pra curar :P
                myPokemon.UpdateHP(Mathf.RoundToInt(quantiaStatsUP) * -1);
                break;

            default:
                break;
        }

    }

}
