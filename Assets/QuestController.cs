using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Authentication.ExtendedProtection;

public class QuestController : MonoBehaviour
{
    [Header("Jogador")]
    [Space(15)]
    [SerializeField] PlayerController playerController;  // controle do jogador 
    [SerializeField] GameObject player;
    [SerializeField] PokemonParty pokemonParty;

    [Header("Importantes")]
    [Space(15)]
    [SerializeField] SistemaDeBatalha sistemaDeBatalha;
    [SerializeField] Camera cam;  // CAMERA DO JOGADOR PADRAO!!
    [SerializeField] GameObject tpPointCentro;

    [Header("Quest [1]")]
    [Space(15)]
    [SerializeField] GameObject mesaCarvalho;
    [SerializeField] GameObject escolhaPokemon;
    [SerializeField] GameObject npcRota1;
    [SerializeField] public List<Pokemon> pokeInicial;
    [SerializeField] GameObject Arbursto1;
    
    [SerializeField] GameObject[] Pokebolas;

    [SerializeField] GameObject MenuConfirmacao;
    [SerializeField] bool confirmacao = false;
    [SerializeField] int escolhido = -1;


//
    public bool centroPokemon = false;

   
    public void Start()
    {
        pokemonParty = player.GetComponent<PokemonParty>();
    }

    public void Update()
    {
        if (confirmacao == true)
        {
            switch (escolhido)
            {
                //squirtle
                case 0:
                    Squirtle();
                    confirmacao = false;
                    break;

                //bulba
                case 1:
                    Bulbassauro();
                    confirmacao = false;
                    break;

                //charmand
                case 2:
                    Charmannder();
                    confirmacao = false;
                    break;

            }
        }

    }

    public void Squirtle()
    {
        if (confirmacao)
        {
            Time.timeScale = 1f;
            pokemonParty.pokemons.Remove(pokemonParty.pokemons[0]);
            pokemonParty.pokemons.Add(pokeInicial[0]);
            escolhaPokemon.SetActive(false);

            //Iniciando POKEMON
            pokemonParty.pokemons[0].Init();

            //DESTRUINDO NPC
            Destroy(npcRota1);

            //DESTRUINDO ARBUSTOS
            Destroy(Arbursto1);


            //DESTRUINDO POKEBOLAS
            Destroy(Pokebolas[0]);

            //Dizendo que meu jogador pode comecar suas batalhas!
            sistemaDeBatalha.PlayerCanBattle = true;

            //Destruindo Canvas
            Destroy(escolhaPokemon);
        }
        else
        { escolhido = 0; }
        
    }

    public void Bulbassauro()
    {
        if (confirmacao)
        {
            Time.timeScale = 1f;
            pokemonParty.pokemons.Remove(pokemonParty.pokemons[0]);
            pokemonParty.pokemons.Add(pokeInicial[1]);


            //Iniciando POKEMON
            pokemonParty.pokemons[0].Init();

            //DESTRUINDO NPC
            Destroy(npcRota1);

            //DESTRUINDO ARBUSTOS
            Destroy(Arbursto1);

            //DESTRUINDO POKEBOLAS
            Destroy(Pokebolas[1]);

            //Dizendo que meu jogador pode comecar suas batalhas!
            sistemaDeBatalha.PlayerCanBattle = true;


            //Destruindo Canvas
            Destroy(escolhaPokemon);
        }
        else { escolhido = 1; }
        

    }

    public void Charmannder()
    {
        if (confirmacao)
        {
            Time.timeScale = 1f;
            pokemonParty.pokemons.Remove(pokemonParty.pokemons[0]);
            pokemonParty.pokemons.Add(pokeInicial[2]);

            //Iniciando POKEMON
            pokemonParty.pokemons[0].Init();

            //DESTRUINDO NPC
            Destroy(npcRota1);

            //DESTRUINDO ARBUSTOS
            Destroy(Arbursto1);

            //DESTRUINDO POKEBOLAS
            Destroy(Pokebolas[2]);

            //Dizendo que meu jogador pode comecar suas batalhas!
            sistemaDeBatalha.PlayerCanBattle = true;

            //Destruindo Canvas
            Destroy(escolhaPokemon);
        }
        else { escolhido = 2; }
        

    }

    public void RevelarMenuConfirmacao()
    {
        MenuConfirmacao.SetActive(true);
    }
    public void ApagarMenuConfirmacao()
    {
        MenuConfirmacao.SetActive(false);
    }
    public void Confirmar()
    {
        confirmacao = true;   
    }


}
