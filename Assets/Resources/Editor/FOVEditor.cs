using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]

public class FOVEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;    //EnemyFOV 클래스를 참조하겠다.

        /* 원주 위의 시작점의 좌표를 계산(주어진 각도 1/2) */

        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        Handles.color = Color.white;    //원의 색상 지정
        Handles.DrawWireDisc(fov.transform.position,    //원점좌표
                            Vector3.up,                 //노멀 벡터
                            fov.viewRange);             //원 반지름

        Handles.color = new Color(1, 1, 1, 0.2f);   //부채꼴 색상 지정
        Handles.DrawSolidArc(fov.transform.position,    //원점좌표
                                Vector3.up,             //노멀 벡터
                                fromAnglePos,           //부채꼴 시작 좌표
                                fov.viewAngle,          //부채꼴 각도
                                fov.viewRange);         //부채꼴 반지름
        //시야각의 텍스트를 표시
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewAngle.ToString());

    }
}
