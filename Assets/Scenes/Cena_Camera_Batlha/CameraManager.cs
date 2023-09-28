using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    public CinemachineFreeLook topCamera;
    public CinemachineFreeLook middleCamera;
    public CinemachineFreeLook bottomCamera;

    

    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    // Chamado para mudar a c�mera para o plano aberto
    public void SwitchToTopCamera()
    {
        
            
            cinemachineBrain.ActiveVirtualCamera.Priority = topCamera.Priority;

        
        
    }
    // Chamado para mudar a c�mera para o plano m�dio (jogador)
    public void SwitchToMiddleCamera()
    {
        cinemachineBrain.ActiveVirtualCamera.Priority = middleCamera.Priority;
    }

    // Chamado para mudar a c�mera para o plano m�dio (inimigo)
    public void SwitchToBottomCamera()
    {
        cinemachineBrain.ActiveVirtualCamera.Priority = bottomCamera.Priority;
    }
}
