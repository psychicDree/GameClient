using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class JoinRoomRequest : BaseRequest
{
    private UserData userData1 = null;
    private UserData userData2 = null;
    private RoomListPanel roomListPanel;

    private void Start()
    {
        roomListPanel = GetComponent<RoomListPanel>();
    }

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        base.Awake();
    }

    public void SendRequest(int id)
    {
        Debug.Log("Joining room of id "+ id);
        base.SendRequest(id.ToString());
    }

    public override void OnResponse(string data)
    {
        //returncode-id,username,totalcount,wincount|id,username,totalcount,wincount
        string[] strs = data.Split('-');
        ReturnCode returnCode = (ReturnCode) int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            string[] udStrArray = strs[1].Split('|');
            userData1 = new UserData(udStrArray[0]);
            userData2 = new UserData(udStrArray[1]);
            roomListPanel.OnJoinResponse(returnCode, userData1, userData2);
        }
    }
}
