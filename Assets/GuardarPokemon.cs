using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarPokemon : MonoBehaviour
{

    [SerializeField] SistemaDeBatalha sistema;
    [SerializeField] PokemonParty playerParty;
    [SerializeField] GameObject PcUI;

    private void OnTriggerStay(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            PcUI.SetActive(true);
            if( Input.GetKeyDown(KeyCode.Escape) ) 
            {
                PcUI.SetActive(false);
            }
        }
    }
}
