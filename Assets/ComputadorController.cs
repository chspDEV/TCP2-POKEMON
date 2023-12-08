using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComputadorController : MonoBehaviour
{
    [SerializeField] GameObject pc;
    [SerializeField] GameObject pressSpace;

    public void AbrirPC()
    {
        pc.SetActive(true);
    }
    public void FecharPC()
    {
        pc.SetActive(false);
    }

    private void OnTriggerEnter(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            pressSpace.SetActive(true);
            colisao.GetComponent<PlayerController>().InteractOBJ = gameObject;
            colisao.GetComponent<PlayerController>().CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            pressSpace.SetActive(false);
            colisao.GetComponent<PlayerController>().InteractOBJ = null;
            colisao.GetComponent<PlayerController>().CanInteract = false;
        }
    }
}
