using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 1)] //메뉴에 있는 Asset 누르면 바로 위에 menuName으로 지정한 이름의 창이 생김. 선택하면 fileName으로 지정한 이름의 Object가 생김

public class GameDataObject : ScriptableObject
{
    public int killCnt = 0;
    public float HP= 120f;
    public float damage = 25;
    public float speed = 6.0f;
    public List<Item> equipItem = new List<Item>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
