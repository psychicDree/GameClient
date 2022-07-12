using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class UpdateRoomRequest : BaseRequest
{
	private RoomPanel panel;
	
	public override void Awake()
	{
		requestCode = RequestCode.Room;
		actionCode = ActionCode.UpdateRoom;
		panel = GetComponent<RoomPanel>();
		base.Awake();
	}

	public override void OnResponse(string data)
	{
		UserData ud1, ud2;
		string[] udStrArray = data.Split('|');
		ud1 = new UserData(udStrArray[0]);
		ud2 = new UserData(udStrArray[1]);
		panel.SetAllPlayerResultSync(ud1,ud2);
	}
}
