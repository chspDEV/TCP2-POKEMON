using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SlotMochila : MonoBehaviour
{

    [SerializeField] Mochila info;
    [SerializeField] int index;
    public Image minhaImagem;
    public TextMeshProUGUI meuTexto;

    public void Start()
    {
        //info = GameObject.Find("InfoPokemon").GetComponent<PcInfoChanger>();
        AddEvent(gameObject, EventTriggerType.PointerClick, delegate { OnClick(); });
    }

    // Update is called once per frame
    public void Update()
    {
        info.UpdateSlot(index);
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;

        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnClick()
    {
        var _pkm = info.DescobrirItem(index);

        if (_pkm != null)
        {
            info.UpdateTextInfo(_pkm);
            info.item_selecionado = _pkm;

            //Checando se for uma pokebola e tem um inimigo contra
            if (_pkm.tipoItem == TIPO_ITEM.POKEBOLA)
            {
                info.party.SetActive(false);

                if(info.gm.sistemaDeBatalha.enemyUnit.Pokemon != null)
                    info.ControlarBotao(true);
            }
            else if (info.pkm_selecionado != null)
            {
                info.ControlarBotao(false);
                info.party.SetActive(true);
            }
           
            //Resetando a cor do slot que vai ser deselecionado
            if (info.slot_selecionado != null)
                info.slot_selecionado.GetComponent<Image>().color = new Color(255, 255, 255, 1f);

            //Pasando o novo slot selecionado e colocar em vermelho
            var _slot = info.GetSlot(index);
            info.slot_selecionado = _slot;

            if (info.slot_selecionado != null)
                _slot.GetComponent<Image>().color = Color.red;
        }

    }

}
