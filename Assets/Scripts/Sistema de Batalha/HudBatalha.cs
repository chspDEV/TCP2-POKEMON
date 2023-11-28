using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HudBatalha : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextoNome;
    [SerializeField] TextMeshProUGUI TextoVida;
    [SerializeField] TextMeshProUGUI TextoLevel;
    [SerializeField] TextMeshProUGUI TextoStatus;
    [SerializeField] BarraVida BarraHp;
    public BarraXP BarraXp;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    [SerializeField] Color slpColor;

    [SerializeField] Pokemon _pokemon;
    BarraVida _barraVida;
    [SerializeField] GameObject VidaPlayer;

    Dictionary<ConditionID, Color> statusColors;

    public void Update()
    {
        if (_pokemon.HP >= _pokemon.VidaMax / 2)
        {
            MudaCorVerde();
        }
    }

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        Debug.Log("SetData chamado");
        TextoNome.text = pokemon.Base.Nome;
        TextoLevel.text = "Lvl " + pokemon.Level;
        BarraHp.SetHP((float) pokemon.HP / pokemon.VidaMax);


        BarraXp.SetXP((float)pokemon.XpAtual / Mathf.Pow(pokemon.level, 3));

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

    public void SetarVida(Pokemon pkm)
    {
        TextoVida.SetText(pkm.HP + "/" + pkm.VidaMax);
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

    public void MudaCorAmarelo()
    {
        var _Image = VidaPlayer.GetComponent<UnityEngine.UI.Image>();
        _Image.color = Color.yellow;
    }

    public void MudaCorVermelho()
    {
        var _Image = VidaPlayer.GetComponent<UnityEngine.UI.Image>();
        _Image.color = Color.red;
    }

    public void MudaCorVerde()
    {
        var _Image = VidaPlayer.GetComponent<UnityEngine.UI.Image>();
        _Image.color = Color.green;
    }

    public IEnumerator UpdateHP()
    {
        if (_pokemon.HpChanged)
        {
            yield return BarraHp.SetHPsmooth((float)_pokemon.HP / _pokemon.VidaMax);
            _pokemon.HpChanged = false;

            if (TextoVida != null)
            {
                SetarVida(_pokemon);
            }
        }

        if (_pokemon.HP <= _pokemon.VidaMax / 4)
        {
            MudaCorVermelho();
            //BarraHp.MudaCorVermelho();
        }
        else if (_pokemon.HP >= _pokemon.VidaMax / 4 && _pokemon.HP <= _pokemon.VidaMax / 2)
        {
            MudaCorAmarelo();
            //BarraHP.MudaCorAmarelo();
        }
        else if (_pokemon.HP >= _pokemon.VidaMax / 2)
        {
            MudaCorVerde();
        }
    }

}
