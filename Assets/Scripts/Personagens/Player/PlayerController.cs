using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] GameController gm;
    #region variaveis
    [Header("Configurações do Player")]
    public short velocidade;
    public short velocidadeCorrida;

    public bool CanInteract = false;
    public GameObject InteractObject;

    [Range(0.1f,10f)]
    public float fallSpeed; // Ajuste esse valor para a velocidade desejada.

    [SerializeField] LayerMask GramaAlta;
    [SerializeField] LayerMask FovLayer; // TRIGER PARA DETECTAR SE O TREINADOR INIMIGO TE VIU
    [SerializeField] LayerMask Interagivel;
    [SerializeField] private bool TenhoSapato;
    public bool seMovendo = false;

    public event Action OnEncountered;
    public event Action<Collider> OnEnterTrainersView;
    public  Rigidbody rb;
    public  Animator anim;
    public GameObject InteractOBJ;
    public bool encontrouPokemon = false;

    [SerializeField] SistemaDeBatalha sistema;

    public Vector3 Position;
    private Quaternion currentRotation;
    [SerializeField] private short rotationSpeed; //45


    #endregion
    public bool moveEnabled = true;
    private void Start()
    {
        TenhoSapato = false;
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, -fallSpeed * Time.fixedDeltaTime, 0); // Ajuste os eixos e a velocidade conforme necessário./

    }

    public void HandleUpdate() 
    {
        Move();

        #region HACK
        if (Input.GetKeyDown(KeyCode.F1)) { transform.position = new Vector3(-109.15f, 0.52f, -0.38f); }//casa
        if (Input.GetKeyDown(KeyCode.F2)) { transform.position = new Vector3(-6.12f, 3.66f, 0f); }//meio pallet
        if (Input.GetKeyDown(KeyCode.F3)) { transform.position = new Vector3(-143.13f, 0.52f, -0.38f); }//laboratorio
        if (Input.GetKeyDown(KeyCode.F4)) { transform.position = new Vector3(-127f, 0.52f, -2.42f); }//pokecenter
        if (Input.GetKeyDown(KeyCode.F5)) { transform.position = new Vector3(-159.4f, 0.52f, -2.42f); }//pokemart
        if (Input.GetKeyDown(KeyCode.F6)) { transform.position = new Vector3(-159.4f, 0.52f, -2.42f); }//ginasio
        //adicionar viridian qnd tiver
        #endregion
    }


    public void Move()
    {
        if (moveEnabled)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Position = new Vector3(horizontal, -fallSpeed, vertical);

            // Correr se estiver apertando shift
            if (Input.GetKey(KeyCode.LeftShift) && TenhoSapato)
            {
                // Se movendo na diagonal, divide a velocidade por 2
                float diagonalFactor = (Mathf.Abs(horizontal) > 0 && Mathf.Abs(vertical) > 0) ? 1.3f : 1f;
                rb.velocity = ((Position * velocidadeCorrida * 10) / diagonalFactor) * Time.fixedDeltaTime;
            }
            else // Não tenho sapato T-T
            {
                // Se movendo na diagonal, divide a velocidade por 2
                float diagonalFactor = (Mathf.Abs(horizontal) > 0 && Mathf.Abs(vertical) > 0) ? 2f : 1f;
                rb.velocity = ((Position * velocidade * 10)/diagonalFactor) * Time.fixedDeltaTime;
            }

            // nathan: simplifiquei a gambiarra 
            float angle = Mathf.Atan2(Position.x, Position.z) * Mathf.Rad2Deg;

            if (Position.sqrMagnitude > 0.1f)
            {
                currentRotation.eulerAngles = new Vector3(0, angle, 0);
                transform.rotation = currentRotation;
            }

            // Atualizando a var seMovendo
            seMovendo = Position.sqrMagnitude > 0.1f;
            anim.SetBool("andando", seMovendo);
            OnMoveOver();
        }
    }



    private void OnMoveOver()
    {
        EncontrarPokemons();
        ChecandoEncontroDeTreinador();

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Interact());
        }
    }
    private void EncontrarPokemons()  // Falta fazer o encontro de pokemons resetar a variavel para false depois de encontrar algum pokemon!
    {
        if (encontrouPokemon == false)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, GramaAlta);

            if (colliders.Length > 0 && UnityEngine.Random.Range(1, 101) <= 10 && seMovendo && sistema.PlayerCanBattle == true)
            {
                    Debug.Log("Um pokemon selvagem apareceu");
                //Animação ficara falsa aqui!
                    OnEncountered();
                    encontrouPokemon = true;

            }
        }
    }

    private void ChecandoEncontroDeTreinador()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, FovLayer);

        if (colliders.Length > 0 )
        {
            Debug.Log("Está na visão do treinador!");
            //Animação ficara falsa aqui!
            OnEnterTrainersView?.Invoke(colliders[0]);
        }
    }

    IEnumerator Interact()
    {
        if (CanInteract)
        {
            //Para NPCS
            Interactable interactable = InteractOBJ.GetComponent<Interactable>();

            //Para MESA
            MesaCarvalho mesa = InteractOBJ.GetComponent<MesaCarvalho>();

            //Para PC
            ComputadorController pc = InteractOBJ.GetComponent<ComputadorController>();

            Debug.Log("Interagi");

            if (interactable != null)
            {
                yield return interactable.Interact(transform);
            }

            if (mesa != null)
            {
                mesa.Ativar();
            }

            if (pc != null)
            {
                pc.AbrirPC();
            }

        }
        else
        {
            Debug.Log("N interagi");
        }
    }



    public IEnumerator ResetEncontroPokemon()
    {
        yield return new WaitForSeconds(3);
        encontrouPokemon = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GramaAlta")
        {
            gm.area = other.GetComponent<MapArea>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GramaAlta")
        {
            gm.area = null;
        }
    }

}
