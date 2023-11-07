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
            //era iss aqui que tava travando o jogo, n entendi
            //pq q vc colocou aqui ent eu só comentei
            //Time.timeScale = 0f;
            playerParty = colisao.GetComponent<PokemonParty>();
            Debug.Log("CURANDO!");
            foreach (Pokemon p in playerParty.pokemons)
            {
                p.HP = p.VidaMax;
            }

            sistema.PlayerCanBattle = true;
        }
    }
}
