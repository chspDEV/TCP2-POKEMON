using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SlotPKM : MonoBehaviour
{

    [SerializeField] PcInfoChanger info;
    [SerializeField] int index;
    public Image minhaImagem;


    public void Start()
    {
        //info = GameObject.Find("InfoPokemon").GetComponent<PcInfoChanger>();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnter(); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExit(); });
        AddEvent(gameObject, EventTriggerType.PointerClick, delegate { OnClick(); });
    }

    // Update is called once per frame
    public void Update()
    {
        info.UpdateSlotParty(index);
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;

        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter()
    {
        var _pkm = info.DescobrirPkmParty(index);

        if (_pkm != null) info.UpdateTextInfo(_pkm);    
    }

    public void OnExit()
    {
        info.DeleteTextInfo();
    }

    public void OnClick()
    {
        info.ControlarBotao(false, true);

        var _pkm = info.DescobrirPkmParty(index);
        if (_pkm != null) info.pkm_selecionado = _pkm;
    }


}
