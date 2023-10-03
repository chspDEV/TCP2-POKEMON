using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    [Header("Posição de destino do teletransporte")]
    [Space(15)]
    [SerializeField] Vector3 tpPoint;

    GameObject player;
    [SerializeField] GameObject tempObject;

    bool playerInZone;
    [SerializeField] byte alpha;

    void Start()
    {
        playerInZone = false;
        player = GameObject.Find("Player");

        if (tempObject == null)
        {
            Debug.Log("FALHEI EM ACHAR TRANSICAO.");
        }
        else { tempObject.SetActive(false); } //Escondendo a transição
    }

    void Update()
    {

        if (playerInZone)
        {
            tempObject.SetActive(true);
            player.transform.position = tpPoint;
            StartCoroutine(ParandoTransicao());
        }
        
    }

    public IEnumerator ParandoTransicao()
    {
        yield return new WaitForSeconds(1f);
        tempObject.SetActive(false);
        yield break;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.name == "Player")
        {
            playerInZone = false;
        }
    }
}
