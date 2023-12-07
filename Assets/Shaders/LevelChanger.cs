using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    [Header("Posição de destino do teletransporte")]
    [Space(15)]
    [SerializeField] GameObject tpPoint;
    [SerializeField] GameObject tpPointCentro;
    

    GameObject target;
    PlayerController player;
    Animator anim;
    [SerializeField] GameObject tempObject; //Transicao

    bool playerInZone;
    byte alpha;

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

        //anim = tempObject.GetComponent<Animator>();
    }

    void Update()
    {
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();

        if (playerInZone)
        {
            Teleporte();
            StartCoroutine(Transicao());
        }
        
    }

    public void Teleporte()
    {
        //anim.Play("Transicao");

        var _posicao = tpPoint.transform.position;
        target.transform.position = _posicao;
        StopAllCoroutines(); 
    }

    public void TeleporteCentroPokemon()
    {
        var _posicao = tpPointCentro.transform.position;
        target.transform.position = _posicao;
        StopAllCoroutines();
    }

    public void DevolverVel()
    {
        var _target = GameObject.Find("Player");
        var _player = target.GetComponent<PlayerController>();

        //Permitindo ele andar
        _player.velocidade = buVel;
        _player.velocidadeCorrida = buVelcor;
    }

    public void TirarVel()
    {
        var _target = GameObject.Find("Player");
        var _player = target.GetComponent<PlayerController>();

        //Permitindo ele andar
        _player.velocidade = 0;
        _player.velocidadeCorrida = 0;
    }

    public IEnumerator Transicao()
    {
        tempObject.SetActive(true);

        var _target = GameObject.Find("Player");
        var _player = target.GetComponent<PlayerController>();

        //Impedindo jogador de andar
        _player.velocidade = 0;
        _player.velocidadeCorrida = 0;

        yield return new WaitForSeconds(0.8f);

        //Permitindo ele andar
        _player.velocidade = buVel;
        _player.velocidadeCorrida = buVelcor;

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
