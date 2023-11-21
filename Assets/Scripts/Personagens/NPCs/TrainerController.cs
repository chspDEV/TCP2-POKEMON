using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    //[SerializeField] DialogManager dialog_manager;

    [SerializeField] float speed;
    [SerializeField] float stopDistance;

    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject batalha;
    [SerializeField] GameObject eu;
    [SerializeField] GameObject canvasDialogo;

    //
    [SerializeField] GameController gm;
    [SerializeField] SistemaDeBatalha sistema;

    [SerializeField] bool posso_mostrar = false;
    [SerializeField] bool posso_ativar = false;
    public bool posso_batalha = false;
    public bool PerdiBatalha = false;
    public bool _PlayerCanBattle = false;

    [SerializeField] GameObject target;
    short buVel;
    short buVelcor;
    PlayerController player;
    Rigidbody rb;

    BoxCollider col;

    [SerializeField] float _increase;
    [Range(1f, 1000f)]
    [SerializeField] float _increaseMax;

    //Character character;
    //private Quaternion currentRotation;
    //Transform targetPos;

    public void Start()
    {
        target = GameObject.Find("Player");
        //character = GetComponent<Character>();
        rb = GetComponent<Rigidbody>();

        //Pegando o RIGIDBODY do meu alvo e parando ele:
        player = target.GetComponent<PlayerController>();

        _PlayerCanBattle = sistema.PlayerCanBattle;

        col = GetComponent<BoxCollider>();

    }

    public void ExclamationOff() 
    { 
    exclamation.SetActive(false);
    }
    public void Update()
    {
        _PlayerCanBattle = sistema.PlayerCanBattle;
        target = GameObject.Find("Player");

        if (!_PlayerCanBattle)
        {
            col.enabled = false;
        }
        else { col.enabled = true; }

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
                player.velocidadeCorrida = 0;

                //Chamando dialogo
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
            }
            //Temporizador até começar a batalha
            if (posso_ativar)
            {
                if (_increase <= _increaseMax)
                {
                    _increase += 0.01f;
                }
                else { posso_batalha = true; }
            }

        }

    }

    //private IEnumerator MoveTowardsPlayer()
    //{
    //    // Mantém o movimento enquanto o jogador estiver dentro do trigger.
    //    while (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
    //    {
    //        // Calcula a direção do jogador em relação a este objeto.
    //        Vector3 direction = (player.transform.position - transform.position).normalized;

    //        // Move o objeto em direção ao jogador com a velocidade especificada.
    //        rb.velocity = direction * speed;

    //        // Aguarda o próximo quadro antes de continuar o movimento.
    //        yield return null;
    //    }
    //}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && posso_ativar == false && _PlayerCanBattle)
        {
            posso_mostrar = true; //Começa a co-rotina
            posso_ativar = true; // Garante que nao rode esse codigo novamente

            exclamation.SetActive(true); //Mostra a exclamacao
            Invoke("ExclamationOff", 0.5f);
        }
    }

   
}
