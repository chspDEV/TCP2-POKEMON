using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base {
        get { return _base; }
    }
    public int Level { 
        get { return level;  }
    }

   [SerializeField] public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public Move CurrentMove { get; set; }

    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public Condition Status { get; private set; }

    public int StatusTime { get; set; }

    public Condition VolatileStatus { get; private set; }

    public int VolatileStatusTime { get; set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();

    public bool HpChanged { get; set; }

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

        ResetStatBoost();

        Status = null;
        VolatileStatus = null;
    }

    void CalcularStatus()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Ataque * Level) / 100f )+ 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defesa * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAtaque * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefesa * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Velocidade * Level) / 100f) + 5);

        VidaMax = Mathf.FloorToInt((Base.VidaMax * Level) / 100f ) + 10 + Level;
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
             { Stat.Attack, 0 },
             { Stat.Defense, 0 },
             { Stat.SpAttack, 0 },
             { Stat.SpDefense, 0 },
             { Stat.Speed, 0 },
             { Stat.Accuracy, 0 },
             { Stat.Evasion, 0 }
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
                StatusChanges.Enqueue($"{Base.Nome} {stat} aumentou!");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Nome} {stat} caiu!");
            }
            Debug.Log($"{stat} foi boostado para {StatBoosts[stat]}");
        }
    }

    public int Ataque
    {
        get { return GetStat(Stat.Attack); }
    }
    public int Defesa
    {
        get { return GetStat(Stat.Defense); }
    }
    public int SpAtaque
    {
        get { return GetStat(Stat.SpAttack); }
    }
    public int SpDefesa
    {
        get { return GetStat(Stat.SpDefense); }
    }
    public int Velocidade
    {
        get { return GetStat(Stat.Speed); }
    }
    public int VidaMax
    {
        get; private set; 
    }
    
    public DamageDetails TomarDano(Move move, Pokemon attacker)
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

        float attack = (move.Base.Categoria == MoveCategory.Special) ? attacker.SpAtaque : attacker.Ataque;
        float defense = (move.Base.Categoria == MoveCategory.Special) ? SpDefesa : Defesa;

        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Poder * ((float)attack / defense) + 2;
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