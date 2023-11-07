using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] public int level;

    public PokemonBase Base {
        get { return _base; }
    }
    public int Level { 
        get { return level;  }
    }

   [SerializeField] public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public Move CurrentMove { get; set; }

    public Sprite sprite;
    public string nome;

    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public Condition Status { get; private set; }

    public int StatusTime { get; set; }

    public Condition VolatileStatus { get; private set; }

    public int VolatileStatusTime { get; set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();

    public float XpAtual { get; set; }
    public float XpGiven { get; set; }

    public bool HpChanged { get; set; }
    public bool XpChanged { get; set; }

    public event System.Action OnStatusChanged;

    public void Init()
    {
        // Geração de Golpes
        Moves = new List<Move>();
        foreach (var move in Base.GolpesParaAprender)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
        CalcularStatus();

        HP = VidaMax;
        XpAtual = 0f;
        XpGiven = 100f;

        ResetStatBoost();

        Status = null;
        VolatileStatus = null;
        sprite = _base.sprite;
        nome = _base.Nome;
    }

    void CalcularStatus()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Ataque, Mathf.FloorToInt((Base.Ataque * Level) / 100f )+ 5);
        Stats.Add(Stat.Defesa, Mathf.FloorToInt((Base.Defesa * Level) / 100f) + 5);
        Stats.Add(Stat.AtaqueEspecial, Mathf.FloorToInt((Base.SpAtaque * Level) / 100f) + 5);
        Stats.Add(Stat.DefesaEspecial, Mathf.FloorToInt((Base.SpDefesa * Level) / 100f) + 5);
        Stats.Add(Stat.Velocidade, Mathf.FloorToInt((Base.Velocidade * Level) / 100f) + 5);

        VidaMax = Mathf.FloorToInt((Base.VidaMax * Level) / 100f ) + 10 + Level;
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
             { Stat.Ataque, 0 },
             { Stat.Defesa, 0 },
             { Stat.AtaqueEspecial, 0 },
             { Stat.DefesaEspecial, 0 },
             { Stat.Velocidade, 0 },
             { Stat.Precisão, 0 },
             { Stat.Evasão, 0 }
        };
    }
    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        // stat boost vai aqui
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }

        return statVal;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

             StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);
            if (boost > 0)
            {
                StatusChanges.Enqueue($"{stat} de {Base.Nome}  aumentou!");
            }
            else
            {
                StatusChanges.Enqueue($"{stat} de {Base.Nome}  caiu!");
            }
            Debug.Log($"{stat} foi boostado para {StatBoosts[stat]}");
        }
    }

    public int Ataque
    {
        get { return GetStat(Stat.Ataque); }
    }
    public int Defesa
    {
        get { return GetStat(Stat.Defesa); }
    }
    public int SpAtaque
    {
        get { return GetStat(Stat.AtaqueEspecial); }
    }
    public int DefesaEspecial
    {
        get { return GetStat(Stat.DefesaEspecial); }
    }
    public int Velocidade
    {
        get { return GetStat(Stat.Velocidade); }
    }
    public int VidaMax
    {
        get; private set; 
    }
    public Sprite Sprite
    {
        get { return sprite; }
    }

    public DamageDetails TomarDano(Move move, Pokemon Attacker)
    {
        float critical = 1f;
        if (UnityEngine.Random.value * 100f <= 6.25f) 
            critical = 2f;

        float type = TypeChart.GetEffectiveness(move.Base.Tipo, this.Base.Tipo1) * TypeChart.GetEffectiveness(move.Base.Tipo, this.Base.Tipo2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };

        float Ataque = (move.Base.Categoria == MoveCategory.Special) ? Attacker.SpAtaque : Attacker.Ataque;
        float _Defesa = (move.Base.Categoria == MoveCategory.Special) ? DefesaEspecial : Defesa;

        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * Attacker.Level + 10) / 250f; 
        float d = a * move.Base.Poder * ((float)Ataque / _Defesa) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);

        
        return damageDetails;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, VidaMax);
        HpChanged = true;
    }

    public void SetStatus(ConditionID conditionId)
    {
        if (Status != null) return; 

        Status = ConditionsDB.Conditions[conditionId];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Nome} {Status.StartMessage}");
        OnStatusChanged?.Invoke();
    }

    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public void SetVolatileStatus(ConditionID conditionId)
    {
        if (VolatileStatus != null) return;

        VolatileStatus = ConditionsDB.Conditions[conditionId];
        VolatileStatus?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Nome} {VolatileStatus.StartMessage}");     
    }

    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }

    public Move GetRandomMove()
    {
        var movesWithPP = Moves.Where(x => x.PP > 0).ToList();

        int r = UnityEngine.Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }
    public bool OnBeforeMove()
    {
        bool canPerformMove = true;

        if (Status?.OnBeforeMove != null)
        {
            if (!Status.OnBeforeMove(this))
            {
                canPerformMove = false;
            }
        }

        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
            {
                canPerformMove = false;
            }

        }

        return canPerformMove;
    }
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }
    public void OnBattleOver()
    {
        VolatileStatus = null;
        ResetStatBoost();
        
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}