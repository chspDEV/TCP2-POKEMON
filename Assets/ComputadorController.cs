using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComputadorController : MonoBehaviour
{
    [SerializeField] GameObject pc;

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
        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            AbrirPC();
        }

        if (colisao.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Escape))
        {
            FecharPC();
        }
    }
}
