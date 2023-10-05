using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Caminhos : MonoBehaviour
{
    public float velCam;
    CinemachineDollyCart cart;

    public Transform[] targets;
    public CinemachineVirtualCamera virtualCamera;

    private int currentTargetIndex = 0;
    [SerializeField] int currentPathIndex = -1;
    private bool shouldRestartSwitchTargets = true;
    public CinemachineSmoothPath startPath;
    public CinemachineSmoothPath[] alternatePaths;

    private Coroutine changeTrackCoroutine;

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
        StopChangeTrackCoroutine(); 
        currentPathIndex = -1;
        cart.m_Path = startPath;
        StartChangeTrackCoroutine(); 
    }

    public void ResetarCaminho()
    {
        StopChangeTrackCoroutine(); 
        currentPathIndex = -1;
        cart.m_Path = startPath;
        StartChangeTrackCoroutine(); 
    }

    private void StartChangeTrackCoroutine()
    {
        if (changeTrackCoroutine != null)
        {
            StopCoroutine(changeTrackCoroutine);
        }
        changeTrackCoroutine = StartCoroutine(ChangeTrack());
    }

    private void StopChangeTrackCoroutine()
    {
        if (changeTrackCoroutine != null)
        {
            StopCoroutine(changeTrackCoroutine);
            changeTrackCoroutine = null;
        }
    }

    IEnumerator ChangeTrack()
    {
        yield return new WaitForSeconds(3f);

        currentPathIndex = (currentPathIndex + 1) % alternatePaths.Length;

        if (currentPathIndex <= 3)
        {
            var path = alternatePaths[currentPathIndex];
            cart.m_Path = path;
            shouldRestartSwitchTargets = true;
        }
        else
        {
            ResetarCaminho();
            yield break;
        }

        Debug.Log(currentPathIndex);
        StartChangeTrackCoroutine(); 
    }

    public void SetPathAndTarget(CinemachineSmoothPath newPath, Transform newTarget)
    {
        cart.m_Path = newPath;
        virtualCamera.LookAt = newTarget;
        shouldRestartSwitchTargets = false;
    }

    IEnumerator SwitchTargets()
    {
        while (true)
        {
            Transform currentTarget = targets[currentTargetIndex];
            virtualCamera.LookAt = currentTarget;
            yield return new WaitForSeconds(velCam);

            if (shouldRestartSwitchTargets)
            {
                currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
            }
        }
    }
}
