using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    //criei esse script pra conseguir interagir com a mesa do carvalho
    [SerializeField] private Transform _pontoInteracao;
    [SerializeField] private float _pontoInteracaoRadius = 0.5f;
    [SerializeField] private LayerMask _camadaInteragivel;

    //numero de objeots pra colidir
    private readonly Collider[] _colisores = new Collider[3];
    [SerializeField] private int _numEncontrado;


    private void Update()
    { 
        _numEncontrado = Physics.OverlapSphereNonAlloc(_pontoInteracao.position, _pontoInteracaoRadius,_colisores, _camadaInteragivel);
    }
}
