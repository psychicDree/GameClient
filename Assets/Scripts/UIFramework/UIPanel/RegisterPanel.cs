using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private TMP_InputField _usernameIf;
    private TMP_InputField _passwordIf;
    private TMP_InputField _repasswordIf;
    private Button _registerButton;
    private Button _closeButton;
    private RegisterRequest _registerRequest;
    private void Start()
    {
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        _usernameIf = transform.Find("UsernameLabel/UsernameInput").GetComponent<TMP_InputField>();
        _passwordIf = transform.Find("PasswordLabel/PasswordInput").GetComponent<TMP_InputField>();
        _repasswordIf = transform.Find("RePasswordLabel/PasswordInput").GetComponent<TMP_InputField>();
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        _registerRequest = GetComponent<RegisterRequest>();
    }

    private void OnRegisterClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(_usernameIf.text)) msg += "please fill username...";
        if (string.IsNullOrEmpty(_passwordIf.text)) msg += "please fill password...";
        if (_repasswordIf.text != _passwordIf.text) msg += "passwords do not match...";
        if (msg != "")
        {
            uiManager.ShowMessage(msg);
            return;
        }
        _registerRequest.SendRequest(_usernameIf.text,_passwordIf.text);
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Success) uiManager.ShowMessageSync("Registration SuccessFull");
        else uiManager.ShowMessageSync("Registration SuccessFull");
    }
    private void OnCloseClick()
    {
        transform.DOScale(0, 0.5f);
        transform.DOLocalMove(new Vector3(650, 0, 0), 0.5f);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);
        transform.localPosition = new Vector3(650, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
