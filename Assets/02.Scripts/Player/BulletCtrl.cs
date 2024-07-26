using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] Rigidbody rb;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float speed = 1000f;    //1500f;

    public float damage = 25;

    void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        Invoke("BulletDisable", 2.0f);
    }

    private void OnEnable()
    {
        damage = GameManager.G_Instance.gameData.damage;
        GameManager.OnItemChange += UpdateSetUp;
        
        rb.AddForce(tr.forward * speed);
    }

    void UpdateSetUp()
    {
        damage = GameManager.G_Instance.gameData.damage;
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
