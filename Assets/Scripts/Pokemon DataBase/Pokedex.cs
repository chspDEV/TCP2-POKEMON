using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pokedex : MonoBehaviour
{
    public PokemonBase[] pokemon;
    public TextMeshProUGUI nameText;
    public GameObject BoxPokemon; 
    public Transform caixaTextoPanel; 

    private void Start()
    {   
        NomearPokemons();
    }

    private void NomearPokemons()
    {
        for (int i = 0; i < pokemon.Length; i++)
        {
           GameObject nameBox = Instantiate(BoxPokemon, caixaTextoPanel);
           TextMeshProUGUI nameText = nameBox.GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = "001" + pokemon[i].Nome;

           
            //if (i < caixaTextoPosition.Length)
            //{
            //    nameBox.transform.localPosition = caixaTextoPosition[i];
            //}
        }
    }
}
