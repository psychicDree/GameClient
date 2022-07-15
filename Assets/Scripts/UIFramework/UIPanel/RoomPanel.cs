using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    private TMP_Text localPlayerUsername, localPlayerTotalCount, localPlayerWinCount;
    private TMP_Text enemyPlayerUsername, enemyPlayerTotalCount, enemyPlayerWinCount;
    private Transform BluePanel, RedPanel;
    private UserData UserData;
    private UserData ud1 = null;
    private UserData ud2 = null;
    private StartGameRequest startGameRequest;
    private void Start()
    {
        startGameRequest = GetComponent<StartGameRequest>();
        localPlayerUsername = transform.Find("BlueTeam/Username").GetComponent<TMP_Text>();
        localPlayerTotalCount = transform.Find("BlueTeam/TotalCount").GetComponent<TMP_Text>();
        localPlayerWinCount = transform.Find("BlueTeam/WinCount").GetComponent<TMP_Text>();
        enemyPlayerUsername = transform.Find("RedTeam/Username").GetComponent<TMP_Text>();
        enemyPlayerTotalCount = transform.Find("RedTeam/TotalCount").GetComponent<TMP_Text>();
        enemyPlayerWinCount = transform.Find("RedTeam/WinCount").GetComponent<TMP_Text>();
        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartClick);
    }

    public override void OnEnter()
    {
        BluePanel = transform.Find("BlueTeam").GetComponent<Transform>();
        RedPanel = transform.Find("RedTeam").GetComponent<Transform>();
        EnterAnim();
    }

    private void Update()
    {
        if (UserData != null)
        {
            SetLocalPlayerResult(UserData.username,UserData.totalCount,UserData.winCount);
            ClearEnemyPlayerResult();
            UserData = null;
        }

        if (ud1 != null)
        {
            SetLocalPlayerResult(ud1.username,ud1.totalCount,ud1.winCount);
            if(ud2!=null)
                SetEnemyPlayerResult(ud2.username,ud2.totalCount,ud2.winCount);
            else ClearEnemyPlayerResult();
            ud1 = null;
            ud2 = null;
        }
    }

    public override void OnPause()
    {
        ExitAnim();
    }

    public override void OnExit()
    {
        ExitAnim();
    }

    public void SetLocalPlayerResultSync()
    {
        UserData = facade.GetUserData();
    }

    public void SetAllPlayerResultSync(UserData ud1, UserData ud2)
    {
        this.ud1 = ud1;
        this.ud2 = ud2;
    }
    private void SetLocalPlayerResult(string username, int totalCount, int winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = totalCount.ToString();
        localPlayerWinCount.text = winCount.ToString();
    }
    private void SetEnemyPlayerResult(string username, int totalCount, int winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = totalCount.ToString();
        enemyPlayerWinCount.text = winCount.ToString();
    }
    private void ClearEnemyPlayerResult()
    {
        enemyPlayerUsername.text = "waiting..";
        enemyPlayerTotalCount.text = "--";
        enemyPlayerWinCount.text = "--";
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);
        BluePanel.localPosition = new Vector3(-1000, 0, 0);
        BluePanel.DOLocalMoveX(-115, 0.5f);
        RedPanel.localPosition = new Vector3(1000, 0, 0);
        RedPanel.DOLocalMoveX(115, 0.5f);
    }

    private void ExitAnim()
    {
        BluePanel.DOLocalMoveX(-1000, 0.5f);
        RedPanel.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }
    public void OnStartGameResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiManager.PushPanelSync(UIPanelType.Game);
            facade.EnterPlayingSync();
        }
        else
        {
            uiManager.ShowMessageSync("You are not host.");
        }
    }
}
