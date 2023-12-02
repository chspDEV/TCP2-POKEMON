using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ChangeScene();
        }
    }

   


    private void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
