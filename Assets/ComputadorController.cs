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

    private void OnTriggerStay(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            AbrirPC();
        }
    }

    private void OnTriggerEnter(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            pressSpace.SetActive(true);
        }

        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            AbrirPC();
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
