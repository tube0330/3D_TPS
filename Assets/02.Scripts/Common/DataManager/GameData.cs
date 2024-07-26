using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class GameData   //기능적 요소보다 데이터적 성격이 강한 클래스로 Entity 클래스라고 한다.
    {
        public List<Item> equipItem = new List<Item>();
        public int killcnt = 0;
        public float HP = 120f;
        public float damage = 25;
        public float speed = 6.0f;
    }

    [System.Serializable]
    public class Item
    {
        public enum ITEMTYPE {HP, SPEED, GRENADE, DAMAGE}   //canvasUI_item에 있는 아이템 종류 선언
        public enum ITEMCALC {VALUE, PERSENT};  //아이템 계산 방식

        public ITEMTYPE itemtype;   //아이템 종류
        public ITEMCALC itemcal;    //아이템 계산 종류
        public string name;         //아이템 이름
        public string desc;         //아이템 소개
        public float value;         //아이템 값
    }
}