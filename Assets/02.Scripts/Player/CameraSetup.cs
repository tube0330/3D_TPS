using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    CinemachineVirtualCamera cam;

    void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();    
    }

    void Update()
    {
        cam.LookAt = transform;
        cam.Follow = transform;
    }
}
