using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    private int id;
    public TMP_Text username;
    public TMP_Text totalCount;
    public TMP_Text winCount;
    public Button joinButton;
    private RoomListPanel _roomListPanel;
    void Start()
    {
        if(_roomListPanel==null)
            _roomListPanel = GetComponent<RoomListPanel>();
        joinButton?.onClick.AddListener(OnJoinClick);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    private void OnJoinClick()
    {
        _roomListPanel.OnJoinClick(id);
    }

    public void SetRoomItem(int id, string username, string totalCount, string winCount,RoomListPanel roomListPanel)
    {
        this.id = id;
        this.username.text = username;
        this.totalCount.text = totalCount;
        this.winCount.text = winCount;
        this._roomListPanel = roomListPanel;
    }
}
