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

    // Chamado para mudar a câmera para o plano aberto
    public void SwitchToTopCamera()
    {
        
            
            cinemachineBrain.ActiveVirtualCamera.Priority = topCamera.Priority;

        
        
    }
    // Chamado para mudar a câmera para o plano médio (jogador)
    public void SwitchToMiddleCamera()
    {
        cinemachineBrain.ActiveVirtualCamera.Priority = middleCamera.Priority;
    }

    // Chamado para mudar a câmera para o plano médio (inimigo)
    public void SwitchToBottomCamera()
    {
        cinemachineBrain.ActiveVirtualCamera.Priority = bottomCamera.Priority;
    }
}
