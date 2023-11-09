using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogoDeBatalha : MonoBehaviour
{
   [SerializeField] int letrasPorSegundo;
   [SerializeField] Color CorBrilhante;
   [SerializeField] TextMeshProUGUI dialogText;

   [SerializeField] GameObject SelecaoDeAcoes;
   [SerializeField] GameObject SelecaoDeGolpe;
   [SerializeField] GameObject DetalheDeGolpes;

   [SerializeField] List<TextMeshProUGUI> textosDeAcao;
   [SerializeField] List<TextMeshProUGUI> textosDeGolpes;

   [SerializeField] TextMeshProUGUI ppText;     // Texto do PowerPoints de cada golpe
   [SerializeField] TextMeshProUGUI tipoText;  // Tipagem do golpe


   public void SetDialog(string dialogo)
   {
     dialogText.text = dialogo;
   }

   public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letra in dialog.ToCharArray())
        {
            dialogText.text += letra;
            yield return new WaitForSeconds(1f / letrasPorSegundo);
        }

        yield return new WaitForSeconds(1f);
    }

    public void AtivarTextoDialogo(bool ativo)
    {
        dialogText.enabled = ativo;
    }
    public void AtivarSelecaoAcao (bool ativo)
    {
        SelecaoDeAcoes.SetActive(ativo);
    }
    public void AtivarSelecaoGolpe (bool ativo)
    {
        SelecaoDeGolpe.SetActive(ativo);
        DetalheDeGolpes.SetActive(ativo);
    }

    public void UpdateActionSelection(int selectedAction)
    {
      /*  for (int i = 0; i < textosDeAcao.Count; ++i)
        {
            if (i == selectedAction)
            {
                textosDeAcao[i].color = CorBrilhante;
            }              
            else
            {
                textosDeAcao[i].color = Color.white;
            }             
        }
      */
    }

    
    public void UpdateMoveSelection(Move move)
    {
        for (int i = 0; i < textosDeGolpes.Count; ++i)
        {
            ppText.text = $"PP {move.PP}/{move.Base.PP}";
            tipoText.text = move.Base.Tipo.ToString();
            //  Debug.Log(ppText);
            //   Debug.Log(tipoText);
            if (move.PP == 0)
                ppText.color = Color.red;
            else
                ppText.color = Color.white;
        }
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < textosDeGolpes.Count; ++i)
        {
            if (i < moves.Count)
            {
                textosDeGolpes[i].text = moves[i].Base.Nome;
            }
            else
            {
                textosDeGolpes[i].text = "-";
            }
        }
    }
}
