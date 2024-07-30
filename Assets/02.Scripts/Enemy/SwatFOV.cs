using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatFOV : MonoBehaviour
{
    [Range(0, 360)] public float viewAngle = 120f;
    [SerializeField] Transform tr;
    [SerializeField] Transform playertr;
    public float viewRange = 15;

    [Header("Layer")]
    [SerializeField] int playerLayer;
    [SerializeField] int barrelLayer;
    [SerializeField] int boxLayer;
    [SerializeField] int layerMask;

    void Start()
    {
        tr = transform;
        playertr = GameObject.Find("Player").GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer("Player");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        boxLayer = LayerMask.NameToLayer("BOXES");
        layerMask = 1<<playerLayer | 1<< barrelLayer | 1<<boxLayer;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;

        Collider[] cols = Physics.OverlapSphere(tr.position, viewRange, 1 << playerLayer);

        if (cols.Length == 1)
        {
            Vector3 dir = (playertr.position - tr.position).normalized;

            if (Vector3.Angle(tr.forward, dir) < viewAngle * 0.5f)
                isTrace = true;
        }

        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        Vector3 dir = (playertr.position - tr.position).normalized;

        if (Physics.Raycast(tr.position, dir, out hit, viewRange, layerMask))    //Raycast로 장애물이 있는지 판단
        {
            isView = hit.collider.CompareTag("Player");
        }

        return isView;
    }
}
