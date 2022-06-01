using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    private RegisterPanel _registerPanel;
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        _registerPanel = GetComponent<RegisterPanel>();
        base.Awake();
    }

    public void SendRequest(string username,string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        _registerPanel.OnRegisterResponse(returnCode);
    }
}
