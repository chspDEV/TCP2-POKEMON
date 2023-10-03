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

    GameObject target;
    PlayerController player;
    Animator anim;
    [SerializeField] GameObject tempObject;

    bool playerInZone;
    [SerializeField] byte alpha;

    short buVel; //SALVANDO VELOCIDADE 
    short buVelcor; //SALVANDO VELOCIDADE CORRIDA

    void Start()
    {
        playerInZone = false;
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();

        buVel = player.velocidade;
        buVelcor = player.velocidadeCorrida;

        if (tempObject == null)
        {
            Debug.Log("FALHEI EM ACHAR TRANSICAO.");
        }
        else { tempObject.SetActive(false); } //Escondendo a transição

        anim = tempObject.GetComponent<Animator>();
    }

    void Update()
    {

        if (playerInZone)
        {
            //anim.Play("Transicao");
            target.transform.position = tpPoint;
            StopAllCoroutines();
            StartCoroutine(Transicao());
        }
        
    }

    public IEnumerator Transicao()
    {
        tempObject.SetActive(true);

        //Impedindo jogador de andar
        player.velocidade = 0;
        player.velocidadeCorrida = 0;

        yield return new WaitForSeconds(1f);

        //Permitindo ele andar
        player.velocidade = buVel;
        player.velocidadeCorrida = buVelcor;
        tempObject.SetActive(false);

        //anim.Play("Idle");
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
