using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private Button _closeButton;
    private TMP_InputField _usernameIf;
    private TMP_InputField _passwordIf;
    private Button _enterButton;
    private Button _registerButton;
    private LoginRequest _loginRequest;
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnimation();
    }

    private void Start()
    {
        _closeButton = transform.Find("CloseButton").GetComponent<Button>();
        _usernameIf = transform.Find("UsernameLabel/UsernameInput").GetComponent<TMP_InputField>();
        _passwordIf = transform.Find("PasswordLabel/PasswordInput").GetComponent<TMP_InputField>();
        _enterButton = transform.Find("EnterButton").GetComponent<Button>();
        _registerButton = transform.Find("RegisterButton").GetComponent<Button>();
        _loginRequest = GetComponent<LoginRequest>();
        
        _closeButton.onClick.AddListener(OnCloseClick);
        _enterButton.onClick.AddListener(OnEnterClick);
        _registerButton.onClick.AddListener(OnRegisterClick);
    }

    private void OnCloseClick()
    {
        transform.DOScale(Vector3.zero, 0.4f);
       Tweener tweener = transform.DOLocalMove(new Vector3(650, 0, 0), 0.4f);
       tweener.OnComplete(() => uiManager.PopPanel());
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
    private void OnEnterClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(_usernameIf.text)) msg += "please fill username...";
        if (string.IsNullOrEmpty(_passwordIf.text)) msg += "please fill password...";
        if (msg != "")
        {
            uiManager.ShowMessage(msg);
            return;
        }
        _loginRequest.SendRequest(_usernameIf.text,_passwordIf.text);
    }

    private void OnRegisterClick()
    {
        uiManager.PushPanel(UIPanelType.Register);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiManager.ShowMessageSync("Login Successful");
            uiManager.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            uiManager.ShowMessageSync("Credential not matched...");
        }
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
}
