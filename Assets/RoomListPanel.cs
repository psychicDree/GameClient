using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    private RectTransform battleResult;
    private RectTransform roomList;

    private void Start()
    {
        battleResult = transform.Find("BattleResult").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }

    public override void OnEnter()
    {
        if(battleResult!=null) EnterAnimation();
    }

    public override void OnPause()
    {
        base.OnPause();
        HideAnimation();
    }

    public override void OnResume()
    {
        base.OnResume();
        EnterAnimation();
    }

    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        battleResult.transform.localPosition = new Vector3(-650, 0, 0);
        battleResult.DOLocalMoveX(-170, 0.5f);
        
        roomList.transform.localPosition = new Vector3(650, 0, 0);
        roomList.DOLocalMoveX(95, 0.5f);
    }

    private void HideAnimation()
    {
        battleResult.DOLocalMoveX(-650, 0.5f);
        Tweener tweener = roomList.DOLocalMoveX(650, 0.5f);
        tweener.OnComplete(() => gameObject.SetActive(false));
    }
    private void OnCloseClick()
    {
        uiManager.PopPanel();
    }

    public override void OnExit()
    {
        HideAnimation();
    }
}
