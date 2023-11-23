using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BillboardType { LookAtCamera, CameraForward }

public class Billboard : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;
    [SerializeField] private GameController gm;

    private void Awake()
    {
        gm = GameObject.Find("GameController").GetComponent<GameController>();
    }
    private void LateUpdate()
    {
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(gm.cam.transform.position, Vector3.up);
                break;
            case BillboardType.CameraForward:
                transform.forward = gm.cam.transform.forward;
                break;
            default:
                break;
        }
    }
}
