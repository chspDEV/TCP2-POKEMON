using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] public List<Pokemon> pokemons;

    public List<Pokemon> Pokemons { get { return pokemons;  } }
    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }

    }

    public Pokemon GetHealthyPokemon()
    {
        //return pokemons.Where(x => x.HP > 0).FirstOrDefault();

        for (int i = 0; i < pokemons.Count; i++)
        {
            if (pokemons[i].HP > 0)
            {
                Debug.Log(pokemons[i].nome);
                return pokemons[i];
            }
        }

        return null;
    }
}
