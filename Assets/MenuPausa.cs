using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject Menu;

    private bool menuAberto = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuAberto)
            {
                AbrirMenu();
            }
            else
            {
                FecharMenu();
            }
        }
    }

    public void AbrirMenu()
    {
        Time.timeScale = 0f; 
        Menu.SetActive(true);
        menuAberto = true;
        Debug.Log("Menu aberto");
    }

    public void FecharMenu()
    {
        Time.timeScale = 1f; 
        Menu.SetActive(false);
        menuAberto = false;
        Debug.Log("Menu fechado");
    }

    public void VoltaMenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }

    public void VoltarJogo()
    {
        FecharMenu(); 
    }
}
