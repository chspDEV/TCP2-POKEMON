using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{

    GameObject player;
    bool playerInZone;

    [Header("Posição de destino do teletransporte")]
    [Space(15)]
    [SerializeField] Vector3 tpPoint;

    [SerializeField] GameObject TransicaoGO;
    [SerializeField] byte alpha;

    

    void Start()
    {
        playerInZone = false;
        player = GameObject.Find("Player");
        TransicaoGO = GameObject.FindWithTag("Transicao");
        

    }

    void Update()
    {
        if (playerInZone)
        {
            player.transform.position = tpPoint;
            StartCoroutine(Transicao());
        }

    }

    public IEnumerator Transicao()
    {
        TransicaoGO.SetActive(true);
        var _imagem = TransicaoGO.GetComponent<Image>();
        _imagem.color = new Color32(0, 0, 0, alpha);

        if (alpha <= 0)
        {
            TransicaoGO.SetActive(false);
            yield break;
        }
        else { alpha -= 1; _imagem.color = new Color32(0, 0, 0, alpha); }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.name == "Player")
        {
            playerInZone = false;
        }
    }
}
