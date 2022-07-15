using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class GameOverRequest : BaseRequest
{
    private GamePanel panel;
    private ReturnCode returnCode;
    private bool isGameOver = false;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameOver;
        panel = GetComponent<GamePanel>();
        base.Awake();
    }

    private void Update()
    {
        if (isGameOver)
        {
            panel.OnGameOverResponse(returnCode);
            isGameOver = false;
        }
    }

    public override void OnResponse(string data)
    {
        returnCode  = (ReturnCode)int.Parse(data);
        isGameOver = true;
    }
}
