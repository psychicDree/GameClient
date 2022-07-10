using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    private ClientManager _clientManager;
    private AudioManager _audioManager;
    private CameraManager _cameraManager;
    private PlayerManager _playerManager;
    private RequestManager _requestManager;
    private UIManager _uiManager;
    
    private static GameFacade _instance;
    public static GameFacade Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        OnInitManager();
    }

    private void OnInitManager()
    {
        _clientManager = new ClientManager(this);
        _audioManager = new AudioManager(this);
        _cameraManager = new CameraManager(this);
        _playerManager = new PlayerManager(this);
        _requestManager = new RequestManager(this);
        _uiManager = new UIManager(this);
        
        _clientManager.OnInit();
        _audioManager.OnInit();
        _cameraManager.OnInit();
        _playerManager.OnInit();
        _requestManager.OnInit();
        _uiManager.OnInit();
    }

    private void OnDestroy()
    {
        OnDestroyManager();
    }
    private void OnDestroyManager()
    {
        _clientManager.OnDestroy();
        _audioManager.OnDestroy();
        _cameraManager.OnDestroy();
        _playerManager.OnDestroy();
        _requestManager.OnDestroy();
        _uiManager.OnDestroy();
    }

    private void Update()
    {
        UpdateAllManagers();
    }

    private void UpdateAllManagers()
    {
        _clientManager.OnUpdate();
        _audioManager.OnUpdate();
        _cameraManager.OnUpdate();
        _playerManager.OnUpdate();
        _requestManager.OnUpdate();
        _uiManager.OnUpdate();
    }

    public void AddRequest(ActionCode requestCode, BaseRequest baseRequest)
    {
        _requestManager.AddRequest(requestCode, baseRequest);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        _requestManager.RemoveRequest(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        _requestManager.HandleResponse(actionCode, data);
    }

    public void ShowMessage(string msg)
    {
        _uiManager.ShowMessage(msg);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        _clientManager.SendRequest(requestCode, actionCode, data);
    }

    public void SetUserData(UserData userData)
    {
        _playerManager.UserData = userData;
    }

    public UserData GetUserData()
    {
        return _playerManager.UserData;
    }
}
