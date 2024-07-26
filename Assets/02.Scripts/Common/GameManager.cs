using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_Instance;
    public bool isGameOver = false;
    public Text killTxt;
    public int killCnt = 0;

    [Header("Datamanager")]
    [SerializeField] DataManager dataManager;
    public GameData gameData;

    //인벤토리 아이템이 변경되었을 때 발생 시킬 이벤트 정의
    public delegate void ItemChangedDelegate();
    public static event ItemChangedDelegate OnItemChange;
    [SerializeField] private GameObject slotList;
    public GameObject[] itemObjects;

    void Awake()
    {
        if (G_Instance == null)
            G_Instance = this;

        else if (G_Instance != this)
            Destroy(gameObject);

        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

        killTxt = GameObject.Find("Canvas_UI").transform.GetChild(7).GetComponent<Text>();

        DontDestroyOnLoad(gameObject);
        OnCloseClick(false);
        LoadGameData();
    }

    void LoadGameData()
    {
        //killCnt = PlayerPrefs.GetInt("KILLCOUNT", 0);

        #region 하드디스크에 저장된 데이터 넘어오는중
        GameData data = dataManager.Load();
        gameData.HP = data.HP;
        gameData.damage = data.damage;
        gameData.killcnt = data.killcnt;
        gameData.equipItem = data.equipItem;
        gameData.speed = data.speed;
        #endregion

        //killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");    //자릿수설정

        killTxt.text = $"<color=#ff0000>KILL</color> " + gameData.killcnt.ToString("0000");
    }

    void SaveGameData()
    {
        dataManager.Save(gameData);
    }

    public void AddItem(Item item)   //인벤토리에서 아이템을 추가했을 때 데이터 정보를 업데이트하는 함수
    {
        if (gameData.equipItem.Contains(item)) return;   //item 비교해서 원래 있던 item과 같으면 추가 안함

        gameData.equipItem.Add(item);

        switch (item.itemtype)
        {
            case Item.ITEMTYPE.HP:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                    gameData.HP += item.value;  //더하는 방식일때

                else
                    gameData.HP += gameData.HP * item.value;    //곱하는 방식일때
                break;

            case Item.ITEMTYPE.SPEED:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                    gameData.speed += item.value;  //더하는 방식

                else
                    gameData.speed += gameData.speed * item.value;    //곱하는 방식
                break;

            case Item.ITEMTYPE.DAMAGE:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                    gameData.damage += item.value;  //더하는 방식

                else
                    gameData.damage += gameData.damage * item.value;    //곱하는 방식
                break;

            case Item.ITEMTYPE.GRENADE:

                break;
        }

        OnItemChange(); //아이템 변경된 것을 실시간으로 반영하기 위해 호출
    }

    public void RemoveItem(Item item)   //인벤토리에서 아이템을 뺐을 때 데이터 정보를 업데이트하는 함수
    {
        gameData.equipItem.Remove(item);

        switch (item.itemtype)
        {
            case Item.ITEMTYPE.HP:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                    gameData.HP -= item.value;  //더하는 방식일때

                else
                    gameData.HP = gameData.HP / (1.0f + item.value);    //곱하는 방식일때
                break;

            case Item.ITEMTYPE.SPEED:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                    gameData.speed -= item.value;  //더하는 방식

                else
                    gameData.speed = gameData.speed / (1.0f + item.value);    //곱하는 방식
                break;

            case Item.ITEMTYPE.DAMAGE:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                    gameData.damage -= item.value;  //더하는 방식

                else
                    gameData.damage = gameData.damage / (1.0f + item.value);    //곱하는 방식
                break;

            case Item.ITEMTYPE.GRENADE:

                break;
        }

        OnItemChange(); //아이템 변경된 것을 실시간으로 반영하기 위해 호출
    }

    public bool isPause = false;

    public void OnPauseClick()
    {
        isPause = !isPause;

        Time.timeScale = (isPause) ? 0.0f : 1f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>(); //player에 있는 MonoBehaviour를 상속하는 스크립트들을 가져옴

        foreach (var script in scripts)
        {
            script.enabled = !isPause;
        }

        var canvasGroup = GameObject.Find("Panel_Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPause;
    }

    public void OnCloseClick(bool isOpen)
    {
        var canvasGroup = GameObject.Find("Inventory").GetComponent<CanvasGroup>();

        Time.timeScale = (isOpen) ? 0f : 1f;

        canvasGroup.alpha = (isOpen) ? 1f : 0f;
        canvasGroup.interactable = isOpen;
        canvasGroup.blocksRaycasts = isOpen;
    }

    public void KillScore()
    {
        /* ++killCnt;
        killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");

        PlayerPrefs.SetInt("KILLCOUNT", killCnt); */

        gameData.killcnt++;
        killTxt.text = $"<color=#ff0000>KILL</color> " + gameData.killcnt.ToString("0000");
        //PlayerPrefs.SetInt("KILLCOUNT", killCnt);
    }

    void OnDisable()    //게임 종료하면 자동으로 호출
    {
        //PlayerPrefs.DeleteKey("KILLCOUNT");
    }

    void OnApplicationQuit()    //게임이 끝났을 때 자동호출
    {
        SaveGameData();
    }
}
