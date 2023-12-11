using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotLoja : MonoBehaviour
{
    public Image minhaImagem;
    public TextMeshProUGUI meuTexto;
    public ItemBase item;

    // Update is called once per frame
    void Update()
    {
        minhaImagem.sprite = item.sprite;
        meuTexto.text = $"{item.nome} \n {item.preco} moedas";
        
    }
}
