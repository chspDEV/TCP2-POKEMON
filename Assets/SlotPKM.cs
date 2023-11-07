using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotPKM : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject[] slot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AtualizarSlot();
    }

    public void AtualizarSlot()
    {
        PokemonParty _pt = player.GetComponent<PokemonParty>();

        for (int i = 0; i < _pt.pokemons.Count; i++)
        {
            if (_pt.pokemons[i].sprite != null && _pt.pokemons[i].nome != "PlaceHolder")
            {
                slot[i].GetComponentInChildren<Image>().sprite = _pt.pokemons[i].sprite;
            }
            else { slot[i].GetComponentInChildren<Image>().sprite = null; }

            if (_pt.pokemons[i].nome != null && _pt.pokemons[i].nome != "PlaceHolder")
            {
                slot[i].GetComponentInChildren<TextMeshProUGUI>().text = _pt.pokemons[i].nome + " Lv " + _pt.pokemons[i].level.ToString("n0");

                //slot[i].GetComponentInParent<TextMeshProUGUI>().text = _pt.pokemons[i].HP.ToString("n0") + " / " + _pt.pokemons[i].VidaMax.ToString("n0");

            }
            else { slot[i].GetComponentInChildren<TextMeshProUGUI>().text = ""; }
                
        }

    }
}
