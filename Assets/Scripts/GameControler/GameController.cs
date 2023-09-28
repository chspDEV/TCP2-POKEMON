using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { FreeRoam, Battle, Dialog, Cutscene }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;  // controle do jogador 
    [SerializeField] SistemaDeBatalha sistemaDeBatalha;
    [SerializeField] Camera cam;  // CAMERA DO JOGADOR PADRAO!!
    [SerializeField] BattleUnit PlayerBattleUnit;
    [SerializeField] BattleUnit EnemyBattleUnit;
    [SerializeField] Caminhos caminhos;
    GameState state;

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

     /*   playerController.OnEnterTrainersView += (Collider trainerCollider) =>
        {
           var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
              StartCoroutine(trainer.TriggerTrainerBattle(playerController));
                if (trainer != null)
                {
                    state = GameState.Cutscene;
                    StartCoroutine(trainer.TriggerTrainerBattle(playerController));
                }
            }
        }; */
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

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        sistemaDeBatalha.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);

        PlayerBattleUnit.DestroyInstantiatedModel();
        EnemyBattleUnit.DestroyInstantiatedModel();

        // gambiarra pra resetar a booleana de encontrar pokemon     
        StartCoroutine(playerController.ResetEncontroPokemon());
        caminhos.Resetar();
    }

    void Update()
    {

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