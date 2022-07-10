using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    private UserData UserData1;
    private UserData UserData2;
    private RectTransform battleResult;
    private RectTransform roomList;
    private ListRoomRequest _listRoomRequest;
    private GameObject roomItemPrefab;
    private VerticalLayoutGroup roomLayout;
    private List<UserData> _userDatas = null;
    private CreateRoomRequest CreateRoomRequest;
    private JoinRoomRequest JoinRoomRequest;
    private void Start()
    {
        battleResult = transform.Find("BattleResult").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomClick);
        transform.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        _listRoomRequest = GetComponent<ListRoomRequest>();
        CreateRoomRequest = GetComponent<CreateRoomRequest>();
        JoinRoomRequest = GetComponent<JoinRoomRequest>();
    }

    private void Update()
    {
        if (_userDatas != null)
        {
            LoadRoomItem(_userDatas);
            _userDatas = null;
        }

        if (UserData1 != null || UserData2 != null)
        {
            BasePanel panel = uiManager.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResultSync(UserData1, UserData2);
        }
    }

    public override void OnEnter()
    {
        if(battleResult!=null) EnterAnimation();
        SetBattleResult();
        if(_listRoomRequest==null) _listRoomRequest = GetComponent<ListRoomRequest>();
        _listRoomRequest.SendRequest();
    }

    public override void OnPause()
    {
        HideAnimation();
    }

    public override void OnResume()
    {
        EnterAnimation();
        _listRoomRequest.SendRequest();
    }

    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        battleResult.transform.localPosition = new Vector3(-650, 0, 0);
        battleResult.DOLocalMoveX(-170, 0.5f);
        
        roomList.transform.localPosition = new Vector3(650, 0, 0);
        roomList.DOLocalMoveX(95, 0.5f);
    }

    private void HideAnimation()
    {
        battleResult.DOLocalMoveX(-650, 0.5f);
        Tweener tweener = roomList.DOLocalMoveX(650, 0.5f);
        tweener.OnComplete(() => gameObject.SetActive(false));
    }
    private void OnCloseClick()
    {
        uiManager.PopPanel();
    }

    public override void OnExit()
    {
        HideAnimation();
    }
    private void SetBattleResult()
    {
        UserData userData = facade.GetUserData();
        transform.Find("BattleResult/Username").GetComponent<TMP_Text>().text = userData.username;
        transform.Find("BattleResult/TotalCount").GetComponent<TMP_Text>().text = userData.totalCount.ToString();
        transform.Find("BattleResult/WinCount").GetComponent<TMP_Text>().text = userData.winCount.ToString();
    }

    public void LoadRoomItemSync(List<UserData> udList)
    {
        this._userDatas = udList;
    }

    private void LoadRoomItem(List<UserData> udList)
    {
        RoomItem[] roomItems = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (var item in roomItems) item.DestroySelf();
        int count = udList.Count;
        for (int i = 0; i < count; i++)
        {
            RoomItem roomItem = Instantiate(roomItemPrefab, roomLayout.transform).GetComponent<RoomItem>();
            UserData userData = udList[i];
            roomItem.SetRoomItem(userData.id, userData.username, userData.totalCount.ToString(), userData.winCount.ToString(),this);
        }

        int roomCount = GetComponentsInChildren<RoomItem>().Length;
    }

    public void OnJoinClick(int id)
    {
        JoinRoomRequest.SendRequest(id);
    }
    private void OnCreateRoomClick()
    {
        BasePanel panel = uiManager.PushPanel(UIPanelType.Room);
        CreateRoomRequest.SendRequest();
        CreateRoomRequest.SetPanel(panel);
    }

    private void OnRefreshClick()
    {
        _listRoomRequest.SendRequest();
    }

    public void OnJoinResponse(ReturnCode returnCode, UserData userData1, UserData userData2)
    {
        switch (returnCode)
        {
            case ReturnCode.NotFound:
                uiManager.ShowMessageSync("Room Not Found");
                break;
            case ReturnCode.Failed:
                uiManager.ShowMessageSync("Failed to join");
                break;
            case ReturnCode.Success:
                this.UserData1 = userData1;
                this.UserData2 = userData2;
                break;
        }
    }
}
