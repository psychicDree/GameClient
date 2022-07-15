using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class CreateRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;

    private void Start()
    {
        
    }

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        if (returnCode == ReturnCode.Success)
        {
            roomPanel.SetLocalPlayerResultSync();
            facade.SetCurrentRoleType(roleType);
        }
    }

    public void SetPanel(BasePanel panel)
    {
        roomPanel = panel as RoomPanel;
    }
}
