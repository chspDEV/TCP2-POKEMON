using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Golpe", menuName = "Pokemon/Criar um novo Golpe")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string nome;

    [TextArea]
    [SerializeField] string descricao;

    [SerializeField] TipoDePokemon tipo;
    [SerializeField] int poder;
    [SerializeField] int precisao;
    [SerializeField] bool sempreAcerta;
    [SerializeField] int pp;
    [SerializeField] int prioridade;
    [SerializeField] MoveCategory categoria;
    [SerializeField] MoveEffects efeitos;
    [SerializeField] List<SecondaryEffects> efeitoSecundario;
    [SerializeField] MoveTarget alvo;

    public string Nome
    {
        get { return nome; }
    }
    public string Descricao
    {
        get { return descricao; }
    }

    public TipoDePokemon Tipo
    {
        get { return tipo; }
    }

    public int Poder
    {
        get { return poder; }
    }

    public int Precisao
    {
        get { return precisao; }
    }

    public bool SempreAcerta
    {
        get { return sempreAcerta; }
    }
    public int PP
    {
        get { return pp; }
    }

    public int Prioridade
    {
        get { return prioridade; }
    }

    public MoveCategory Categoria
    {
        get { return categoria; }
    }

    public MoveEffects Efeitos
    {
        get { return efeitos; }
    }

    public List<SecondaryEffects> EfeitoSecundario
    {
        get { return efeitoSecundario; }
    }

    public MoveTarget Alvo
    {
        get { return alvo; }
    }
}


[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;

    public List<StatBoost> Boosts { get { return boosts; } }

    public ConditionID Status { get { return status; } }

    public ConditionID VolatileStatus { get { return volatileStatus; } }
}


[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget alvo;

    public int Chance { get { return chance; } }
    public MoveTarget Alvo { get { return alvo; } }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum MoveCategory
{
    Physical, Special, Status
}

public enum MoveTarget
{
    Inimigo, Proprio
}