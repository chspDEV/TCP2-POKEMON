using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Authentication.ExtendedProtection;

public class QuestController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;  // controle do jogador 
    [SerializeField] SistemaDeBatalha sistemaDeBatalha;
    [SerializeField] Camera cam;  // CAMERA DO JOGADOR PADRAO!!
    [SerializeField] GameObject mesaCarvalho;
    [SerializeField] GameObject escolhaPokemon;
    [SerializeField] GameObject player;
    [SerializeField] PokemonParty pokemonParty;


    [SerializeField] public List<Pokemon> pokeInicial;

    bool npcRota1 = true;

    public void Start()
    {
        pokemonParty = player.GetComponent<PokemonParty>();
    }

    public void Squirtle()
    {
        Time.timeScale = 1f;
        pokemonParty.pokemons.Insert(0, pokeInicial[0]);
        escolhaPokemon.SetActive(false);
        npcRota1 = false;
    }

    public void Bulbassauro()
    {
        Time.timeScale = 1f;
        pokemonParty.pokemons.Insert(1, pokeInicial[1]);
        escolhaPokemon.SetActive(false);
        npcRota1 = false;
    }

    public void Charmannder()
    {
        Time.timeScale = 1f;
        pokemonParty.pokemons.Insert(2, pokeInicial[2]);
        escolhaPokemon.SetActive(false);
        npcRota1 = false;
    }
}
