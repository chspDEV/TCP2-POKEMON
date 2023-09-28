using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region variaveis
    [Header("Configurações do Player")]
    [SerializeField] public short velocidade;
    [SerializeField] public short velocidadeCorrida;
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

    public Vector3 Position;
    private Quaternion currentRotation;
    [SerializeField] private short rotationSpeed; //45
    

    #endregion

    //by Nathan:
    /* o metodo HandleUpdate não e um update, ele so tem esse nome porem e um metodo comum q e 
       chamado atraves de outro script, caso na sua cena não tenha um script cahamado Gamecontroller 
       e por conta disso q a movimentação não funcionava, pois o HandleUpadate do player e chamado atraves de outro script.
    */

    private void Start()
    {
        TenhoSapato = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }
    public void HandleUpdate() 
    {
        Move();   
    }

    private void Move()
    {
        Position = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // Correr se estiver apertando shift
        if (Input.GetKey(KeyCode.LeftShift) && TenhoSapato)
        {
            rb.velocity = Position * velocidadeCorrida;
        }
        else // Não tenho sapato T-T
        {
            rb.velocity = Position * velocidade;
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
        OnMoveOver();
    }



    private void OnMoveOver()
    {
        EncontrarPokemons();
        ChecandoEncontroDeTreinador();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Interact());
        }
    }
    private void EncontrarPokemons()  // Falta fazer o encontro de pokemons resetar a variavel para false depois de encontrar algum pokemon!
    {
        if (!encontrouPokemon)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, GramaAlta);

            if (colliders.Length > 0 && UnityEngine.Random.Range(1, 101) <= 10 && seMovendo)
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, Interagivel))
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
        yield return new WaitForSeconds(7);
        encontrouPokemon = false;
    }
    
}