using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeAtualizador : MonoBehaviour
{
    [SerializeField] SistemaDeBatalha bt;
    [SerializeField] DialogoDeBatalha dbt;
    public List<int> index;


    public void DizerGolpe1()
    {
        bt.currentMove = index[0];
        dbt.UpdateMoveSelection(bt.playerUnit.Pokemon.Moves[index[0]]);
    }

    public void DizerGolpe2()
    {
        if (1 < bt.playerUnit.Pokemon.Moves.Count)
        {
            bt.currentMove = index[1];

            dbt.UpdateMoveSelection(bt.playerUnit.Pokemon.Moves[index[1]]);
        }

    }

    public void DizerGolpe3()
    {
        if (2 < bt.playerUnit.Pokemon.Moves.Count)
        {
            bt.currentMove = index[2];

            dbt.UpdateMoveSelection(bt.playerUnit.Pokemon.Moves[index[2]]);
        }
    }

    public void DizerGolpe4()
    {
        if (3 < bt.playerUnit.Pokemon.Moves.Count)
        {
            bt.currentMove = index[3];

            dbt.UpdateMoveSelection(bt.playerUnit.Pokemon.Moves[index[3]]);
        }
    }
}
