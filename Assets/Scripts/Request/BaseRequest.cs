using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class BaseRequest : MonoBehaviour
{
    private RequestCode _requestCode = RequestCode.None;

    public virtual void Awake()
    {
        GameFacade.Instance.AddRequest(_requestCode, this);
    }
    
    public virtual void SendRequest(){ }
    public virtual void OnResponse(string data){ }
    
    public void OnDestroy()
    {
        GameFacade.Instance.RemoveRequest(_requestCode);
    }
}
