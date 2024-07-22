using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Rigidbody rb;
    AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] GameObject Effect;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    /* private void OnTriggerEnter(Collider col)
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
    } */

    void OnDamage(object[] _params)
    {
        Vector3 hitPos = (Vector3) _params[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitPos.normalized);
        GameObject blood = Instantiate(Effect, hitPos, rot);
        Destroy(blood, 1.0f);
        SoundManager.S_instance.PlaySound(hitPos, clip);
    }
}
