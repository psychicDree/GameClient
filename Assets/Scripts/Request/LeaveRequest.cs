using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class LeaveRequest : BaseRequest
{
	private GamePanel gamePanel;
	public override void Awake()
	{
		requestCode = RequestCode.Room;
		actionCode = ActionCode.LeaveRoom;
		gamePanel = GetComponent<GamePanel>();
		base.Awake();
	}
	public void SendResponse()
	{
		base.SendRequest("r");
	}
	public override void OnResponse(string data)
	{
		string[] strs = data.Split(',');
		ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
		string msg = strs[1];
		gamePanel.OnLeaveResponseSync(returnCode,msg);
	}
}
