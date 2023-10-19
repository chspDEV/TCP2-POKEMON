using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PokeHeal : MonoBehaviour
{

    [SerializeField] SistemaDeBatalha sistema;
    [SerializeField] PokemonParty playerParty;


    private void OnTriggerStay(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space) && sistema.PlayerCanBattle == false)
        {
            Time.timeScale = 0f;
            playerParty = colisao.GetComponent<PokemonParty>();

            foreach (Pokemon p in playerParty.pokemons)
            {
                p.HP = p.VidaMax;
            }

            sistema.PlayerCanBattle = true;
        }
    }
}
