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


    private void OnCollisionStay(Collision colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space) && desligarColisao == false)
        {
            Time.timeScale = 0f;
            escolhaPokemon.SetActive(true);
            desligarColisao = true; 
        }
    }
}
