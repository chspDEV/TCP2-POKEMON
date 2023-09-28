using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{

    GameObject player;
    bool playerInZone;

    [Header("Posição de destino do teletransporte")]
    [Space(15)]
    [SerializeField] Vector3 tpPoint;

    void Start()
    {
        playerInZone = false;
        player = GameObject.Find("Player");

    }

    void Update()
    {

        if (playerInZone)
        {
            player.transform.position = tpPoint;
        }

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
