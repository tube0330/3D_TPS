using System.Collections;
using System.Collections.Generic;
using DataInfo;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager G_Instance;
    public bool isGameOver = false;
    public Text killTxt;
    public int killCnt = 0;

    [Header("DataManager")]
    [SerializeField] DataManager dataManager;
    //public GameData gameData;
    public GameDataObject gameData; //위의 방법 대신 Attribute를 사용한 방법

    //인벤토리 아이템이 변경(추가, 삭제)되었을 때 발생 시킬 이벤트 정의
    public delegate void ItemChangedDelegate();
    public static event ItemChangedDelegate OnItemChange;
    [SerializeField] private GameObject slotList;
    public GameObject[] itemObjects;

    [Header("Network")]
    public GameObject playerPrefab;
    public Text CurPlayer;
    public Text logText;

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
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitForSeconds(1f);
        float pos = Random.Range(0, 3f);
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(2f, 0f, 0f), Quaternion.identity);
        player.transform.position = new Vector3(pos, 0.3f, pos);
        Debug.Log($"Player instantiated at {player.transform.position}");
    }

    void LoadGameData()
    {
        //killCnt = PlayerPrefs.GetInt("KILLCOUNT", 0);

        #region 하드디스크에 저장된 데이터 넘어오는중, 데이터 초기
        /* GameData data = dataManager.Load();
        gameData.HP = data.HP;
        gameData.damage = data.damage;
        gameData.killcnt = data.killcnt;
        gameData.equipItem = data.equipItem;
        gameData.speed = data.speed; */
        #endregion

        if (gameData.equipItem.Count > 0)
            InventorySetUp();

        //killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");    //자릿수설정
        killTxt.text = $"<color=#ff0000>KILL</color> " + gameData.killCnt.ToString("0000");
    }

    void InventorySetUp()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();

        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            for (int j = 1; j < slots.Length; j++)  //j=1 -> slotlist(부모) 빼고 하려고
            {
                if (slots[j].childCount > 0) continue;

                int itemIdx = (int)gameData.equipItem[i].itemtype;  //보유한 item 종류에 따라 인덱스 추출
                itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[j].transform);   //item의 부모는 slot이 됨
                itemObjects[itemIdx].GetComponent<ItemInfo>().itemData = gameData.equipItem[i]; //item의 iteminfo 클래스의 itemData에 로드한 gameData.equopItem[i] 데ㅇㅣ터 값을 저장

                break;  //데이터값 저장할 곳 찾았으니까 이제 빠져나옴
            }
        }
    }

    void SaveGameData()
    {
        //dataManager.Save(gameData);
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);   //.asset 파일에 데이터 저장
        #endif   //.asset 파일에 데이터 저장
    }

    public void AddItem(Item item)   //인벤토리에서 아이템을 추가했을 때 데이터 정보를 업데이트하는 함수
    {
        if (gameData.equipItem.Contains(item)) return;   //item 비교해서 원래 있던 item과 같으면 추가 안함

        gameData.equipItem.Add(item);

        switch (item.itemtype)
        {
            case Item.ITEMTYPE.HP:
                if (item.itemcal == Item.ITEMCALC.VALUE)
                {
                    gameData.HP += item.value;  //더하는 방식일때
                    Debug.Log("HP추가");
                }

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
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);   //.asset 파일에 데이터 저장
        #endif
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
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);   //.asset 파일에 데이터 저장
        #endif
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

        gameData.killCnt++;
        killTxt.text = $"<color=#ff0000>KILL</color> " + gameData.killCnt.ToString("0000");
        //PlayerPrefs.SetInt("KILLCOUNT", killCnt);
    }

    void OnApplicationQuit() => SaveGameData();    //게임이 끝났을 때 자동호출

    [PunRPC]
    void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        CurPlayer.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //SetRoomInfo();
        photonView.RPC(nameof(SetRoomInfo), RpcTarget.All);
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> Enter room";

        photonView.RPC("UpdateLogText", RpcTarget.All, msg);    // 모든 클라이언트에서 UpdateLogText RPC 호출
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        //SetRoomInfo();
        photonView.RPC(nameof(SetRoomInfo), RpcTarget.All);
        string msg = $"\n<color=red>{player.NickName}</color> Left room";

        photonView.RPC("UpdateLogText", RpcTarget.All, msg);    // 모든 클라이언트에서 UpdateLogText RPC 호출
    }

    public override void OnLeftRoom() => SceneManager.LoadScene("StartScene");

    [PunRPC]
    void UpdateLogText(string msg) => logText.text += msg;
}
