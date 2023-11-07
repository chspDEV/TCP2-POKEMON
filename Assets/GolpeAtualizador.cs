using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeAtualizador : MonoBehaviour
{
    [SerializeField] SistemaDeBatalha bt;
    [SerializeField] DialogoDeBatalha dbt;
    [SerializeField] int index;


    private void Update()
    {
      
    }

    // Start is called before the first frame update
    public void OnMouseEnter()
    {
        Debug.Log("Entrada do mouse");
        dbt.UpdateMoveSelection(index, bt.playerUnit.Pokemon.Moves[index]);
    }

    public void OnMouseOver()
    {
        Debug.Log("MOUSE OVERRR");
    }

    void OnMouseExit()
    {
        Debug.Log("MOUSE sai");
    }


}
