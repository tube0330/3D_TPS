using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private readonly string E_bulletTag = "E_BULLET";

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(E_bulletTag))
        {
            col.gameObject.SetActive(false);
        }

    }
}
