using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class RequestManager : BaseManager
{
    public RequestManager(GameFacade facade) : base(facade) { }
    private Dictionary<RequestCode, BaseRequest> _requestsDict = new Dictionary<RequestCode, BaseRequest>();

    public void AddRequest(RequestCode requestCode, BaseRequest baseRequest)
    {
        _requestsDict.Add(requestCode,baseRequest);
    }
    public void RemoveRequest(RequestCode requestCode)
    {
        _requestsDict.Remove(requestCode);
    }
    public override void OnInit()
    {
        
    }

    public void HandleResponse(RequestCode requestCode, string data)
    {
        BaseRequest request = _requestsDict.TryGet<RequestCode, BaseRequest>(requestCode);
        request.OnResponse(data);
    }

}