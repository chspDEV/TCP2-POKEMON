using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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

    public void Ativar()
    {
        escolhaPokemon.SetActive(true);
        Time.timeScale = 0f;
        desligarColisao = true;
        Destroy(pressSpace);
    }

    //void DestroyE()
    //{
    //    if (desligarE == true)
    //    {
    //        Destroy(pressSpace);
    //    }
    //}
    private void OnTriggerEnter(Collider colisao)
    {
        
            if (colisao.gameObject.CompareTag("Player") && pressSpace != null)
            {
                pressSpace.SetActive(true);
                colisao.GetComponent<PlayerController>().CanInteract = true;
                colisao.GetComponent<PlayerController>().InteractOBJ = gameObject;
        }
        
        
    }

    private void OnTriggerExit(Collider colisao)
    {
        
            if (colisao.gameObject.CompareTag("Player") && pressSpace != null)
            {
                pressSpace.SetActive(false);
                colisao.GetComponent<PlayerController>().CanInteract = false;
                colisao.GetComponent<PlayerController>().InteractOBJ = null;
        }
       
        
    }
}
