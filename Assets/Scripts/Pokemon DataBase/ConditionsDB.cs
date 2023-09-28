using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB 
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {  
             ConditionID.psn,
             new Condition()
             {
                 Nome = "Envenenado",
                 StartMessage = "foi envenenado",
                 OnAfterTurn = (Pokemon pokemon) =>
                 {
                    pokemon.UpdateHP(pokemon.VidaMax / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} sofreu com envenenamento");
                 }
             }
        },

        {
             ConditionID.brn,
             new Condition()
             {
                 Nome = "Queimando",
                 StartMessage = "esta queimando",
                 OnAfterTurn = (Pokemon pokemon) =>
                 {
                    pokemon.UpdateHP(pokemon.VidaMax / 8); // com 16 q e o padrao ele não funciona 
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} sofreu com queimaduras");
                 }
             }
        },

        {
             ConditionID.par,
             new Condition()
             {
                 Nome = "Paralizado",
                 StartMessage = "esta paralizado",
                 OnBeforeMove = (Pokemon pokemon) =>
                 {
                   if(Random.Range(1, 5) == 1)
                     {
                         pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} está paralizado e não pode se mover");
                         return false;
                     }
                     return true;
                 }
               
             }
        },

        {
             ConditionID.frz,
             new Condition()
             {
                 Nome = "Congelado",
                 StartMessage = "esta congelado",
                 OnBeforeMove = (Pokemon pokemon) =>
                 {
                   if(Random.Range(1, 5) == 1)
                   {
                         pokemon.CureStatus();
                         pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} não esta mais congelado");
                         return true;
                   }
                     return false;
                 }

             }
        },

         {
             ConditionID.slp,
             new Condition()
             {
                 Nome = "Dormindo",
                 StartMessage = "esta dormindo",
                 OnStart = (Pokemon pokemon) =>
                 {
                   // vai dormir por 1-3 turnos
                   pokemon.StatusTime = Random.Range(1,4);
                   Debug.Log($"Vai dormir por {pokemon.StatusTime} turnos");
                 },
                 OnBeforeMove = (Pokemon pokemon) =>
                 {
                    if(pokemon.StatusTime <= 0)
                    {
                     pokemon.CureStatus();
                     pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} acordou!");
                         return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} esta dormindo");
                    return false;
                 }

             }
        },

        {
            ConditionID.confusion,
            new Condition()
            {
                Nome = "Confusão",
                StartMessage = "esta confuso",
                OnStart = (Pokemon pokemon) =>
                {
                    // Confuso por 1-4 turnos
                    pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log($"Esta confuso por {pokemon.VolatileStatusTime} turnos");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} saiu da confusão!");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;

                    // 50% de chance de se mover
                    if(Random.Range(1,3) == 1)
                        return true;

                    // Tomar dano para a confusão
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Nome} esta confuso");
                    pokemon.UpdateHP(pokemon.VidaMax / 8);
                    pokemon.StatusChanges.Enqueue($"Se machucou por confusão");
                    return false;
                }
            }
        }
    };

}

public enum ConditionID
{
    none, // Nenhum 
    psn, // Envenenado
    brn, // Queimando
    slp, // Dormindo
    par, // Paralizado
    frz, // Congelado
    confusion // Confusão
}