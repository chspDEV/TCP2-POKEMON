using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MesaCarvalho : MonoBehaviour
{

    [SerializeField] GameObject questController;   
    [SerializeField] GameObject escolhaPokemon;
    [SerializeField] GameObject pressSpace;

    bool desligarColisao = false;

    public void Awake()
    {
        //Comecando desativado
        escolhaPokemon.SetActive(false);
    }

    private void OnTriggerStay(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && desligarColisao == false)
        { 
            escolhaPokemon.SetActive(true);
            Time.timeScale = 0f;
            desligarColisao = true; 
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
