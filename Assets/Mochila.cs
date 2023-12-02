using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class Mochila : MonoBehaviour
{
    public GameController gm;
    public GameObject[] slots;

    public GameObject party;
    public GameObject botao;


    public GameObject slot_selecionado;
    public Pokemon pkm_selecionado;
    public ItemBase item_selecionado;



    public TextMeshProUGUI textoItem;

    public void Awake()
    {
        gm = GameObject.Find("GameController").GetComponent<GameController>();
        pkm_selecionado = GetComponent<PcInfoChanger>().pkm_selecionado;
    }

    public void Update()
    {
        gm.item_atual = item_selecionado;
    }

    public void UpdateSlot(int _index)
    {
        if (_index < gm.MOCHILA.Count)
        {
            //Atualizando imagem do ITEM na MOCHILA
            slots[_index].GetComponent<SlotMochila>().minhaImagem.sprite = gm.MOCHILA[_index].sprite;
            var imgColor = slots[_index].GetComponent<SlotMochila>().minhaImagem.color;
            slots[_index].GetComponent<SlotMochila>().minhaImagem.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f);
            slots[_index].GetComponent<SlotMochila>().meuTexto.text = $"x{gm.MOCHILA[_index].amount}";
        }
        else
        {
            //Se não existe pokemon nesse index, deixe tudo em branco!
            slots[_index].GetComponent<SlotMochila>().minhaImagem.sprite = null;
            slots[_index].GetComponent<SlotMochila>().minhaImagem.color = new Color(1, 1, 1, 0f);
            slots[_index].GetComponent<SlotMochila>().meuTexto.text = $"";
        }

    }

    public void ResetarCorSlot()
    {
        slot_selecionado.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
    }

    public ItemBase DescobrirItem(int _index)
    {
        if (_index < gm.MOCHILA.Count) { return gm.MOCHILA[_index]; }
        else { return null; }
    }

    public void UpdateTextInfo(ItemBase _item)
    {
        textoItem.text = _item.nome + ": " + _item.descricao;
    }

    public void DeleteTextInfo()
    {
        textoItem.text = "";
    }

    public void ControlarBotao(bool valor)
    {
        botao.SetActive(valor);
    }

    public GameObject GetSlot(int _index)
    {
        if (_index < gm.MOCHILA.Count)
        {
            return slots[_index];
        }
        else return null;
    }


}
