using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemonSelvagens;

    public Pokemon GetRandomWildPokemon()
    {
            var pokemonSelvagem = pokemonSelvagens[Random.Range(0, pokemonSelvagens.Count)];
            pokemonSelvagem.Init();
            return pokemonSelvagem;
    }
}
