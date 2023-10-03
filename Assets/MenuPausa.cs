using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Pokedex;
    public GameObject Controles;


    private bool pokedexAberta = false;
    private bool menuAberto = false;
    private bool controlesAberto = false;


    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            if (!menuAberto)
            {
                AbrirMenu();
                FecharPokedex();
                FecharControles();
            }
            else
            {
                FecharMenu();
                FecharPokedex();
                FecharControles();
            }
        }
    }

    public void AbrirPokedex()
    { 
        Pokedex.SetActive(true);
        pokedexAberta = true;
        Menu.SetActive(false);
        menuAberto = false;
    }

    public void AbrirControles()
    {
        Controles.SetActive(true);
        controlesAberto = true;
        Menu.SetActive(false);
        menuAberto = false;
    }

    public void FecharControles()
    {
        Controles.SetActive(false);
        controlesAberto = false;
    }

    public void FecharPokedex()
    {
        Pokedex.SetActive(false);
        pokedexAberta = false;
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
