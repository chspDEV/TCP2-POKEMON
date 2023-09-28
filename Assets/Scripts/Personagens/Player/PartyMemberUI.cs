using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextoNome;
    [SerializeField] TextMeshProUGUI TextoLevel;
    [SerializeField] BarraVida BarraHp;
    [SerializeField] Color corzinha;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        Debug.Log("SetData chamado");
        TextoNome.text = pokemon.Base.Nome;
        TextoLevel.text = "Lvl " + pokemon.Level;
        BarraHp.SetHP((float)pokemon.HP / pokemon.VidaMax);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            //TextoNome.color = corzinha;
        }
        else
        {
            //TextoNome.color = Color.black;
        }
    }
}
