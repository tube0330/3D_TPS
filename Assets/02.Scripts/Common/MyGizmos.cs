using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type
    {
        NORMAL, WAYPOINT
    }
    private const string WAYPOINTFILE = "Enemy";
    public Type type = Type.NORMAL;
    public Color _color;
    public float _r;

    void Start()
    {
        //_color = Color.red;
        //_r = 0.1f;
    }

    private void OnDrawGizmos()
    {
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _r);
        }

        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up*1.0f, WAYPOINTFILE, true);  //위치, 파일명, 스케일 적용여부
            
            Gizmos.DrawWireSphere(transform.position, _r);
        }
    }
}
