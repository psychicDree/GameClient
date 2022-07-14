using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class StartPlayRequest : BaseRequest
{
    private bool isAddControlScript = false;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }

    private void Update()
    {
        if (isAddControlScript)
        {
            facade.AddControlScript();
            isAddControlScript = false;
        }
    }

    public override void OnResponse(string data)
    {
        isAddControlScript = true;
    }
}
