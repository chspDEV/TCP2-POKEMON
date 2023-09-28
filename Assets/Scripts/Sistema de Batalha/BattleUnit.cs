using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
   
    [SerializeField] GameObject posicaoDeInstancia;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] HudBatalha hud;
    [SerializeField] private GameObject instantiatedModel;

    public bool IsPlayerUnit {get { return isPlayerUnit; } }
    public HudBatalha Hud { get { return hud; } }

   

    public Pokemon Pokemon { get; set; }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayerUnit)
        {
            instantiatedModel = Instantiate(pokemon.Base.Modelo3D, posicaoDeInstancia.transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 posicaoInstancia = posicaoDeInstancia.transform.position;
            posicaoInstancia.z += 13f; // Adiciona 15 ao eixo Z
            instantiatedModel = Instantiate(pokemon.Base.Modelo3D, posicaoInstancia, Quaternion.identity);
        }

        hud.SetData(pokemon);
        //Anima��o do pokemon entrando vem aqui
    }

    // M�todo para destruir o objeto instanciado
    public void DestroyInstantiatedModel()
    {

        if (instantiatedModel)
        {
            Destroy(instantiatedModel);
        }
        
    }
}
