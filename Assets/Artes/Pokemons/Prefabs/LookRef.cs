using UnityEngine;

public class LookRef : MonoBehaviour
{
    private bool colidiu = false;

    [Header("Ajuste para a posicao do Pokemon")]
    [SerializeField] Vector3 save_pos;

    // A velocidade de rotação no eixo Y
    public float velocidadeRotacao = 50.0f;

    void Start()
    {

    }

    void Update()
    {
        if (colidiu == false)
        {
            // Girar apenas no eixo Y
            transform.Rotate(save_pos.x, velocidadeRotacao * Time.deltaTime, save_pos.z);
        }
        else
        {
            Debug.Log("Estou falso!");
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Pokemon"))
        {
            // Parar de girar quando colidir com um objeto com a tag "Pokemon"
            colidiu = true;
            Debug.Log("Colidi com Pokemon inimigo!!");
        }
    }
}
