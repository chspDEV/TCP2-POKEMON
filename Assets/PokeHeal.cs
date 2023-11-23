using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PokeHeal : MonoBehaviour
{

    [SerializeField] SistemaDeBatalha sistema;
    [SerializeField] PokemonParty playerParty;
    [SerializeField] GameObject pressSpace;


    private void OnTriggerStay(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
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

    private void OnTriggerEnter(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            pressSpace.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            pressSpace.SetActive(false);
        }
    }
}
