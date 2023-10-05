using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;  // controle do jogador 
    [SerializeField] SistemaDeBatalha sistemaDeBatalha;
    [SerializeField] Camera cam;  // CAMERA DO JOGADOR PADRAO!!
    [SerializeField] GameObject mesaCarvalho;
    [SerializeField] GameObject escolhaPokemon;
    [SerializeField] GameObject player;

    [SerializeField] PokemonParty pokemonParty;


    public void Start()
    {
        pokemonParty = player.getComponent<PokemonParty>();
    }
    public void Squirtle()
    {
        Time.timeScale = 1f;
        escolhaPokemon.SetActive(false);
        
    }

    public void Bulbassauro()
    {
        Time.timeScale = 1f;
        escolhaPokemon.SetActive(false);
    }

    public void Charmannder()
    {
        Time.timeScale = 1f;
        escolhaPokemon.SetActive(false);
    }
}
