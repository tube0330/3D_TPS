using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform mainCamTr;
    public Transform tr;
    void Start()
    {
        tr = transform;
        mainCamTr = Camera.main.transform;
    }

    void Update()
    {
        tr.LookAt(mainCamTr);
    }
}
