using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendedora : MonoBehaviour
{
    public Pokemart pokemart;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().CanInteract = true;
            other.GetComponent<PlayerController>().InteractOBJ = gameObject;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            other.GetComponent<PlayerController>().CanInteract = false;
            other.GetComponent<PlayerController>().InteractOBJ = null;
        }

    }
}
