using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Caminhos : MonoBehaviour
{
    public float velCam;
    CinemachineDollyCart cart;

    public Transform[] targets; // Adicione seus alvos aqui
    public CinemachineVirtualCamera virtualCamera;

    private int currentTargetIndex = 0;
    private int currentPathIndex = 0; // Variável para controlar o índice do caminho
    private bool shouldRestartSwitchTargets = true; // Variável para controlar o reinício do foco em alvos
    public CinemachineSmoothPath startPath;
    public CinemachineSmoothPath[] alternatePaths;

    private void Awake()
    {
        cart = GetComponent<CinemachineDollyCart>();

        Resetar();
    }

    private void Start()
    {
        StartCoroutine(SwitchTargets());
    }

    public void Resetar()
    {
        StopAllCoroutines();

        cart.m_Path = startPath;

        StartCoroutine(ChangeTrack());
    }

    IEnumerator ChangeTrack()
    {
        yield return new WaitForSeconds(Random.Range(4, 6));

        currentPathIndex = (currentPathIndex + 1) % alternatePaths.Length; 

        var path = alternatePaths[currentPathIndex];

        cart.m_Path = path;

        shouldRestartSwitchTargets = true; // Ativa o reinício do foco em alvos quando o caminho é trocado
        //StartCoroutine(SwitchTargets()); // Reinicia o foco em alvos quando o caminho é trocado
    }

    public void SetPathAndTarget(CinemachineSmoothPath newPath, Transform newTarget)
    {
        cart.m_Path = newPath;
        virtualCamera.LookAt = newTarget;
        shouldRestartSwitchTargets = false; // Desativa o reinício do foco em alvos temporariamente
    }

    IEnumerator SwitchTargets()
    {
        while (true)
        {
            Transform currentTarget = targets[currentTargetIndex];

            virtualCamera.LookAt = currentTarget;

            yield return new WaitForSeconds(velCam);

            // Verifica se deve reiniciar o foco em alvos após a corrotina
            if (shouldRestartSwitchTargets)
            {
                currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
            }
        }
    }
}