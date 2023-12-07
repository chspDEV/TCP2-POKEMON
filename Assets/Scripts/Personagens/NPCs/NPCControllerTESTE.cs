    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControllerTESTE : MonoBehaviour, Interactable//, ISavable
{
    [SerializeField] Dialog dialog;
    [SerializeField] DialogManager dialog_manager;

    /*
    [Header("Quests")]
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;

    */

    [Header("Movement")]
    [SerializeField] List<Vector3> movementPattern;
    [SerializeField] float timeBetweenPattern;
    [SerializeField] bool QueroAndar;
    
    //public bool startdl;

    [SerializeField] NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;
    //Quest activeQuest;

    Character character;
    private bool CheckSpace = false;
    [SerializeField] GameObject SpaceBar;
    //ItemGiver itemGiver;
    //PokemonGiver pokemonGiver;
    //Healer healer;
    //Merchant merchant;



    private void Awake()
    {
        dialog_manager = GetComponent<DialogManager>();
        character = GetComponent<Character>();
        //itemGiver = GetComponent<ItemGiver>();
        //pokemonGiver = GetComponent<PokemonGiver>();
        //healer = GetComponent<Healer>();
        // merchant = GetComponent<Merchant>();

    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
           // character.LookTowards(initiator.transform.position);
            state = NPCState.Dialog;
            yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
           // yield return StartCoroutine(dialog_manager.ShowDialog(dialog));
            
            Debug.Log("Estou rodando");

            idleTimer = 0f;
            state = NPCState.Idle;
            //startdl = false;
        }
    }

    private void Update()
    {
        //Ativando e Desativando barra se posso interagir
        SpaceBar.SetActive(CheckSpace);

        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
        }

        character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        if(QueroAndar)
        {
            state = NPCState.Walking;

            var oldPos = transform.position;

            yield return character.Move(movementPattern[currentPattern]);

            if (transform.position != oldPos)
                currentPattern = (currentPattern + 1) % movementPattern.Count;

            state = NPCState.Idle;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CheckSpace = true;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CheckSpace = false;
        }

    }

    #region SAVE
    /*
public object CaptureState()
{
var saveData = new NPCQuestSaveData();
saveData.activeQuest = activeQuest?.GetSaveData();

if (questToStart != null)
    saveData.questToStart = (new Quest(questToStart)).GetSaveData();

if (questToComplete != null)
    saveData.questToComplete = (new Quest(questToComplete)).GetSaveData();

return saveData;
}

public void RestoreState(object state)
{
var saveData = state as NPCQuestSaveData;
if (saveData != null)
{
    activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;

    questToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;
    questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;
}
}
}

[System.Serializable]
public class NPCQuestSaveData
{
public QuestSaveData activeQuest;
public QuestSaveData questToStart;
public QuestSaveData questToComplete;
}

*/
    #endregion
}

//ESTADOS
public enum NPCState { Idle, Walking, Dialog }
