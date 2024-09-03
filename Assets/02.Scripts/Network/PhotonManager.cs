using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    string version = "V1.0";
    public InputField id;
    public InputField room;
    public Button createBtn;
    public Button randomBtn;
    public Transform rInfo;
    public GameObject rItem;
    readonly string roomItemTag = "R_ITEM";
    Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();

    RoomOptions ro;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = version;
            PhotonNetwork.ConnectUsingSettings();
        }

        ro = new RoomOptions();
    }

    public override void OnConnectedToMaster()
    {
        print("Master server connected");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Joined lobby");
        id.text = GetID();
    }

    #region OnClick 이벤트 함수
    public void OnClickRandomRoomBtn()
    {
        PhotonNetwork.NickName = id.text;
        PlayerPrefs.SetString("ID", id.text);   //ID에 id.text 저장
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickCreateRoomBtn()
    {
        string _room = room.text;
        PhotonNetwork.NickName = id.text;
        PlayerPrefs.SetString("ID", id.text);

        if (string.IsNullOrEmpty(_room))
            _room = "ROOM" + Random.Range(1, 999).ToString("000");

        ro.IsVisible = true;
        ro.IsOpen = true;
        ro.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_room, ro);
    }
    #endregion

    string GetID()
    {
        string userID = PlayerPrefs.GetString("ID");

        if (string.IsNullOrEmpty(userID))
            userID = "USER_" + Random.Range(1, 999).ToString("000");

        return userID;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Join random room failed");

        string _room = room.text;
        if (string.IsNullOrEmpty(_room))
            _room = "ROOM" + Random.Range(1, 999).ToString("000");
        ro.IsVisible = true;
        ro.IsOpen = true;
        ro.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_room, ro);
    }

    public override void OnJoinedRoom()
    {
        print("Joined room");
        StartCoroutine(MainScene());
    }

    IEnumerator MainScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("BattleFieldScene_Backup");
        yield return ao;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        /* GameObject temp = null;

        foreach (var _roomInfo in roomList)
        {
            if(_roomInfo.RemovedFromList)
            {
                //rooms.TryGetValue(roomInfo.Name, out temp);
                Destroy(temp);
                rooms.Remove(_roomInfo.Name);
            }

            else
            {
                if(!rooms.ContainsKey(_roomInfo.Name))
                {
                    GameObject roomPref = Instantiate(rItem, rInfo);
                    roomPref.GetComponent<RoomData>().RCnt = _roomInfo;
                    rooms.Add(_roomInfo.Name, roomPref);
                }

                else
                {
                    rooms.TryGetValue(_roomInfo.Name, out temp);
                    temp.GetComponent<RoomData>().RCnt = _roomInfo
                }
            }

            Debug.Log($"Room = {_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})");
        } */

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(roomItemTag))
        {
            Destroy(obj);
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) continue;

            GameObject room = Instantiate(rItem, rInfo);

            RoomData roomData = room.GetComponent<RoomData>();    //roomItem에 RoomData script 있음
            roomData.roomName = roomInfo.Name;
            roomData.connectPlayer = roomInfo.PlayerCount;
            roomData.maxPlayer = roomInfo.MaxPlayers;
            roomData.DisplayRoomData(); //텍스트 정보를 표시하는 함수

            roomData.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });

            if (roomData.connectPlayer == 0)
                Destroy(room);
        }
    }

    void OnClickRoomItem(string rName)
    {
        PhotonNetwork.NickName = id.text;
        PlayerPrefs.SetString("ID", id.text);   //ID에 id.text 저장
        PhotonNetwork.JoinRoom(rName);
    }
}
