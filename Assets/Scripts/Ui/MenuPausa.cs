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
    public GameObject Mochila;

    [SerializeField] PlayerController playerCon;

    
    private bool pokedexAberta = false;
    private bool menuAberto = false;
    private bool controlesAberto = false;
    private bool mochilaAberta = false;


    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))

        {
          
            if (!menuAberto)
            {
               
                AbrirMenu();
                FecharPokedex();
                FecharControles();
                FecharMochila();
            }
            else
            {
                
                FecharMenu();
                FecharPokedex();
                FecharControles();
                FecharMochila();
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

    public void AbrirMochila()
    {
        Mochila.SetActive(true);
        mochilaAberta = true;
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

    public void FecharMochila()
    {
        Mochila.SetActive(false);
        mochilaAberta = false;
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
        playerCon.moveEnabled = false;
    }

    public void FecharMenu()
    {
        Time.timeScale = 1f; 
        Menu.SetActive(false);
        menuAberto = false;
        playerCon.moveEnabled = true;
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
