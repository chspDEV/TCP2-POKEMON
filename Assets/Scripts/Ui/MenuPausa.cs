using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject Menu;
    //public GameObject Pokedex;
    public GameObject Controles;
    public GameObject Mochila;

    [SerializeField] PlayerController playerCon;
    [SerializeField] SistemaDeBatalha sistema;

    [SerializeField] private bool menuAberto = false;



    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            if (!menuAberto)
            {
                Debug.Log("Menu aberto");
                
                //FecharPokedex();
                FecharControles();
                FecharMochila();
                AbrirMenu();
            }
            else
            {
                Debug.Log("Menu fechado");
                
                //FecharPokedex();
                FecharControles();
                FecharMochila();
                FecharMenu();
            }
            
        }
    }

    public void AbrirPokedex()
    { 
        //Pokedex.SetActive(true);
        Menu.SetActive(false);
        menuAberto = false;
    }

    public void AbrirMochila()
    {
        Mochila.SetActive(true);
        var _m = Mochila.GetComponent<Mochila>();
        var _p = Mochila.GetComponent<PcInfoChanger>();

        for (int i = 0; i < _p.slotsParty.Length; i++)
        {
            _p.UpdateSlotParty(i);
        }



        Menu.SetActive(false);
        menuAberto = false;   
    }

    public void AbrirControles()
    {
        Controles.SetActive(true);
        Menu.SetActive(false);
        menuAberto = false;
    }

    public void FecharMochila()
    {
       Mochila.SetActive(false);
       Menu.SetActive(true);
       menuAberto = true;
    }

    public void FecharControles()
    {
        Controles.SetActive(false);
        menuAberto = true;
    }

    public void FecharPokedex()
    {
        //Pokedex.SetActive(false);
        menuAberto = true;
    }


    public void AbrirMenu()
    {
        Time.timeScale = 0f; 
        Menu.SetActive(true);
        menuAberto = true;
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
