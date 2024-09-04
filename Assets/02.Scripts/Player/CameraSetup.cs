using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class CameraSetup : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
            cam.Follow = transform;
            cam.LookAt = transform;
        }
    }
}
