using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] Transform firePos;
    private Transform tr;
    private LineRenderer line;

    void Start()
    {
        tr = transform;
        firePos = transform.GetComponentsInParent<Transform>()[1];
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.enabled = false;
    }

    void Update()
    {
        //광선을 미리 생성
        Ray ray = new Ray(firePos.position, tr.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            //Line Renderer의 첫번째 점의 위치 설정
            line.SetPosition(0, tr.InverseTransformPoint/*월드좌표 위치를 로컬좌표 위치로 변경*/(ray.origin));

            //어떤 물체에 광선이 닿았을 때의 위치를 Line Renderer의 끝점으로 설정
            if (Physics.Raycast(ray, out hit, 100f))
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));

            //안 닿았을 때 끝점을 100으로 설정
            else
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));
            
            StartCoroutine(ShowLaserBeam());
        }
    }

    IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        line.enabled = false;
    }
}
