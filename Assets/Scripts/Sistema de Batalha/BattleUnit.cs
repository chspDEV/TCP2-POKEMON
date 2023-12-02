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
    //public Pokemon pokeatual;
    public int HP;

    private void Update()
    {
        //HP = Pokemon.HP;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        pokemon.Init();

        if (isPlayerUnit)
        {
            instantiatedModel = Instantiate(pokemon.Base.Modelo3D, posicaoDeInstancia.transform.position, pokemon.Base.Modelo3D.transform.rotation);

            var  _pPoke = instantiatedModel.GetComponent<LookRef>();
            _pPoke.IsPlayer = true;
        }
        else
        {
            Vector3 posicaoInstancia = posicaoDeInstancia.transform.position;
            posicaoInstancia.z += 13f; // Adiciona 15 ao eixo Z
            instantiatedModel = Instantiate(pokemon.Base.Modelo3D, posicaoInstancia, pokemon.Base.Modelo3D.transform.rotation);
        }

        hud.SetData(pokemon);
        //Animação do pokemon entrando vem aqui
    }

    // Método para destruir o objeto instanciado
    public void DestroyInstantiatedModel()
    {

        if (instantiatedModel)
        {
            Destroy(instantiatedModel);
        }
        
    }
}
