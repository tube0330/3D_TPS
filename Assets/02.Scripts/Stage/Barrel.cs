using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] string bulletTag = "BULLET";
    [SerializeField] string E_bulletTag = "E_BULLET";

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(bulletTag))
        {
            SoundManager.S_instance.PlaySound(transform.position, SoundManager.S_instance.clip1);
            col.gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag(E_bulletTag))
            col.gameObject.SetActive(false);
    }
}
