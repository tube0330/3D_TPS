using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15f;     //적 캐릭터의 추적 사정 거리

    [Range(0, 360)] public float viewAngle = 120f;    //적 캐릭터의 시야각
    void Start()
    {

    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;    //로컬좌표계 기준으로 설정하기 위해서 적캐릭터의 Y 회전값을 더함

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad/*변환상수로 (PI * 2)/360과 같음*/));   //일반 각도를 Radian으로 변환
    }
}
