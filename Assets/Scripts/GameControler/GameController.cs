using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;  // controle do jogador 
    [SerializeField] VFXController vfxController;  // controle de vfx

    [SerializeField] SistemaDeBatalha sistemaDeBatalha;
    public Camera cam;  // CAMERA DO JOGADOR PADRAO!!
    [SerializeField] BattleUnit PlayerBattleUnit;
    [SerializeField] BattleUnit EnemyBattleUnit;
    [SerializeField] Caminhos caminhos;

    //MOCHILA
    public ItemBase item_atual;
    public Pokemon pkm_selecionado;
    public List<ItemBase> MOCHILA;
    public Mochila mochila;

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
                    sistemaDeBatalha.playerUnit.Hud.SetarLevel(pkm_selecionado);

                    sistemaDeBatalha.dialogBox.AtivarSelecaoMochila(false);
                    sistemaDeBatalha.dialogBox.AtivarSelecaoAcao(false);

                    //Esperar curar
                    StartCoroutine(sistemaDeBatalha.Esperar(2f));
                    sistemaDeBatalha.dialogBox.SetDialog($"Você usou {item_atual.name}.");

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
        mochila.ResetarCorSlot();
        mochila.GetComponent<PcInfoChanger>().ResetarCorSlots();
        Debug.Log("LIMPARINFOMOCHILA");
        mochila.textoItem.text = "";
        mochila.party.SetActive(false);
        item_atual = null;
        pkm_selecionado = null;
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
        //REINICIANDO O JOGO
        if (Input.GetKeyDown(KeyCode.F5)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        if (Input.GetKeyDown(KeyCode.F6)) { AdicionarItem(item_atual); }

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
