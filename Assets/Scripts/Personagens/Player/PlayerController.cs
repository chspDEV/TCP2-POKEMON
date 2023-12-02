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
        if (Input.GetKeyDown(KeyCode.F1)) { transform.position = new Vector3(-123.09f, 19.53f, 176.67f); }//lab
        if (Input.GetKeyDown(KeyCode.F2)) { transform.position = new Vector3(93f, 15f, 40f); }//meio pallet
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f, Interagivel))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green); // Desenha o raio em verde

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                yield return interactable?.Interact(transform);  
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.red); // Desenha o raio em vermelho quando não atinge nada
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
