using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpCentro : MonoBehaviour
{
    [SerializeField] QuestController quest;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            quest.centroPokemon = true;
        }
    }
}
