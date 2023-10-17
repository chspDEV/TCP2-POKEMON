using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MesaCarvalho : MonoBehaviour
{

    [SerializeField] GameObject questController;   
    [SerializeField] GameObject escolhaPokemon;

    bool desligarColisao = false;

    public void Awake()
    {
        //Comecando desativado
        escolhaPokemon.SetActive(false);
    }

    private void OnTriggerStay(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space) && desligarColisao == false)
        { 
            escolhaPokemon.SetActive(true);
            Time.timeScale = 0f;
            desligarColisao = true; 
        }
    }
}
