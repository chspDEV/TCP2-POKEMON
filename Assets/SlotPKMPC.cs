using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SlotPKMPC : MonoBehaviour
{
    [SerializeField] GameObject gm;
    [SerializeField] GameObject[] slot;
    [SerializeField] PcInfoChanger info;
    [SerializeField] Image slotImage;
    Pokemon essePkm;



    public void Start()
    {
        info = GameObject.Find("InfoPokemon").GetComponent<PcInfoChanger>();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnter(essePkm); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExit(); });
    }

    // Update is called once per frame
    void Update()
    {
        AtualizarSlot();
    }
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;

        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(Pokemon pkm)
    {
        info.UpdateText(pkm);
    }

    public void OnExit()
    {
        info.DeleteText();
    }

    public void AtualizarSlot()
    {
        GameController _gm = gm.GetComponent<GameController>();

        for (int i = 0; i < _gm.PC.Count; i++)
        {
            _gm.PC[i].Init();
            essePkm = _gm.PC[i];

            if (_gm.PC[i].Sprite != null && _gm.PC[i].nome != "PlaceHolder")
            {
                slot[i].GetComponent<SlotPKMPC>().slotImage.sprite = _gm.PC[i].Sprite;
            }
            else { slot[i].GetComponentInChildren<Image>().sprite = null; }

            if (_gm.PC[i].nome != null && _gm.PC[i].nome != "PlaceHolder")
            {
                slot[i].GetComponentInChildren<TextMeshProUGUI>().text = _gm.PC[i].nome + " Lv " + _gm.PC[i].level.ToString("n0");

                //slot[i].GetComponentInParent<TextMeshProUGUI>().text = _gm.PC[i].HP.ToString("n0") + " / " + _gm.PC[i].VidaMax.ToString("n0");

            }
            else { slot[i].GetComponentInChildren<TextMeshProUGUI>().text = ""; }
                
        }

    }
}
