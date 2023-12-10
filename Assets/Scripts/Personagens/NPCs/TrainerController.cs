using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    //[SerializeField] DialogManager dialog_manager;

    //[SerializeField] float speed;
    //[SerializeField] float stopDistance;

    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject batalha;
    [SerializeField] GameObject eu;
    [SerializeField] GameObject canvasDialogo;
    public GameObject modelo3D;

    //
    [SerializeField] GameController gm;
    [SerializeField] SistemaDeBatalha sistema;

    [SerializeField] bool posso_mostrar = false;
    [SerializeField] bool posso_ativar = false;
    public bool posso_batalha = false;
    public bool PerdiBatalha = false;
    public bool _PlayerCanBattle = false;
    public bool falei = false;

    [SerializeField] GameObject target;
    short buVel;
    short buVelcor;
    PlayerController player;
    [SerializeField] Animator playerAnim;
    Rigidbody rb;

    public BoxCollider col;

    //[SerializeField] float _increase;
    //[Range(1f, 1000f)]
    //[SerializeField] float _increaseMax;

    //Character character;
    //private Quaternion currentRotation;
    //Transform targetPos;

    public void Start()
    {
        target = GameObject.Find("Player");
        //character = GetComponent<Character>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        //Pegando o RIGIDBODY do meu alvo e parando ele:
        player = target.GetComponent<PlayerController>();
        _PlayerCanBattle = sistema.PlayerCanBattle;
    }

    public void ExclamationOff() 
    { 
        exclamation.SetActive(false);
    }

    public void Update()
    {
        _PlayerCanBattle = sistema.PlayerCanBattle;
        target = GameObject.Find("Player");

        //Temporizador até começar a batalha
        if (Input.GetKeyDown(KeyCode.E) && !falei && posso_ativar)
        {
            falei = true;
            posso_batalha = true;

            //Checando batalha pela segunda vez (deve iniciar aqui)
            CheckForBattle();
        }

    }

    public void CheckForBattle()
    {
        //Desativando e ativando colisao
        /*
        if (!_PlayerCanBattle || PerdiBatalha)
        {
            col.enabled = false;
        }
        else { col.enabled = true; }
        */

        //Se ainda nao perdi a batalha e player pode batalhar:
        if (!PerdiBatalha && _PlayerCanBattle)
        {
            if (posso_mostrar)
            {
                //StartCoroutine(MoveTowardsPlayer());

                buVel = player.velocidade;
                buVelcor = player.velocidadeCorrida;

                //Parando o jogador
                player.velocidade = 0;
                player.rb.velocity = Vector3.zero;
                player.velocidadeCorrida = 0;


                //Chamando dialogo
                playerAnim.SetBool("andando", false);
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

                posso_mostrar = false; 

            }
            else if (posso_ativar)
            {
                if (posso_batalha)
                {
                    col.enabled = false;
                    canvasDialogo.SetActive(false); //Desativando canvas de dialogo
                    batalha.SetActive(true); //Ativando a cena

                    //Dizendo pro sistema de batalha quem eu sou!
                    sistema.treinador_atual = eu;

                    gm.StartBattleTrainer(this.GetComponent<PokemonParty>());
                    //sistema.StartTrainerBattle(target.GetComponent<PokemonParty>(), this.GetComponent<PokemonParty>());

                    player.velocidade = buVel;
                    player.velocidadeCorrida = buVelcor;
                    posso_ativar = false;
                    posso_batalha = false;

                }
                else
                {
                    Debug.Log("Nao posso batalhar.");
                }
            }
            else
            {
                Debug.Log("Nao posso ativar.");
            }

        }
        else
        {
            Debug.Log("Ja perdi nao posso batalhar");
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && posso_ativar == false && _PlayerCanBattle)
        {
            posso_mostrar = true; //Começa a co-rotina
            posso_ativar = true; // Garante que nao rode esse codigo novamente

            exclamation.SetActive(true); //Mostra a exclamacao

            Invoke("ExclamationOff", 0.5f);

            //Checando batalha pela primeira vez
            CheckForBattle();
        }
    }

   
}
