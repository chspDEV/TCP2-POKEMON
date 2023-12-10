using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
   
    [SerializeField] GameObject posicaoDeInstancia;
    [SerializeField] SistemaDeBatalha s;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] HudBatalha hud;
    [SerializeField] private GameObject instantiatedModel;
    [SerializeField] private GameObject myModelTrainer;

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

            if (s.treinador_atual != null)

            {
                var _ref = GameObject.Find("Ref_treinador");
                var _model = s.treinador_atual.GetComponent<TrainerController>().modelo3D;
                myModelTrainer = Instantiate(_model, _ref.transform.position, _model.transform.rotation);

            }

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

        if (myModelTrainer)
        {
            Destroy(myModelTrainer);
        }

    }
}
