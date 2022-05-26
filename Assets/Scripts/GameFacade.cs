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

    public void AddRequest(RequestCode requestCode, BaseRequest baseRequest)
    {
        _requestManager.AddRequest(requestCode, baseRequest);
    }

    public void RemoveRequest(RequestCode requestCode)
    {
        _requestManager.RemoveRequest(requestCode);
    }

    public void HandleResponse(RequestCode requestCode, string data)
    {
        _requestManager.HandleResponse(requestCode, data);
    }
}
