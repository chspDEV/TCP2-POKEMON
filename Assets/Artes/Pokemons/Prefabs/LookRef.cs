using UnityEngine;

public class LookRef : MonoBehaviour
{

    [Header("Variavel para saber se ainda gira")]
    [Space(10)]
    [SerializeField] private bool colidiu = false;

    [Header("Ajuste para a posicao do Pokemon")]
    [Space(10)]
    [SerializeField] Vector3 save_pos;

    public bool IsPlayer = false;



    // A velocidade de rotação no eixo Y
    public float velocidadeRotacao = 50.0f;

    void Awake()
    {
        //save_pos.x = this.transform.rotation.x;
        //save_pos.z = this.transform.rotation.z;
    }

    void Update()
    {
        if (IsPlayer)
        {
            this.colidiu = true;
        }
        else
        if (!IsPlayer)
        {
            if (this.colidiu == false)
            {
                // Girar apenas no eixo Y
                transform.Rotate(save_pos.x, velocidadeRotacao * Time.deltaTime, save_pos.z);
            }
            else
            {
                Debug.Log("Estou falso!");
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(!IsPlayer)
        {
            if (collision.gameObject.CompareTag("Pokemon"))
            {
                // Parar de girar quando colidir com um objeto com a tag "Pokemon"
                this.colidiu = true;

                Debug.Log("Colidi com Pokemon inimigo!!");
            }

        }

    }
    
}
