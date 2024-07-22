using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] Rigidbody rb;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float speed = 1000f;    //1500f;

    void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        Invoke("BulletDisable", 2.0f);
        rb.AddForce(tr.forward * speed);
    }

    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
