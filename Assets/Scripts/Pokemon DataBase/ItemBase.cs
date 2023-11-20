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
    public TIPO_ITEM tipoItem;
    public STATUS_AUMENTAR statsUP;

    [Header("Valores")]
    public float quantiaStatsUP;
    public float valorCaptura;
    public float preco;

    [Space(10f)]

    [Header("Graficos")]

    public Sprite sprite;
    public GameObject modelo3D;
    public VisualEffectAsset vfx;
    




    public bool Captura(Pokemon enemy)
    {
        var tentativas = 0;
        var tentativas_max = 4;
        var consegui = false;

        //CAPTURA
        var C = ((3 * enemy.VidaMax - 2 * enemy.HP) * (enemy.ChanceCaptura * valorCaptura) * enemy.Status.ChanceCaptura) / 3 * enemy.VidaMax;

        //CONTENCAO
        var F = 1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / C));

        while(tentativas < tentativas_max)
        {
            var _resultado = Random.Range(0, 65526);

            if (_resultado > F)//consegui capturar
            {
                consegui = true;
                break;
            }

            tentativas++;
        }

        //SE O VALOR SORTEADO FOR MAIOR QUE F ENTAO FALSE
        if (!consegui) { return false; } else { return true; }
    }

    public void StatsUp(Pokemon myPokemon)
    {
        switch (statsUP)
        {
            case STATUS_AUMENTAR.LEVEL:
                myPokemon.level += Mathf.RoundToInt(quantiaStatsUP);
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
