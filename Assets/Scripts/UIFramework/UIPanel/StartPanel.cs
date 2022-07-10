using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    private Button _loginButton;
    public override void OnEnter()
    {
        _loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        _loginButton.onClick.AddListener(OnLoginClick);
        EnterAnimation();
    }
    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);
        transform.localPosition = new Vector3(650, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void HideAnimation()
    {
        transform.DOScale(0, 0.5f);
        transform.DOLocalMoveX(650, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        EnterAnimation();
    }

    public override void OnPause()
    {
        HideAnimation();
    }
    private void OnLoginClick()
    {
        uiManager.PushPanel(UIPanelType.Login);
    }
}
