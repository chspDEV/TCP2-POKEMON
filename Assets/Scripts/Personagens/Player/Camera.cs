using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float velociade = 5.0f;
    public Transform target;
    public Vector3 offset = Vector3.up;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, velociade * Time.deltaTime) ;  
        
       
    }
}
