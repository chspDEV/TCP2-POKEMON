using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Moedas : MonoBehaviour
{
    public GameController gm;
    public TextMeshProUGUI texto;

    // Update is called once per frame
    void Update()
    {
        texto.text = $"Moedas: {gm.moedas}";
    }
}
