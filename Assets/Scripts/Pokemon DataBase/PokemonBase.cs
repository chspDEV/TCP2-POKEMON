using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Criar um Pokemon")]
public class PokemonBase : ScriptableObject
{

    #region Base
    [Space(10)]
    [Header("Informações do Pokémon")]
    [Space(10)]
    [SerializeField] string nome;

    [TextArea]
    [SerializeField] string descricao;

    public Sprite sprite;  // obs: colocar o modelo 3D quando ele estiver pronto

    [SerializeField] GameObject modelo3D;

    [SerializeField] TipoDePokemon tipo1;
    [SerializeField] TipoDePokemon tipo2;

    // Status Base 
    [Space(10)]
    [Header("Status Base")]
    [Space(10)]
    [SerializeField] int vidaMax; // Vida maxima 
    [SerializeField] int ataque; // Ataque
    [SerializeField] int xpAtual; // XP ATUAL
    [SerializeField] int xpGiven; // XP Dado

    [SerializeField] int defesa; // Defesa
    [SerializeField] int spAtaque; // Special Ataque
    [SerializeField] int spDefesa; // Special Defesa
    [SerializeField] int velocidade; // Velocidade

    [SerializeField] List<GolpesParaAprender> golpesParaAprender;

    #endregion

    #region Get Status

    public string Nome
    {
        get { return nome; }
    }

    public string Descricao
    {
        get { return descricao; }
    }

    public Sprite Sprite
    {
        get { return sprite; }
    }
   

    public GameObject Modelo3D
    {
        get { return modelo3D; }
    }

    public TipoDePokemon Tipo1
    {
        get { return tipo1; }
    }

    public TipoDePokemon Tipo2
    {
        get { return tipo2; }
    }

    public int VidaMax
    {
        get { return vidaMax; }
    }

    public int Ataque
    {
        get { return ataque; }
    }

    //XP--------
    public int XpAtual
    {
        get { return xpAtual; }
    }

    public int XpGiven
    {
        get { return xpGiven; }
    }
    //---------

    public int Defesa
    {
        get { return defesa; }
    }

    public int SpAtaque
    {
        get { return spAtaque; }
    }
    public int SpDefesa
    {
        get { return spDefesa; }
    }
    public int Velocidade
    {
        get { return velocidade; }
    }
    public List<GolpesParaAprender> GolpesParaAprender
    {
        get { return golpesParaAprender; }
    }
    #endregion
}

[System.Serializable]
public class GolpesParaAprender
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }

}

public enum TipoDePokemon
{
    Nenhum,
    Normal,
    Fogo,
    Agua,
    Eletrico,
    Grama,
    Gelo,
    Lutador,
    Veneno,
    Terra,
    Voador,
    Psiquico,
    Inseto,
    Pedra,
    Fantasma,
    Dragao,
    Sombrio,
    Metal,
    Fada


}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // Esses status não são para buffar ou debuffar, são para o moveAccuracy (precisão do golpe)
    Accuracy,
    Evasion
}

public class TypeChart
{
    static float[][] chart =
    {
       //                       Nor   Fir   Wat   Ele   Gra   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Dra   Dar  Ste    Fai
        /*Normal*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0,    1f,   1f,   0.5f, 1f},
        /*Fire*/    new float[] {1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f},
        /*Water*/   new float[] {1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f},
        /*Electric*/new float[] {1f,   1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f},
        /*Grass*/   new float[] {1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f},
        /*Ice*/     new float[] {1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f},
        /*Fighting*/new float[] {2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f},
        /*Poison*/  new float[] {1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f},
        /*Ground*/  new float[] {1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   1f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   2f,   1f},
        /*Flying*/  new float[] {1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f},
        /*Psychic*/ new float[] {1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f},
        /*Bug*/     new float[] {1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f},
        /*Rock*/    new float[] {1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f},
        /*Ghost*/   new float[] {0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 1f,   1f},
        /*Dragon*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0f},
        /*Dark*/    new float[] {1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f},
        /*Steel*/   new float[] {1f,   0.5f, 0.5f, 0.5f, 1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 2f},
        /*Fairy*/   new float[] {1f,   0.5f, 1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f},
    };

    public static float GetEffectiveness(TipoDePokemon attackType, TipoDePokemon defenseType)
    {
        if (attackType == TipoDePokemon.Nenhum || defenseType == TipoDePokemon.Nenhum)
        {
            return 1;
        }
        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}