using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    //파일 입출력을 위한 네임스페이스. 스트림 구축하기 위해 선언
using System.Runtime.Serialization.Formatters.Binary;   //binary 양식을 위한 네임스페이스
using DataInfo;

public class DataManager : MonoBehaviour
{
    [SerializeField] string dataPath;   //저장경로

    public void Initialize()    //저장경로를 초기화하기 위한 함수
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";    //파일 저장 경로 + 파일명 지정
    }

    public void Save(GameData gameData)
    {
        BinaryFormatter binary = new BinaryFormatter();
        FileStream file = File.Create(dataPath);    //데이터 저장을 위한 File 생성

        GameData data = new GameData(); //파일에 저장할 클래스에- 데이터 할당
        data.equipItem = gameData.equipItem;
        data.killcnt = gameData.killcnt;
        data.HP = gameData.HP;
        data.damage = gameData.damage;
        data.speed = gameData.speed;

        binary.Serialize(file, data);
        file.Close();       //닫지 않으면 메모리를 많이 차지한채로 게임을 진행하기 때문에 반드시 닫아야 함        
    }

    public GameData Load()
    {
        if(File.Exists(dataPath))   //존재 유무 파악
        {
            //데이터 불러오기
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);

            GameData data = (GameData)binary.Deserialize(file); //하드디스크에 바이트단위로 저장되더있능ㄴ 데이터를 float, string, int형으로 가져옴(역직렬화)
            file.Close();
            return data;
        }

        else
        {
            //파일이 없는 경우 기본값 반환
            GameData data = new GameData();
            return data;
        }
    }

}
