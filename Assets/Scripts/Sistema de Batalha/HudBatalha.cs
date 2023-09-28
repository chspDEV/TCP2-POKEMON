using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudBatalha : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextoNome;
    [SerializeField] TextMeshProUGUI TextoLevel;
    [SerializeField] TextMeshProUGUI TextoStatus;
    [SerializeField] BarraVida BarraHp;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    [SerializeField] Color slpColor;

    Pokemon _pokemon;

    Dictionary<ConditionID, Color> statusColors;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        Debug.Log("SetData chamado");
        TextoNome.text = pokemon.Base.Nome;
        TextoLevel.text = "Lvl " + pokemon.Level;
        BarraHp.SetHP((float) pokemon.HP / pokemon.VidaMax);

        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColor },
            {ConditionID.brn, brnColor },
            {ConditionID.slp, slpColor },
            {ConditionID.par, parColor },
            {ConditionID.frz, frzColor },
        };
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }

    void SetStatusText()
    {
        if (_pokemon.Status == null)
        {
            TextoStatus.text = "";
        }
        else
        {
            TextoStatus.text = _pokemon.Status.Id.ToString().ToUpper();
            TextoStatus.color = statusColors[_pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if (_pokemon.HpChanged)
        {
            yield return BarraHp.SetHPsmooth((float)_pokemon.HP / _pokemon.VidaMax);
            _pokemon.HpChanged = false;
        }
       
    }


}
