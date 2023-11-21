using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PcInfoChanger : MonoBehaviour
{
    public TextMeshProUGUI nome;
    public TextMeshProUGUI lvl; //Lv. level
    public TextMeshProUGUI hp;

    public TextMeshProUGUI[] golpes; //NomeGolpe (Tipo)

    public void UpdateText(Pokemon pkm)
    {
        nome.text = pkm.nome;
        lvl.text = $"Lv. {pkm.level}";
        hp.text = $"{pkm.HP}/{pkm.VidaMax}";

        for (int i = 0; i < pkm.Moves.Count; i++)
        {
            golpes[i].text = $"{i + 1}. {pkm.Moves[i].Base.Nome} ({pkm.Moves[i].Base.Tipo})";
        }
    }

    public void DeleteText()
    {
        nome.text = "";
        lvl.text = "";
        hp.text = $"";

        for (int i = 0; i < golpes.Length; i++)
        {
            golpes[i].text = $"{i+1}. ";
        }
    }
}
