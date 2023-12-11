using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pokemart : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Comprar;
    public GameController gm;
    public ItemBase item_selecionado;

    public void Update()
    {
        if (item_selecionado != null) ControlComprar(true);
        else ControlComprar(false);
    }
    public void ControlCanvas(bool ativar)
    {
        Canvas.SetActive(ativar);
    }

    public void ControlComprar(bool ativar)
    {
        Comprar.SetActive(ativar);
    }

    public void ComprarItem()
    {
        if (gm.moedas >= item_selecionado.preco)
        {
            gm.moedas -= (int)item_selecionado.preco; //NAO SEI PQ DIABOS O PRECO TA EM FLOAT MAS PREGUIÇA
            gm.MOCHILA.Add(item_selecionado);
            item_selecionado = null;
        }
    }

    public void SelecionarSlot(GameObject slot)
    {
        if (item_selecionado == null)
        {
            item_selecionado = slot.GetComponent<SlotLoja>().item;
            slot.GetComponent<Image>().color = new Color(255, 0, 0, 1f);
        }
        else
        {
            slot.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            item_selecionado = null;
        }
        
    }
}
