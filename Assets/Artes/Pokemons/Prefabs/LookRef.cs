using UnityEngine;

public class LookRef : MonoBehaviour
{

    [Header("Ajuste para a posicao do Pokemon")]
    [Space(10)]
    [SerializeField] Vector3 save_pos;

    public bool IsPlayer = false;


    public void Start()
    {
        if (IsPlayer == false)
        {
            Quaternion quaternion = Quaternion.Euler(save_pos.x, save_pos.y, save_pos.z);
            transform.rotation = quaternion;
        }

    }
    
}
