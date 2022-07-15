using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class ListRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        if(data == "0") return;
        List<UserData> udList = new List<UserData>();
        string[] userdataArray = data.Split('|');
        foreach (var userdata in userdataArray)
        {
            string[] strs = userdata.Split(',');
            udList.Add(new UserData(int.Parse(strs[0]), username: strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
            roomListPanel.LoadRoomItemSync(udList);
        }
    }
}
