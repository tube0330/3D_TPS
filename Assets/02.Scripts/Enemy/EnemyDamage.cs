using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private readonly string bulletTag = "BULLET";
    void Start()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag(bulletTag))
            col.gameObject.SetActive(false);
    }
}
