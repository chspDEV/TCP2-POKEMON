using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRef : MonoBehaviour
{
    GameObject reference;
    //[Range(0.01f, 10f)]
    [SerializeField] float velocidadeRotacao = 1f; // Velocidade de rotação



    // Start is called before the first frame update
    void Start()
    {
        reference = GameObject.Find("Ref_poke");
    }

    void Update()
    {
        if (reference != null)
        {
            Vector3 direcao = transform.position - reference.transform.position;

            Quaternion rotacaoDesejada = Quaternion.LookRotation(direcao);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoDesejada, Time.deltaTime * velocidadeRotacao);
        }
    }
}
