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

    public List<Pokemon> PC;
    

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
        var pokemonSelvagem = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        sistemaDeBatalha.StartBattle(playerParty, pokemonSelvagem);
    }

    public void UsarItem()
    {
        switch (state)
        {
            case GameState.FreeRoam:
                if (item_atual.tipoItem == TIPO_ITEM.CONSUMIVEL)
                {
                    item_atual.StatsUp(pkm_selecionado);
                    LimparInfoMochila();
                    break;
                }
                
                break;
            case GameState.Battle:
                if (item_atual.tipoItem == TIPO_ITEM.CONSUMIVEL)
                {
                    item_atual.StatsUp(pkm_selecionado);
                    LimparInfoMochila();
                    break;
                } 

                if (item_atual.tipoItem == TIPO_ITEM.POKEBOLA)
                {
                    sistemaDeBatalha.StartCoroutine(sistemaDeBatalha.TryToCatch());
                    sistemaDeBatalha.dialogBox.AtivarSelecaoMochila(false);
                    sistemaDeBatalha.dialogBox.AtivarSelecaoAcao(false);
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

    void Update()
    {
        //REINICIANDO O JOGO
        if (Input.GetKeyDown(KeyCode.F5)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

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
