using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Rigidbody rb;
    AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] GameObject Effect;

    [SerializeField] string bulletTag = "BULLET";
    [SerializeField] string E_bulletTag = "E_BULLET";
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(bulletTag))
        {
            source.PlayOneShot(clip, 1f);
            GameObject effect = Instantiate(Effect, col.transform.position, Quaternion.identity);
            col.gameObject.SetActive(false);

            Destroy(effect, 2f);
        }

        if (col.gameObject.CompareTag(E_bulletTag))
            col.gameObject.SetActive(false);
    }
}
