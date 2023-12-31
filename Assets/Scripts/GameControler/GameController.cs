using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;  // controle do jogador 
    [SerializeField] VFXController vfxController;  // controle de vfx

    public SistemaDeBatalha sistemaDeBatalha;
    public Camera cam;  // CAMERA DO JOGADOR PADRAO!!
    [SerializeField] BattleUnit PlayerBattleUnit;
    [SerializeField] BattleUnit EnemyBattleUnit;
    [SerializeField] Caminhos caminhos;
    [SerializeField] public GameObject arbusto;
    [SerializeField] public GameObject arbusto2;

    //MOCHILA
    public ItemBase item_atual;
    public Pokemon pkm_selecionado;
    public List<ItemBase> MOCHILA;
    public Mochila mochila;
    public Mochila mochilaMenu;

    public List<Pokemon> PC;

    public MapArea area;

    public int moedas;
    public GameState state;

    public void Awake()
    {
        ConditionsDB.Init();
    }
    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        sistemaDeBatalha.OnBattleOver += EndBattle;
        

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnDialogFinished += () =>     //  DialogManager.Instance.OnCloseDialog 
        {
            if(state == GameState.Dialog)
            state = GameState.FreeRoam;
        };

    }
    void deletarArbusto()
    {
        if (sistemaDeBatalha.treinador_atual.name == "Brock")
        {
            Debug.Log("Deletando arbustos");

            if (arbusto != null)
            {
                Debug.Log("Destruindo arbusto");
                Destroy(arbusto);
            }

            if (arbusto2 != null)
            {
                Debug.Log("Destruindo arbusto2");
                Destroy(arbusto2);
            }
            else
            {
                Debug.LogWarning("Arbusto2 � nulo!");
            }
        }
        
        return;
    }


    void StartBattle()
    {
        state = GameState.Battle;
        sistemaDeBatalha.gameObject.SetActive(true);
        cam.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var pokemonSelvagem = area.GetRandomWildPokemon();

        sistemaDeBatalha.StartBattle(playerParty, pokemonSelvagem);
    }

    public void UsarItem()
    {
        switch (state)
        {
            case GameState.FreeRoam:
                if (item_atual.tipoItem == TIPO_ITEM.CONSUMIVEL)
                {
                    Debug.Log("USEI CONSUMIVEL");
                    item_atual.StatsUp(pkm_selecionado);
                    
                    LimparInfoMochila();
                    break;
                }
                break;
            case GameState.Battle:
                if (item_atual.tipoItem == TIPO_ITEM.CONSUMIVEL)
                {
                    Debug.Log("USEI CONSUMIVEL EM BATALHA");

                    var pkm = sistemaDeBatalha.GetPokemonPlayer(pkm_selecionado);

                    Debug.Log(pkm.HP);
                    item_atual.StatsUp(pkm);

                    if (item_atual.tipoItem == TIPO_ITEM.CONSUMIVEL && item_atual.statsUP == STATUS_AUMENTAR.LEVEL)
                    {
                        StartCoroutine(sistemaDeBatalha.ChecarEvolucao(pkm));
                        
                    }

                    sistemaDeBatalha.playerUnit.Hud.SetarLevel(pkm_selecionado);

                    sistemaDeBatalha.dialogBox.AtivarSelecaoMochila(false);
                    sistemaDeBatalha.dialogBox.AtivarSelecaoAcao(false);

                    //Esperar curar
                    StartCoroutine(sistemaDeBatalha.Esperar(2f));
                    sistemaDeBatalha.dialogBox.SetDialog($"Voc� usou {item_atual.name}.");

                    StartCoroutine(sistemaDeBatalha.TurnoInimigo());
                    LimparInfoMochila();
                    break;
                } 

                if (item_atual.tipoItem == TIPO_ITEM.POKEBOLA)
                {
                    sistemaDeBatalha.StartCoroutine(sistemaDeBatalha.TryToCatch());
                    sistemaDeBatalha.dialogBox.AtivarSelecaoMochila(false);
                    Debug.Log("USEI POKEBOLA");
                    sistemaDeBatalha.dialogBox.AtivarSelecaoAcao(false);
                    LimparInfoMochila();
                    break;
                }
                    
                break;
            case GameState.Dialog:
                break;
            case GameState.Cutscene:
                break;
        }
    }

    public void LimparInfoMochila()
    {

        RemoverItem();

       

        NularMochila(mochila);
        NularMochila(mochilaMenu);

        item_atual = null;
        pkm_selecionado = null;
    }

    public void NularMochila(Mochila _mochila)
    {
        //Primeiro atualizar a mochila
        var _p = _mochila.GetComponent<PcInfoChanger>();

        for (int i = 0; i < _p.slotsParty.Length; i++)
        {
            _p.UpdateSlotParty(i);
        }

        //Agora reseta oq tem que ser resetado
        _mochila.botao.SetActive(false);

        if(_mochila.slot_selecionado != null)
            _mochila.ResetarCorSlot();

        _mochila.GetComponent<PcInfoChanger>().ResetarCorSlots();
        Debug.Log("LIMPARINFOMOCHILA");
        _mochila.textoItem.text = "";

        _mochila.party.SetActive(false);

        _mochila.slot_selecionado = null;
        _mochila.pkm_selecionado = null;
        _mochila.item_selecionado = null;
        _mochila.GetComponent<PcInfoChanger>().slot_selecionado = null;
        _mochila.GetComponent<PcInfoChanger>().pkm_selecionado = null;
    }

    public void StartBattleTrainer(PokemonParty _tParty)
    {
        state = GameState.Battle;
        sistemaDeBatalha.gameObject.SetActive(true);
        cam.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = _tParty;

        sistemaDeBatalha.StartTrainerBattle(playerParty, trainerParty);
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;

        PlayerBattleUnit.DestroyInstantiatedModel();
        EnemyBattleUnit.DestroyInstantiatedModel();

        // gambiarra pra resetar a booleana de encontrar pokemon     
        StartCoroutine(playerController.ResetEncontroPokemon());
        caminhos.Resetar();

        sistemaDeBatalha.gameObject.SetActive(false);
        
        cam.gameObject.SetActive(true);
    }

    public void RemoverItem()
    {
        if (item_atual != null)
        {
            if (item_atual.amount > 1)
            {
                item_atual.amount--;
            }
            else
            {
                for (int i = 0; i < MOCHILA.Count; i++)
                {
                    if (MOCHILA[i].name == item_atual.name)
                    {
                        MOCHILA.Remove(item_atual);
                        break;
                    }
                }
            }
        }
    }

    public void AdicionarItem(ItemBase item)
    {
        var igual = false;

        for (int i = 0; i < MOCHILA.Count; i++)
        {
            if (MOCHILA[i].name == item.name)
            {
                igual = true;
                MOCHILA[i].amount++;
                break;
            }
        }

        //Nao tem nenhum item igual na mochila
        if(igual == false)
        MOCHILA.Add(item);
        
    }

    void Update()
    {
        if (sistemaDeBatalha != null && sistemaDeBatalha.treinador_atual != null)
        {
            deletarArbusto();
            // Outro c�digo do Update, se necess�rio
        }
        //REINICIANDO O JOGO
        //if (Input.GetKeyDown(KeyCode.Keypad8)) { AdicionarItem(item_atual); }
        if (Input.GetKeyDown(KeyCode.Keypad9)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
            if (playerController.encontrouPokemon && playerController.seMovendo && state != GameState.FreeRoam)
            {
               playerController.rb.velocity = Vector3.zero;
               playerController.seMovendo = false;
               return;
            }
             
        }
        else if (state == GameState.Battle)
        {
            sistemaDeBatalha.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }

}
