
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SlotPKM : MonoBehaviour
{

    [SerializeField] PcInfoChanger info;
    [SerializeField] int index;
    public Image minhaImagem;
    public Slider minhaVida;


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
        
        var _pkm = info.DescobrirPkmParty(index);
        Debug.Log("PKM CLICADO = " + _pkm);
        if (_pkm != null && _pkm.nome != "PlaceHolder")
        {
            //Se ja selecionei um item e preciso selecionar um pokemon -> ligar botao usar
            info.pkm_selecionado = _pkm;

            info.ControlarBotao(false, true);

            if (info.gm.item_atual != null)
            {
                //ativando os dois botao memo e fe
                Debug.Log("RODEI ATIVAR BOTAO");

                if (info.gm.item_atual.statsUP == STATUS_AUMENTAR.VIDA && _pkm.HP == _pkm.VidaMax)
                { 
                    //Nao ativar botao se for usar potion e estiver de vida cheia (coxascode)
                }
                else
                {
                    info.gm.mochila.botao.SetActive(true);
                    info.gm.mochilaMenu.botao.SetActive(true);
                }

            }
            

            //Resetando a cor do slot que vai ser deselecionado
            if (info.slot_selecionado != null)
            info.slot_selecionado.GetComponent<Image>().color = new Color(255, 255, 255, 1f);

            //Pasando o novo slot selecionado e colocar em vermelho
            var _slot = info.GetSlotParty(index);
            info.slot_selecionado = _slot;
            _slot.GetComponent<Image>().color = Color.red;  
        }
       
            
    }


}
